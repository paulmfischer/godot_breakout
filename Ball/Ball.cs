using Godot;
using System;

public partial class Ball : RigidBody2D
{
	[Signal]
	public delegate void OnBallExitedEventHandler();

	public void OnScreenExited()
	{
		GD.PrintS("Ball Screen Exited");
		QueueFree();
		EmitSignal(SignalName.OnBallExited);
	}
}
