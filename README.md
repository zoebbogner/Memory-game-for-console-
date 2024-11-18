# Memory Game Project

## Overview
The **Memory Game** is a console-based application written in **C#**, offering an engaging way to test your memory skills. You can play with another person or challenge the computer's AI, which has three different levels of difficulty. The project focuses on object-oriented design principles, ensuring a clear separation between the game's logic and its user interface.

---

## Features
- **Two Game Modes**: Play against another human or compete with the AI.
- **AI Opponent**: Choose from three levels of difficulty:
  - Easy: The AI makes random moves.
  - Medium: The AI remembers some previously revealed cards.
  - Hard: The AI keeps track of all revealed cards for strategic moves.
- **Dynamic Board Sizes**: Customize the grid size to increase or decrease the challenge.
- **Score Tracking**: Displays scores at the end of the game.

---

## Gameplay
1. **Start the Game**:
   - Choose the mode (Player vs Player or Player vs AI).
   - If Player vs AI, select the difficulty level.
2. **Reveal Cards**:
   - Players take turns flipping two cards to find matching pairs.
   - Matches are rewarded with an extra turn.
3. **Winning the Game**:
   - The game ends when all pairs are matched.
   - The player with the most matches wins!

---

## Technical Highlights
- **Object-Oriented Design**:
  - The game uses clean separation between core game logic and the console-based user interface.
  - Encapsulation and polymorphism enhance code maintainability and flexibility.
- **AI Implementation**:
  - The AI uses decision-making algorithms to simulate memory and logic at different skill levels.
- **User Input Validation**:
  - Ensures smooth gameplay by handling incorrect or invalid inputs gracefully.

---

## How to Run
### Prerequisites
- **.NET SDK** (6.0 or later)
- A code editor or terminal that supports .NET applications (e.g., Visual Studio, Visual Studio Code, or CLI).

### Steps
1. Clone this repository:
   ```bash
   git clone <repository-url>
   ```
2. Navigate to the project directory:
   ```bash
   cd MemoryGame
   ```
3. Build the application:
   ```bash
   dotnet build
   ```
4. Run the application:
   ```bash
   dotnet run
   ```

---

## Future Improvements
- Add a graphical user interface (GUI) for a more immersive experience.
- Implement a multiplayer online mode.
- Include a leaderboard to track high scores.
- Expand AI difficulty levels with more advanced strategies.

---

## Contributing
Contributions are welcome! If you want to enhance the project or fix issues:
1. Fork the repository.
2. Create a new branch for your feature:
   ```bash
   git checkout -b feature-name
   ```
3. Commit your changes:
   ```bash
   git commit -m "Add your message here"
   ```
4. Push the branch:
   ```bash
   git push origin feature-name
   ```
5. Submit a pull request.

---

## License
This project is licensed under the [MIT License](LICENSE).

---

## Contact
For any questions or feedback, feel free to reach out via email or open an issue in this repository.

Enjoy the game and challenge your memory!
