# Circus Charlie: Reimagined

## Overview

A modern recreation of the classic 1980s Konami arcade game, **Circus Charlie**, built in Unity. 
In this project, you play as Charlie the clown, riding a lion and jumping through flaming hoops. While staying true to the nostalgic retro feel and core mechanics of the original game, this version introduces a **brand-new surprise mechanic**: a deployable Shield to help navigate the circus chaos!

---

## 🎮 Instructions

### Core Movement
- **Move Left**: `Arrow Left` / `A`
- **Move Right**: `Arrow Right` / `D`
- **Jump**: `Spacebar` / `W` / `Arrow Up`

### 🛡️ The Surprise Mechanic: Energy Shield
Timing your jumps perfectly can be tough. I introduced a custom **Shield Mechanic** that allows Charlie to temporarily absorb a hit from the flaming pots or hoops. The shild is gaind after every 3 successfull hoop jumps.

---

## 🎪 Level Design & Hazards

- **Flaming Pots**: Static hazards placed along the track.
- **Fire Hoops**: Moving obstacles that Charlie must time his jumps to pass through safely.
- **Money Bags / Coins**: Collectibles scattered throughout the level to increase your high score.
- **The Podium**: Reaching the final podium triggers the stage's win condition.

---

## ⚙️ Technical Highlights

This project served as a foundational exploration of game architecture in Unity, focusing on modularity and clean code:

- **Custom Game Mechanics**: Extended the original game's logic by implementing a custom, toggleable Energy Shield system integrated directly with the core movement and collision scripts.
- **Debug & Testing Environment**: Built a suite of developer shortcuts (e.g., instant win, spawning entities, hard state resets) to facilitate rapid testing, edge-case resolution, and iteration during development.
- **Object Pooling**: Implemented a pooling system for dynamic entities like Fire Hoops to optimize memory allocation and prevent Garbage Collection spikes during runtime.
---

## 🛠️ Debug & Development Tools

To facilitate rapid testing and iteration during development, the following debug shortcuts were implemented:

- `Left Shift + 1`: Move Charlie directly to the win podium.
- `Left Shift + 2`: Start the game instantly.
- `Left Shift + 3`: Pause / Unpause the game.
- `Left Shift + 4`: Activate Shield
- `Left Shift + 5`: Deactivate Shield
- `Left Shift + 6`: End the stage and return to the Main Menu.
- `Left Shift + 7`: Spawn collectible coins from all active Flaming Pots.
- `Left Shift + 8`: Hard reset the game state.

---

## 🎨 Art & Audio

- **Visuals**: Classic 2D sprite work aiming to capture the 8-bit aesthetic of the 1984 original.
- **Audio**: Nostalgic 8-bit sound effects for jumping, collecting coins, and taking damage, tied directly to the game's event system.

---

## 💡 Notes

- Created within the Department of Visual Communication at Bezalel Academy of Arts and Design, in collaboration with The Hebrew University of Jerusalem
