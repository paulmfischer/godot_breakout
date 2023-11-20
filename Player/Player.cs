using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[Export]
	public float Speed { get; set; } = 20.0f;
	[Export]
	public float PaddleBottomOffset { get; set; } = 0.10f;

	public Vector2 ScreenSize; // Size of the game window.

	public override void _Ready()
	{
		base._Ready();
		ScreenSize = GetViewportRect().Size;
		var bottomOffset = ScreenSize.Y * PaddleBottomOffset;
		Position = new Vector2(
			x: ScreenSize.X / 2,
			y: ScreenSize.Y - bottomOffset
		);
		GD.PrintS("Player initial position:", Position);
	}

	public override void _PhysicsProcess(double delta)
	{
		// Let player collide with walls and stop with physics!!!
		CalculateVelocityFromInput();
		var collision = MoveAndCollide(Velocity * (float)delta);
		if (collision != null)
		{
			// GD.PrintS("Collided with", ((Node)collision.GetCollider()).Name);
			var bottomOffset = ScreenSize.Y * PaddleBottomOffset;
			Position = new Vector2(Position.X, ScreenSize.Y - bottomOffset);
		}
	}

	public void CalculateVelocityFromInput()
	{
		var inputDir = Vector2.Zero;

		if (Input.IsActionPressed("move_right"))
		{
			inputDir.X += 1;
		}

		if (Input.IsActionPressed("move_left"))
		{
			inputDir.X -= 1;
		}

		if (inputDir.Length() > 0)
		{
			inputDir = inputDir.Normalized() * Speed;
		}
		Velocity = inputDir * Speed;
	}
}
