const mongoose = require('mongoose');

const achievementSchema = new mongoose.Schema({
    name: { type: String, unique: true, required: true, ref: 'User' }, // Foreign Key
    totalScore: { type: Number, required: true,  default: 0},
    levelPassed: { type: Number, required: true, default: 0},
    completedLevels: {
    type: [
        {
            levelName: String,
            score: Number,
            completedAt: Date
        }
    ],
    default: []}
    
});
// Kiểm tra model trước khi khai báo
const Achievement = mongoose.models.Achievement || mongoose.model('Achievement', achievementSchema);
module.exports = Achievement;
