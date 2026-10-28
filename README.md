# Delta Time ⏱️

A neon-themed 2D roguelike top-down shooter developed for **İSTÜN Game Jam**. Features dynamic time-scaling mechanics, procedural projectile ricochets, and a system upgrade loop[cite: 5].

<img width="862" height="502" alt="Delta Time Banner" src="https://github.com/user-attachments/assets/8a4ff5e7-4430-4d63-977a-93413f1bbbe9" />

> 🕹️ **Play / Download:** [Itch.io Page]([https://itch.io](https://ozyilmaz.itch.io/delta-time))



---

## 🎮 Game Overview & Mechanics

**Delta Time** combines top-down shooter dynamics with a tactical time-scaling rule: **time only moves when the player moves**[cite: 5]. Players navigate an arena filled with geometric viruses, collect "Bits", and upgrade their setup to survive[cite: 5].

- **Input-Driven Time Scale:** Game time dynamically scales based on player movement vectors, allowing precise aiming and tactical decision-making[cite: 5].
- **Weapon & Physics Mechanics:** Includes ricocheting bullets, multishots, and time-freezing power-ups[cite: 5].
- **In-Game Shop & Upgrades:** Spend collected "Bits" between waves to overclock system stats and unlock abilities[cite: 5].

---

## 🛠️ Technical & Architecture Highlights

- **Custom Time Management:** Overridden delta-time calculation logic bound directly to movement input instead of fixed framerate updates[cite: 5].
- **Event-Driven Shop System:** Decoupled upgrade system handling passive stat adjustments and active modifiers dynamically.
- **Object Pooling:** Efficient spawning and recycling for high-density enemy waves and projectile physics.
- **Rapid Prototyping:** Built under Game Jam time constraints with a focus on core loop stability and visual feedback[cite: 5].

---

## 🚧 Development Status & Future Roadmap

> **Note:** The core gameplay loop and mechanics are complete and fully functional[cite: 5]. The project is currently receiving active updates and feature expansions.

### 📋 Planned Features & Updates
- [x] Core time-scaling movement and shooting engine[cite: 5]
- [x] In-game shop & system upgrade loops[cite: 5]
- [x] Basic enemy types & ricochet mechanics[cite: 5]
- [ ] **Android / Mobile Port:** On-screen virtual joystick & touch control UI adaptation
- [ ] Additional enemy archetypes & boss encounters
- [ ] Expanded shop items, weapons, and passive perks
- [ ] Audio & SFX polish

---

## 📸 Screenshots

<img width="1921" height="1081" alt="Gameplay 1" src="https://github.com/user-attachments/assets/f0d1b49b-57d8-4589-93d0-633725905994" />

<img width="2559" height="1599" alt="Gameplay 2" src="https://github.com/user-attachments/assets/53c0d4ca-018a-41cc-9d7d-b90b57da2a61" />

<img width="2560" height="1600" alt="Gameplay 3" src="https://github.com/user-attachments/assets/44e33fc9-2f6c-496b-b789-3902dcca18bf" />

---

## 🚀 Built With & Target Platforms

- **Engine:** Unity
- **Language:** C#
- **Platforms:** PC (Windows Standalone)[cite: 5] / WebGL *(Android support coming soon)*
