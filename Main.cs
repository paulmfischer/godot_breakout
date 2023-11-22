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
	public PackedScene PlayerScene { get; set; }
	[Export]
	public float PlayerBottomOffset { get; set; } = 75f;
	[Export]
	public float TopBrickOffset { get; set; } = 75f;
	[Export]
	public float BrickPadding { get; set; } = 25.0f;
	[Export]
	public int BrickRows { get; set; }
	[Export]
	public int BrickCols { get; set; }

	private Vector2 ScreenSize; // Size of the game window.
	private int Score = 0;

	public override void _Ready()
	{
		ScreenSize = GetViewport().GetVisibleRect().Size;
		// InstantiateBricks();
		// InstantiateBall();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionPressed("escape"))
		{
			GetTree().Quit();
		}
	}

	private async void NewGame()
	{
		Score = 0;
		var hud = GetNode<HUD>("HUD");
		hud.UpdateScore(Score);
		hud.ShowMessage("Get Ready!");
		InstantiatePlayer();
		InstantiateBricks();
		InstantiateBall();
		GetTree().Paused = true;
		await ToSignal(GetTree().CreateTimer(2.0), SceneTreeTimer.SignalName.Timeout);
		GetTree().Paused = false;
	}

	public void OnBallExited()
	{
		// InstantiateBall();
		GetNode<HUD>("HUD").ShowGameOver();
		Cleanup();
	}

	private void CheckIfLevelOver()
	{
		Score++;
		var hud = GetNode<HUD>("HUD");
		hud.UpdateScore(Score);
		if (Score >= (BrickRows * BrickCols))
		{
			Cleanup();
			hud.ShowGameOver();
		}
	}

	public void InstantiatePlayer()
	{
		AddChild(PlayerScene.Instantiate<Player>());
	}

	public void InstantiateBall()
	{
		Ball ball = BallScene.Instantiate<Ball>();
		ball.OnBallExited += OnBallExited;
		AddChild(ball);
	}

	public void InstantiateBricks()
	{
		int numOfBricksToCreate = BrickRows * BrickCols;
		GD.Print("Instantiate bricks");
		var bricks = new Godot.Collections.Array<Brick>();
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
				brick.OnBrickRemoved += CheckIfLevelOver;
				brick.Position = new Vector2(
					x: positionX,
					y: positionY
				);
				GD.PrintS("matrix position", row, col, "index access", row * BrickCols + col, "position", brick.Position);
				AddChild(brick);
			}
		}
	}

	private void Cleanup()
	{
		GetNode<Player>("Player").QueueFree();
		GetNode<Ball>("Ball").QueueFree();
		var bricks = this.GetChildByType<Brick>();

		GD.PrintS("Remaining bricks to free up", bricks.Count());
		foreach (var brick in bricks)
		{
			brick.QueueFree();
		}
	}
}
