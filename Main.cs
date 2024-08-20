using Godot;

namespace SpaceGame;

public partial class Main : Node2D
{
	public override void _Ready()
	{
		var size = GetViewportRect().Size * Scale.Inverse();
		GetNode<TextureRect>("Background").Size = size;
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventKey eventKey
			&& eventKey.Pressed
			&& eventKey.Keycode == Key.Escape)
		{
			GetTree().Quit();
		}
	}
}
