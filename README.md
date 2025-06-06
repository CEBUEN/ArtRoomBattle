## 🎮 Description

**ArtRoomBattle** is a 1v1 LAN-based multiplayer shooter developed in Unity using C#. Players enter a classroom-themed arena and battle using projectile attacks until one player runs out of health. Built as a networking test project, it uses Unity’s **Netcode for GameObjects** to handle player synchronization, movement, and combat over the network.

## 📖 Project Overview

**ArtRoomBattle** is a small-scale PvP prototype created to explore Unity’s built-in multiplayer tools. The game features two players connecting over a local network, each with their own health bar and ability to shoot projectiles. When a player’s health reaches zero, they lose the match. This project emphasizes lightweight combat mechanics, synchronized network behavior, and basic game state tracking.

## 🕹️ Gameplay Features

- **1v1 Networked Combat**  
  Two players connect over LAN and battle in a small map.

- **Projectile Shooting System**  
  Players can shoot projectiles to damage opponents.

- **Health-Based Win Condition**  
  Players lose when their health reaches zero.

- **Networked Object Sync**  
  Uses Unity’s Netcode for GameObjects to synchronize players and projectiles.

## 🎮 Controls

- `WASD` — Move player  
- `Mouse Left Click` — Shoot projectile  

## 🧰 Tech Stack

- **Engine**: Unity  
- **Networking**: Unity Netcode for GameObjects  
- **Language**: C#  
- **Platform**: Windows (PC)  
- **Visuals/Effects**: ShaderLab and basic Unity materials  
