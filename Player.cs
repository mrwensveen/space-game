using Godot;

namespace SpaceGame;

public partial class Player : CharacterBody2D
{
	private const int ACCELERATION = 5;
	private const float ANGULAR_SPEED = Mathf.Pi;

	private Vector2 _velocity = Vector2.Zero;

	[Signal]
	public delegate void HealthDepletedEventHandler();

	public Player()
	{
		GD.Print("Hello World!");
	}

	public override void _Ready()
	{
		GD.Print("Ready!");
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

		Rotation += ANGULAR_SPEED * direction * (float)delta;

		if (Input.IsActionPressed("p1_up"))
		{
			_velocity += Vector2.Up.Rotated(Rotation) * ACCELERATION;
		}
		if (Input.IsActionPressed("p1_down"))
		{
			_velocity += Vector2.Down.Rotated(Rotation) * ACCELERATION / 5;
		}

		_velocity = _velocity.LimitLength(1000);
		_velocity *= .9995f;

		Position += _velocity * (float)delta;
	}
}
