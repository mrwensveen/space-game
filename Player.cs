using Godot;

namespace SpaceGame;

public partial class Player : CharacterBody2D
{
	private int _acceleration = 5;
	private Vector2 _velocity = Vector2.Zero;
	private float _angularSpeed = Mathf.Pi;

	public Player()
	{
		GD.Print("Hello World!");
	}

	public override void _Process(double delta)
	{
		var direction = 0;
		if (Input.IsActionPressed("p1_left"))
		{
			direction = -1;
		}
		if (Input.IsActionPressed("p1_right"))
		{
			direction = 1;
		}

		Rotation += _angularSpeed * direction * (float)delta;

		if (Input.IsActionPressed("p1_up"))
		{
			_velocity += Vector2.Up.Rotated(Rotation) * _acceleration;
		}
		if (Input.IsActionPressed("p1_down"))
		{
			_velocity += Vector2.Down.Rotated(Rotation) * _acceleration / 5;
		}

		_velocity = _velocity.LimitLength(1000);
		_velocity *= .999f;

		Position += _velocity * (float)delta;
	}
}
