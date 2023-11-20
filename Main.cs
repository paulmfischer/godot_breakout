using Godot;
using System;
using System.Linq;

public partial class Main : Node
{
	[Export]
	public PackedScene BallScene { get; set; }
	[Export]
	public PackedScene BrickScene { get; set; }
	// [Export]
	// public float TopPadding { get; set; }
	// [Export]
	// public float SidePadding { get; set; }
	[Export]
	public float BrickPadding { get; set; } = 5.0f;
	[Export]
	public int BrickRows { get; set; }
	[Export]
	public int BrickCols { get; set; }

	public Vector2 ScreenSize; // Size of the game window.
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
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

	public void InstantiateBricks()
	{
		GD.PrintS("Instantiate Bricks");
		var numOfBricksToCreate = BrickRows * BrickCols;
		GD.PrintS("numOfBricks", numOfBricksToCreate);
		// var bricks = Enumerable.Range(0, numOfBricksToCreate).Select(x => BrickScene.Instantiate<Brick>());
		Godot.Collections.Array<Brick> bricks = new Godot.Collections.Array<Brick>();
		GD.Print("Array created");
		for (int i = 0; i < numOfBricksToCreate; i++)
		{
			GD.Print("insert array data");
			bricks.Add(BrickScene.Instantiate<Brick>());
		}
		ScreenSize = GetViewport().GetVisibleRect().Size;
		GD.PrintS("Bricks?", bricks.Count, bricks.ElementAt(0));
		var brickSprite = bricks[0].GetNode<Sprite2D>("Sprite2D");
		var size = brickSprite.Texture.GetSize();
		GD.PrintS("single brick size", size);
		var brickWidth = size.X + BrickPadding;
		var brickHeight = size.Y + BrickPadding;
		var gridWidth = brickWidth* BrickCols;
		var gridHeight = brickHeight * BrickRows;
		var colStart = (ScreenSize.X / 2) - (gridWidth / 2);
		var rowStart = (ScreenSize.Y / 2) - (gridHeight * 0.75);
		GD.PrintS("brick with padding", brickWidth, brickHeight, "grid size", gridWidth, gridHeight, "starting positions", colStart, rowStart);

		for (int row = 0; row < BrickRows; row++)
		{
			for (int col = 0; col < BrickCols; col++)
			{
				var positionX = colStart + (brickWidth * col);
				var positionY = (float)(rowStart + (brickHeight * row));
				GD.PrintS("brick position", row, col, "index access", row * numOfBricksToCreate + col, positionX, positionY);
				Brick brick = bricks[row * numOfBricksToCreate + col];
				brick.Position = new Vector2(
					x: positionX,
					y: positionY
				);
				AddChild(brick);
			}
		}
	}
}
