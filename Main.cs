using Godot;
using System;

public partial class Main : Node
{
	[Export]
	public PackedScene BallScene { get; set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ResetBall();
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
		ResetBall();
	}
	public void ResetBall()
	{
		Ball ball = BallScene.Instantiate<Ball>();
		ball.OnBallExited += OnBallExited;
		ball.Position = new Vector2(400, 480);
		ball.LinearVelocity = new Vector2(200, -200);
		AddChild(ball);
	}
}
