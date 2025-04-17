const mongoose = require('mongoose');

const achievementSchema = new mongoose.Schema({
    name: { type: String, unique: true, required: true, ref: 'User' }, // Foreign Key
    totalScore: { type: Number, required: true },
    levelPassed: { type: Number, required: true },
    rank: { type: String, required: true }
});
// Kiểm tra model trước khi khai báo
const Achievement = mongoose.models.Achievement || mongoose.model('Achievement', achievementSchema);
module.exports = Achievement;
