using Godot;

namespace SpaceGame;

public partial class Player : Area2D
{
	private const int ACCELERATION = 5;
	private const int MAX_VELOCITY = 2000;
	private const float ANGULAR_SPEED = Mathf.Pi;

	private Vector2 _velocity = Vector2.Zero;

	[Signal]
	public delegate void HitEventHandler();

	public void Start(Vector2 position)
	{
		GD.Print($"Start! ({position})");

		Position = position;
		Show();
		GetNode<CollisionPolygon2D>("CollisionPolygon2D").Disabled = false;
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

		_velocity = _velocity.LimitLength(MAX_VELOCITY);
		_velocity *= .9995f;

		Position += _velocity * (float)delta;

		var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		if (Input.IsActionPressed("p1_up"))
		{
			animatedSprite2D.Animation = "thrust";
		}
		else
		{
			animatedSprite2D.Animation = "idle";
		}

		if (!animatedSprite2D.IsPlaying())
		{
			animatedSprite2D.Play();
		}
	}

	private void OnBodyEntered(Node2D body)
	{
		Hide(); // Player disappears after being hit.
		EmitSignal(SignalName.Hit);

		// Must be deferred as we can't change physics properties on a physics callback.
		GetNode<CollisionPolygon2D>("CollisionPolygon2D").SetDeferred(CollisionPolygon2D.PropertyName.Disabled, true);
	}
}
