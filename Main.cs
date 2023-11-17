using Godot;
using System;

public partial class Main : Node
{
	// [Export]
	// public PackedScene BallScene { get; set; }

	// [Export]
	// public PackedScene PlayerScene { get; set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Ball ball = BallScene.Instantiate<Ball>();
		// ball.Position = new Vector2(400, 400);
		// ball.LinearVelocity = new Vector2(0, 34);
		// // var velocity = new Vector2()
		// GD.PrintS("Menu Ball initial position", ball.Position);
		// AddChild(ball);
	
		// Player player = PlayerScene.Instantiate<Player>();
		// GD.PrintS("Menu Player initial position", player.Position);
		// AddChild(player);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionPressed("escape"))
		{
			GetTree().Quit();
		}
	}
}
