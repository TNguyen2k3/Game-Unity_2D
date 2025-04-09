const mongoose = require('mongoose');

const userSchema = new mongoose.Schema({
    name: {type: String, unique: true, required: true },
    gmail: { type: String, required: true },
    username: { type: String, unique: true, required: true },
    otp: { type: String, required: true },
    otpExpired: { type: Date, required: true }
});

// Kiểm tra model trước khi khai báo
const User = mongoose.models.User || mongoose.model('User', userSchema);
module.exports = User;
