const jwt = require('jsonwebtoken');
const User = require('../Models/User');

const authMiddleware = async (req, res, next) => {
    try {
        const token = req.header("Authorization")?.replace("Bearer ", "");
        if (!token) {
            return res.status(401).json({ message: "Access Denied! No token provided." });
        }

        const decoded = jwt.verify(token, process.env.JWT_SECRET);
        const user = await User.findById(decoded.id).select("-otp -otpExpired"); // Lấy thông tin user từ DB

        if (!user) {
            return res.status(404).json({ message: "User not found!" });
        }

        req.user = user; // Gán thông tin user vào request
        req.token = token; // Lưu token để sử dụng sau này
        next(); 
    } catch (err) {
        console.log(err);
        return res.status(403).json({ message: "Invalid or expired token!" });
    }
};

module.exports = authMiddleware;
