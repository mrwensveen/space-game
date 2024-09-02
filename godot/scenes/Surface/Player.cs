using Godot;
using System;

namespace SpaceGame.Scenes.Surface;

public partial class Player : Node2D
{
	private const int SPEED = 300;

	// private Sprite2D? _sprite2D;
	private CharacterBody2D? _human;

	public override void _Ready()
	{
		// _sprite2D = GetNode<Sprite2D>("Sprite2D");
		_human = GetNode<CharacterBody2D>("Human");
	}

	public override void _Process(double delta)
	{
		var direction = Input.GetAxis("p1_left", "p1_right");

		Position = new Vector2(Position.X + direction * SPEED * (float)delta, Position.Y);

		var animationPlayer = _human!.GetNode<AnimationPlayer>("AnimationPlayer");

		if (direction == 0 && animationPlayer.IsPlaying())
		{
			animationPlayer.Pause();
		}
		else if (direction != 0 && !animationPlayer.IsPlaying())
		{
			animationPlayer.Play();
		}

		if (direction != 0)
		{
			Scale = Scale with { X = Mathf.Abs(Scale.X) * (direction < 0 ? -1 : 1) };
		}
	}
}
