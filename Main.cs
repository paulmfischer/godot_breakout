using Godot;
using System;
using System.Linq;

public partial class Main : Node
{
	[Export]
	public PackedScene BallScene { get; set; }
	[Export]
	public PackedScene BrickScene { get; set; }
	[Export]
	public float TopBrickOffset { get; set; } = 50f;
	[Export]
	public float BrickPadding { get; set; } = 25.0f;
	[Export]
	public int BrickRows { get; set; }
	[Export]
	public int BrickCols { get; set; }

	public Vector2 ScreenSize; // Size of the game window.
	public int Score = 0;

	public override void _Ready()
	{
		ScreenSize = GetViewport().GetVisibleRect().Size;
		InstantiateBricks();
		InstantiateBall();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionPressed("escape"))
		{
			GetTree().Quit();
		}
	}

	public void OnBallExited()
	{
		InstantiateBall();
	}

	public void InstantiateBall()
	{
		Ball ball = BallScene.Instantiate<Ball>();
		ball.OnBallExited += OnBallExited;
		AddChild(ball);
	}

	private void CheckIfLevelOver()
	{
		Score++;
		GD.PrintS("Number of bricks removed", Score);
		if (Score >= (BrickRows * BrickCols))
		{
			GD.PrintS("Pausing game");
			GetTree().Paused = true;
		}
	}

	public void InstantiateBricks()
	{
		int numOfBricksToCreate = BrickRows * BrickCols;
		Godot.Collections.Array<Brick> bricks = new Godot.Collections.Array<Brick>();
		for (int i = 0; i < numOfBricksToCreate; i++)
		{
			bricks.Add(BrickScene.Instantiate<Brick>());
		}
		Sprite2D brickSprite = bricks[0].GetNode<Sprite2D>("Sprite2D");
		Vector2 spriteSize = brickSprite.Texture.GetSize();
		GD.PrintS("single brick size", spriteSize);
		float brickWithPaddingWidth = spriteSize.X + BrickPadding;
		float brickWithPaddingHeight = spriteSize.Y + BrickPadding;
		float gridWidth = brickWithPaddingWidth * BrickCols;
		float colStart = (ScreenSize.X / 2) - (gridWidth / 2) + (brickWithPaddingWidth / 2);
		GD.PrintS("brick with padding", brickWithPaddingWidth, brickWithPaddingHeight, "grid size", gridWidth, "starting positions", colStart, TopBrickOffset);

		for (int row = 0; row < BrickRows; row++)
		{
			for (int col = 0; col < BrickCols; col++)
			{
				float positionX = colStart + (brickWithPaddingWidth * col);
				float positionY = (float)(TopBrickOffset + (brickWithPaddingHeight * row));
				Brick brick = bricks[row * BrickCols + col];
				brick.TreeExited += CheckIfLevelOver;
				brick.Position = new Vector2(
					x: positionX,
					y: positionY
				);
				GD.PrintS("matrix position", row, col, "index access", row * BrickCols + col, "position", brick.Position);
				AddChild(brick);
			}
		}
	}
}
