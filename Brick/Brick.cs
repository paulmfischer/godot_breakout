using Godot;
using System;

public partial class Brick : StaticBody2D
{
	[Signal]
	public delegate void OnBrickRemovedEventHandler();

	[Export]
	public double RemoveTimer { get; set; } = 1;

	public void Remove()
	{
		var timer = GetNode<Timer>("RemoveBrickTimer");
		timer.Start(RemoveTimer);
	}

	public void OnRemoveBrickTimerTimeout()
	{
		QueueFree();
		EmitSignal(SignalName.OnBrickRemoved);
	}
}
