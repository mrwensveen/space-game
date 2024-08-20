using Godot;

namespace SpaceGame;

public partial class Player : Area2D
{
	private const float ANGULAR_SPEED = Mathf.Pi;

	[Export]
	private float _acceleration = 2;

	[Export]
	private float _deceleration = 1;

	[Export]
	private int _maxVelocity = 2000;

	private Vector2 _velocity = Vector2.Zero;

	[Signal]
	public delegate void HitEventHandler();

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
			_velocity += Vector2.Up.Rotated(Rotation) * _acceleration;
		}
		if (Input.IsActionPressed("p1_down"))
		{
			_velocity += Vector2.Down.Rotated(Rotation) * _deceleration;
		}

		_velocity = _velocity.LimitLength(_maxVelocity);
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

	private void OnScreenExited()
	{
		var size = GetViewportRect().Size * GetGlobalTransform().Scale.Inverse();
		GD.Print(Position, size);

		if (Position.X < 0 || Position.X > size.X)
		{
			Position = new Vector2((Position.X - size.X) * -1, Position.Y);
		}

		if (Position.Y < 0 || Position.Y > size.Y)
		{
			Position = new Vector2(Position.X, (Position.Y - size.Y) * -1);
		}
	}

	private void OnBodyEntered(Node2D body)
	{
		//Hide(); // Player disappears after being hit.
		EmitSignal(SignalName.Hit);

		// Must be deferred as we can't change physics properties on a physics callback.
		GetNode<CollisionPolygon2D>("CollisionPolygon2D").SetDeferred(CollisionPolygon2D.PropertyName.Disabled, true);
	}
}
