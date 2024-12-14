# Snake2D

• Snake Game with Power-Ups and Dynamic Food Spawning

This project is a classic Snake Game implemented in Unity, with enhanced features such as dynamic food spawning, power-ups, and additional mechanics for an engaging gameplay experience.

Features

Core Gameplay
• Snake Movement:
The snake moves in a grid-based environment with smooth directional changes using arrow keys or WASD. The snake grows as it consumes food.
• Collision Handling:
Collisions with walls, obstacles, or the snake itself can result in game reset unless a shield power-up is active.

Dynamic Food Mechanics
• Mass Gainer and Mass Burner Foods:
    • Mass Gainer: Increases the length of the snake.
    • Mass Burner: Decreases the length of the snake by a specified amount, preventing the snake from growing excessively.
    • Auto-Destruction: Food items disappear after a set lifespan if not consumed.
    • Random Positioning: Ensures food does not spawn on occupied grid cells.

Power-Ups
• Power-ups randomly spawn on the grid and grant temporary benefits:
    • Shield: Protects the snake from collisions for a limited time.
    • Speed Boost: Increases the snake's movement speed for a short duration.
    • Score Boost: Provides bonus points for consuming food while active.

Wall Traversal
• Allows the snake to pass through walls and appear on the opposite side of the grid if enabled.

Modular Design
• All functionalities are split into distinct components for better code maintainability:
    • SnakeController: Handles snake movement, growth, shrinkage, and collision.
    • Food: Manages food spawning, lifespan, and interactions.
    • PowerUpController: Handles power-up behaviors and triggers.

How It Works

1. Snake Movement:
The snake moves one grid cell at a time based on the chosen direction. Its body segments follow the head's movement.

2. Food Interaction:
When the snake consumes food, it either grows or shrinks based on the type of food. New food spawns randomly on the grid.

3. Power-Up Activation:
Power-ups trigger temporary effects, enhancing gameplay dynamics.

4. Collision Reset:
The game resets if the snake collides with an obstacle, wall (when wall traversal is disabled), or itself without an active shield.

Code Highlights
Dynamic Food and Power-Up Spawning
• Foods and power-ups spawn at random positions within defined grid boundaries.
• Collision checks ensure they do not overlap with the snake's body.

Snake Growth and Shrinkage
• The snake dynamically adds or removes segments based on interactions, with robust boundary checks to prevent errors.

Power-Up System
• Implements coroutine-based timers for temporary power-up effects, ensuring smooth transitions and time-based logic.