const authMiddleware = require("../Middlewares/authMiddleware");
const express = require('express');
const bcrypt = require('bcryptjs');
const jwt = require('jsonwebtoken');
const User = require('../Models/User');
const sendMail = require("../utils/sendMail");
const bodyParser = require('body-parser');
const fs = require('fs');
const path = require('path');
const zlib = require('zlib');
const Achievement = require("../Models/Achievement");
require("dotenv").config();
const router = express.Router();


// Đăng ký tài khoản
router.post('/register',  async (req, res) => {
    try {
        const { nickname, gmail, username } = req.body;
        const otpExpired = new Date(Date.now());
        const existingUser = await User.findOne({ $or: [{ username: username }, { gmail: gmail }, {name: nickname}] });

        if (existingUser) {
            console.log("Username already exists!");
            return res.status(400).json({ message: "Username already exists!" });
        }
        const hashedPassword = await bcrypt.hash("********", 10);
        const newUser = new User({ name: nickname, gmail, username, otp: hashedPassword, otpExpired });
        const newAchievement = new Achievement({name: nickname})
        await newAchievement.save();
        await newUser.save();
        res.status(201).json({ message: "User registered successfully!" });
    } catch (error) {
        res.status(500).json({ error: error.message });
    }
});

// Đăng nhập (not used)
router.post('/login', async (req, res) => {
    try {
        const { username, otp } = req.body;
        console.log(username, password);
        const user = await User.findOne({
            $or: [{ username: username }, { gmail: username }]
        });
        if (!user) return res.status(404).json({ message: "User not found!" });

        const isMatch = await bcrypt.compare(otp, user.otp);
        if (!isMatch) return res.status(401).json({ message: "Invalid credentials!" });

        const token = jwt.sign({ id: user._id }, process.env.JWT_SECRET, { expiresIn: "1h" });

        res.status(200).json({ token, user });
    } catch (error) {
        console.log(error.message);
        res.status(500).json({ error: error.message });
    }
});

// Đây là danh sách token đã bị thu hồi (nếu bạn không dùng Redis)
const revokedTokens = new Set();

// Đăng xuất (Logout)
router.post("/logout", authMiddleware, (req, res) => {
    const token = req.header("Authorization")?.replace("Bearer ", "");

    console.log("Received Token:", token); 
    if (!token) {
        console.log("No token provided");
        return res.status(400).json({ message: "No token provided" });
    }

    try {
        const decoded = jwt.verify(token, process.env.JWT_SECRET);
        
        // Thu hồi token bằng cách lưu vào danh sách revokedTokens
        revokedTokens.add(token);
        
        return res.json({ message: "Logged out successfully" });
    } catch (err) {
        console.log("Invalid token");
        return res.status(400).json({ message: "Invalid token" });
    }
});

const generateOTP = () => Math.floor(100000 + Math.random() * 900000).toString();

router.post("/request-otp", async (req, res) => {
    const { username } = req.body;

    try {
        const otp = generateOTP();
        const salt = await bcrypt.genSalt(10);
        const hashedOTP = await bcrypt.hash(otp, salt);
        const otpExpired = new Date(Date.now() + 5 * 60 * 1000); // Hết hạn sau 5 phút

        // Tìm user theo gmail hoặc username
        let user = await User.findOne({ $or: [{ gmail: username }, { username: username }] });

        if (user) {
            // Nếu tìm thấy user, cập nhật OTP
            user.otp = hashedOTP;
            user.otpExpired = otpExpired;
            await user.save();
            // Gửi OTP qua email
            await sendMail(user.gmail, `Mã OTP của bạn là: ${otp}`);
        } 
        console.log("message: OTP đã được gửi!");

        return res.json({ success: true, message: `OTP ${otp} đã được gửi!` });
    } catch (error) {
        console.error(error);
        return res.status(500).json({ success: false, message: "Lỗi khi gửi OTP!", error });
    }
});

router.post("/verify-otp", async (req, res) => {
    const { username, otp } = req.body;

    if (!username || !otp) {
        return res.status(400).json({ success: false, message: "Vui lòng nhập username hoặc OTP!" });
    }

    try {
        // Tìm user theo email hoặc username
        const user = await User.findOne({ $or: [{ gmail: username }, { username: username }] });
        
        if (!user) {
            return res.status(404).json({ success: false, message: "Người dùng không tồn tại!" });
        }

        // Kiểm tra xem OTP có hợp lệ không
        if (!user.otp || !user.otpExpired || new Date() > user.otpExpired) {
            console.log(otp + " is expired or invalid")
            return res.status(400).json({ success: false, message: "OTP không hợp lệ hoặc đã hết hạn!" });
        }

        // So sánh OTP đã nhập với OTP trong DB
        const isMatch = await bcrypt.compare(otp, user.otp);

        if (!isMatch) {
            console.log("Mã OTP sai!");
            return res.status(400).json({ success: false, message: "Mã OTP sai!" });
        }

        // Xác thực thành công -> Tạo token, xóa OTP cũ
        await User.updateOne(
            { _id: user._id },
            { $unset: { otp: "", otpExpired: "" } } // Xóa OTP sau khi xác thực
        );
        
        const token = jwt.sign({ id: user._id, email: user.gmail, name: user.name }, process.env.JWT_SECRET, { expiresIn: "7d" });
        console.log(token);
        return res.json({ success: true, message: "Xác thực OTP thành công!", token });
    } catch (error) {
        console.error(error);
        return res.status(500).json({ success: false, message: "Lỗi xác thực OTP!", error });
    }
});

router.get('/play', authMiddleware, (req, res) => {
    res.json({
        message: "Authorized to play!",
        user: {
            username: req.user.username,
            email: req.user.email,
        }
    });
});


router.use(bodyParser.urlencoded({ extended: true, limit: '10mb' }));
router.use(bodyParser.json({ limit: '10mb' }));


// Route để nhận dữ liệu từ client và lưu vào file
router.post('/save-level', authMiddleware, (req, res) => {
    const { levelData, nickname } = req.body;
    const levelsDir = path.join(__dirname, '../Levels');
    const filePath = path.join(levelsDir, `${nickname}_${levelData.levelName}.json`);

    // Đảm bảo thư mục Levels tồn tại
    fs.mkdir(levelsDir, { recursive: true }, (err) => {
        if (err) {
            console.error('Lỗi khi tạo thư mục:', err);
            return res.status(500).json({ message: 'Không thể tạo thư mục lưu trữ' });
        }

        // Ghi file sau khi chắc chắn thư mục tồn tại
        fs.writeFile(filePath, JSON.stringify(levelData, null, 2), (err) => {
            if (err) {
                console.error('Lỗi khi lưu file:', err);
                return res.status(500).json({ message: 'Lưu thất bại' });
            }
            res.status(200).json({ message: 'Lưu thành công' });
        });
    });
});

router.get('/get-level-list', authMiddleware, (req, res) => {
    // const nickname = req.query.nickname; // hoặc lấy từ token nếu muốn bảo mật hơn
    const levelsDir = path.join(__dirname, '../Levels');

    fs.readdir(levelsDir, (err, files) => {
        if (err) {
            console.error('Lỗi khi đọc thư mục:', err);
            return res.status(500).json({ message: 'Không thể đọc thư mục levels' });
        }

        const userFiles = files.filter(file => file.endsWith('.json'));
        const levelNames = userFiles;

        res.status(200).json({ levels: levelNames });
    });
});

router.get('/get-level-data', authMiddleware, (req, res) => {
    const { levelName } = req.body;
    const filePath = path.join(__dirname, '../Levels', `${levelName}.json`);

    fs.readFile(filePath, 'utf8', (err, data) => {
        if (err) {
            console.error('Lỗi khi đọc file:', err);
            return res.status(404).json({ message: 'Level không tồn tại' });
        }

        try {
            const jsonData = JSON.parse(data);
            res.status(200).json(jsonData);
        } catch (parseErr) {
            console.error('Lỗi khi parse JSON:', parseErr);
            res.status(500).json({ message: 'Dữ liệu không hợp lệ' });
        }
    });
});

router.post('/update-achivement', authMiddleware,  async (req, res) => {
    const {levelName, score, nickname} = req.body;
    
    const achievement = await Achievement.findOne({
        name: nickname,
        "completedLevels.levelName": levelName
    });
   
    var existingLevel;
    if (achievement){
        existingLevel = achievement.completedLevels.find(lv => lv.levelName === levelName);
    }
    if (existingLevel) {
        if (score > existingLevel.score) {
            const scoreDiff = score - existingLevel.score;

            await Achievement.updateOne(
                { name: nickname, "completedLevels.levelName": levelName },
                {
                    $set: {
                        "completedLevels.$.score": score,
                        "completedLevels.$.completedAt": new Date()
                    },
                    $inc: { totalScore: scoreDiff }
                }
            );
        } else {
            // Không cần update nếu score thấp hơn
            console.log('Score mới không cao hơn score cũ');
        }
    } else {
        await Achievement.updateOne(
            { name: nickname },
            {
                $push: {
                    completedLevels: {
                        levelName: levelName,
                        score: score,
                        completedAt: new Date()
                    }
                },
                $inc: {
                    totalScore: score,
                    levelPassed: 1
                }
            }
        );
    }
    res.sendStatus(200);

})

router.get('/get-your-profile', async (req, res) => {
    const {nickname} = req.body;
    const achievement = await Achievement.findOne({name: nickname});
    const user = await User.findOne({
        name: nickname
    })
    if (achievement && user){
        res.json({
            message: `Here is ${nickname} 's data`,
            name: nickname,
            username: user.username,
            gmail: user.gmail,
            totalScore: achievement.totalScore,
            levelPassed: achievement.levelPassed,
            completedLevels: achievement.completedLevels
        })
    }
    else return res.status(404).json("User not found");
})

router.delete('/delete-your-level', async (req, res) => {
    try {
        const { levelName } = req.body; // Lấy levelName từ body
        console.log(levelName);
        if (!levelName) {
            return res.status(400).json({ message: 'Missing levelName in request body.' });
        }

        // Đường dẫn đến file level
        const filePath = path.join(__dirname, '../Levels', `${levelName}.json`);
        console.log(filePath);
        // Kiểm tra file có tồn tại không
        if (!fs.existsSync(filePath)) {
            return res.status(404).json({ message: 'Level file not found.' });
        }

        // Xóa file
        fs.unlinkSync(filePath);

        return res.status(200).json({ message: `Level '${levelName}' deleted successfully.` });
    } catch (err) {
        console.error(err);
        return res.status(500).json({ message: 'Error deleting level file.' });
    }
})

module.exports = router;
