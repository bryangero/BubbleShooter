Border.cs
is for the bounce of the bubble on side walls.

Bubble.cs
main script for bubble.
ValidateNeighbors() main checker for same color bubbles. called from source bubble and spreads to neighboring bubbles.

GameManager.cs
Setup for common delegates and events.
RunThroughBubbleMatrix() starts iteration through bubbles for validation on bubble landed.

GameUI.cs
script for UI in main scene.

HexGrid.cs
instantiates initial grid. 
HexOffset(int x, int y) computes hex position of bubbles.

MenuUI.cs
script for UI in menu scene.

ScoreDisplay.cs
Animates and display scores on hit. value of score varies on number of bubbles popped.

Shooter.cs
shooter behavior in game.