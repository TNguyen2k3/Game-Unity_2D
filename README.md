# Angry Birds Clone with Custom Level Editor

🎮 A Unity-based Angry Birds-style game featuring a fully functional Level Editor. Players can play classic levels or design their own custom ones!

---

## 📌 Features

- 🔥 Realistic 2D physics-based gameplay like original Angry Birds.
- 🐥 Multiple bird types with unique abilities.
- 🎯 Enemy pigs with destructible structures.
- 🧰 **Level Editor**:
  - Click and drop enemies, and objects.
  - Choose birds from the panel.
  - Save, load, and delete custom levels.
  - Automatically validate and only allow playable levels to be uploaded. (in progress)
  - Rate difficulty of user-generated levels. (in progress)
- ☁️ JSON-based level data storage for flexibility and sharing.

---

## 🚀 Getting Started

### ✅ Requirements
- Unity (version 2022 or newer recommended)
- .NET Framework (auto-installed with Unity)

### 🔧 How to Run

1. Clone the repo:
   ```bash
   git clone https://github.com/TNguyen2k3/AngryBirds.git
2. Open the project in Unity.

3. Open the scene Home.

Press Play to run the game or Edit to start designing your own level!

🧪 Level Editor Usage
Open the Level Editor from the main menu.

Use the toolbar to:

Add birds, pigs, and structures.

cut, copy, paste or delete selected objects.

Click Save to serialize your level to JSON.

Play the level immediately or share it with others.

📁 Level data is saved to:
Assets/StreamingAssets/CustomLevelData.json
Each object is stored in a compact format:

{
  "data": "prefabName@@x,y,z@@x,y,z@@type"
}
📦 Folder Structure
Angry Birds/
├── Assets/
│   ├── Assets
│   ├── Scripts/
│   ├── Scenes/
│   ├── Resources/
│   └── StreamingAssets/CustomLevelData.json
├── README.md
And some miscellaneous folder
