using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[Export]
	public float Speed { get; set; } = 300.0f;
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
		var velocity = Vector2.Zero;

		if (Input.IsActionPressed("move_right"))
		{
			velocity.X += 1;
		}

		if (Input.IsActionPressed("move_left"))
		{
			velocity.X -= 1;
		}

		if (velocity.Length() > 0)
		{
			velocity = velocity.Normalized() * Speed;
		}

		var sprite = GetNode<Sprite2D>("Sprite2D");
		var spriteHalfWidth = sprite.Texture.GetWidth() * sprite.Scale.X / 2;
		Position += velocity * (float)delta;
		// Position = new Vector2(
		// 	x: Mathf.Clamp(Position.X, 0 + spriteHalfWidth, ScreenSize.X - spriteHalfWidth),
		// 	y: Position.Y
		// );
	}
}
