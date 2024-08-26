using Godot;
using System;

namespace SpaceGame.Scenes.Surface;

public partial class Player : Node2D
{
	private const int SPEED = 300;

	private Sprite2D? _sprite2D;

	public override void _Ready()
	{
		_sprite2D = GetNode<Sprite2D>("Sprite2D");
	}

	public override void _Process(double delta)
	{
		var direction = Input.GetAxis("p1_left", "p1_right");

		Position = new Vector2(Position.X + direction * SPEED * (float)delta, Position.Y);

		if (direction != 0)
		{
			_sprite2D!.FlipH = direction < 0;
		}
	}
}
