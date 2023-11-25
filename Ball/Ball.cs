using Godot;
using System;

public partial class Ball : RigidBody2D
{
	[Signal]
	public delegate void OnBallExitedEventHandler();

	[Export]
	public float Speed { get; set; } = 250.0f;
	[Export]
	public float BallBottomOffset { get; set; } = 0.15f;

	public Vector2 ScreenSize; // Size of the game window.

	public override void _Ready()
	{
		base._Ready();
		Start();
	}

	public void OnScreenExited()
	{
		GD.PrintS("Ball Screen Exited");
		QueueFree();
		EmitSignal(SignalName.OnBallExited);
	}

	public void OnBodyEntered(Node body)
	{
		if (body is Brick)
		{
			(body as Brick).Remove();
		}
	}

	public void Start()
	{
		ScreenSize = GetViewportRect().Size;
		var bottomOffset = ScreenSize.Y * BallBottomOffset;
		Position = new Vector2(
			x: ScreenSize.X / 2,
			y: ScreenSize.Y - bottomOffset
		);
		Vector2 leftVector = new Vector2(0, 0) - Position;
		Vector2 rightVector = new Vector2(ScreenSize.X, 0) - Position;
		RandomNumberGenerator random = new();
		Vector2 initialVector = new Vector2(random.RandfRange(leftVector.X, rightVector.X), random.RandfRange(leftVector.Y, rightVector.Y)).Normalized();
		GD.PrintS("random vector start", initialVector, initialVector * Speed);
		LinearVelocity = initialVector * Speed;
		GD.PrintS("Ball start position:", Position, "linear velocity", LinearVelocity);
	}
}
