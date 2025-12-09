# starlight-engine

Implementes Features:

1. Card Flipping System
- Clicking a card flips it over.
- Animations included for natural flipping.
- Cards cannot be flipped when:
  - They are already matched
- Player can continue flipping without waiting for full evaluation (continuous flipping supported).

2. Card Matching Logic
- Cards are matched using their ID.
- If two flipped cards match:
  - They trigger a “match” animation.
  - They fade out and are removed from the board.
- If they do not match:
  - They flip back automatically.
  - Wrong-match sound feedback is given.

3. Dynamic Layout Support
Implemented card layouts:
- Easy: 2×2  
- Medium: 4×4  
- Hard: 5×6  

The cardArea automatically:
- Resizes cards  
- Maintains spacing  
- Fits the screen reliably  

4. Scoring System  
Base scoring with speed-based multiplier:

| Match Speed | Multiplier | Points |
| Under 1 sec |     ×3     |   +3   |
| Under 3 sec |     ×2     |   +2   |
| Over 3 sec  |     ×1     |   +1   |

Evaluation includes:
- Move counter
- Real-time score updates
- Total match score displayed at win screen


6. Timer System
- Real-time gameplay timer
- Displays seconds elapsed
- Starts only after the preview phase ends
- Saved per difficulty to show best time

7. Game Preview Phase  
At game start:
- All cards reveal their fronts briefly  
- Allows players to memorize layout  
- Cards flip back automatically  
- Timer begins only afterward  

8. Menu System   
This project includes:
- Main Menu  
- Difficulty Selection (Easy / Medium / Hard)  
- Transition into game  
- High score display on win screen  
- Restart button  

9. Audio Feedback System
Includes:
- Flip sound  
- Correct match sound  
- Wrong match sound  
- End-screen celebration sound  
- Audio does not overlap excessively  


10. Win Screen & High Score Saving
At the end of the game:
- Gameplay stops  
- Win panel appears  
- Shows:
  - Moves  
  - Score  
  - Time  
  - Best moves per difficulty  
  - Best time per difficulty  

Uses PlayerPrefs for persistent saving.


GAMEPLAY FLOw
1. Choose difficulty from Main Menu  
2. Cards are generated dynamically  
3. Preview phase shows all card faces  
4. Timer begins  
5. Player matches cards  
6. Score updates using speed multiplier  
7. Game ends when all pairs are matched  
8. Win screen shows stats + high scores  

TECHNICAL SUMMARY

Scripts:
- Card.cs – Handles flipping, animations, matching, fading, UI linking  
- CardManager.cs – Core gameplay logic, scoring, timer, grid generation, sound, high score saving  
- MainMenu.cs – Difficulty selection, game startup  

Features:
- Coroutines for smooth animations  
- Speed-based scoring  
- Adaptive UI  
- Multi-difficulty support  
- Fully dynamic card creation  

Assets Used:
- Custom emoji-style card sprites  
- UI buttons & panels  
- Audio effects (flip, correct, wrong, win sound)  
