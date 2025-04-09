const nodemailer = require("nodemailer");

// Cấu hình transporter (thay đổi thông tin email của bạn)
const transporter = nodemailer.createTransport({
    service: "gmail",
    auth: {
        user: "nguyen.vu1692003@hcmut.edu.vn", // Thay bằng email của bạn
        pass: "yita xwak kyqz gppa", // Thay bằng mật khẩu ứng dụng của Gmail
    },
});

/**
 * Hàm gửi email OTP
 * @param {string} to - Email người nhận
 * @param {string} message - Nội dung email
 */
const sendMail = async (to, message) => {
    const mailOptions = {
        from: '"Game Support" <nguyen.vu1692003@gmail.com>',
        to: to,
        subject: "Mã OTP của bạn",
        text: message,
    };

    try {
        await transporter.sendMail(mailOptions);
        console.log("✅ Email sent successfully!");
    } catch (error) {
        console.error("❌ Error sending email:", error);
        throw error;
    }
};

module.exports = sendMail;