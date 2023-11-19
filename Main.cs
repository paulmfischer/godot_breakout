using Godot;
using System;

public partial class Main : Node
{
	[Export]
	public PackedScene BallScene { get; set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
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
}
