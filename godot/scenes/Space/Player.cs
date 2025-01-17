using System;
using Godot;

namespace SpaceGame.Scenes.Space;

public partial class Player : Area2D
{
	private const float HALF_PI = Mathf.Pi / 2;
	private const float ANGULAR_SPEED = Mathf.Pi;

	[Export] private float _acceleration = 2;

	[Export] private float _deceleration = 1;

	[Export] private int _maxVelocity = 2000;

	[Export] private bool _invincible = true;

	[Signal] public delegate void DiedEventHandler(Player player);

	[Signal] public delegate void LandedEventHandler(Player player);

	private Vector2 _velocity;

	public void Start(Vector2 position, float rotation, int velocity)
	{
		Position = position;
		Rotation = rotation;

		_velocity = Vector2.Up.Rotated(Rotation) * velocity;
		_invincible = true;

		var invincibleTimer = GetNode<Timer>("InvincibleTimer");
		if (invincibleTimer != null && invincibleTimer.IsInsideTree() && invincibleTimer.IsStopped())
		{
			invincibleTimer.Start();
		}

		Show();
		SetProcess(true);
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
			animatedSprite2D.Animation = "default";
		}

		if (!animatedSprite2D.IsPlaying())
		{
			animatedSprite2D.Play();
		}
	}

	private void OnScreenExited()
	{
		var size = GetViewportRect().Size * GetGlobalTransform().Scale.Inverse();

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
		if (_invincible) return;

		// Disable collisions and stop processing

		// Must be deferred as we can't change physics properties on a physics callback.
		GetNode<CollisionPolygon2D>("CollisionPolygon2D")
			.SetDeferred(CollisionPolygon2D.PropertyName.Disabled, true);

		SetProcess(false);

		// Has the player landed safely or died?
		var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

		var angle = (Position - body.Position).Angle();
		var diff = Mathf.Abs(Mathf.Wrap(Rotation - HALF_PI - angle, -Mathf.Pi, Mathf.Pi));

		GD.Print(new { Velocity = _velocity.Length(), Position, Rotation, angle, diff });

		if (diff <= .3 && _velocity.Length() <= 150)
		{
			GD.Print("Landed");
			animatedSprite2D.Animation = "default";
			EmitSignal(SignalName.Landed, this);
		}
		else
		{
			GD.Print("Died");
			GetNode<AudioStreamPlayer>("Audio/Explosion").Play();
			animatedSprite2D.Animation = "explosion";
		}
	}

	private void OnInvincibleTimeout()
	{
		_invincible = false;
		GetNode<CollisionPolygon2D>("CollisionPolygon2D").Disabled = false;
	}

	private void OnAnimationFinished()
	{
		var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		if (animatedSprite2D.Animation == "explosion")
		{
			Hide();
			animatedSprite2D.Animation = "default";

			EmitSignal(SignalName.Died, this);
		}
	}
}
