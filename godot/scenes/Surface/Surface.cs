using Godot;
using System;

public partial class Surface : Node2D
{
	public override void _Input(InputEvent inputEvent)
	{
		if (inputEvent is InputEventKey eventKey
			&& eventKey.Keycode == Key.Escape)
		{
			GetTree().Quit();
		}
	}
}
