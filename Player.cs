using Godot;
using System;
using System.Collections.Generic;

public partial class Player : CharacterBody2D
{
	private int _speed = 400;
	private float _angularSpeed = Mathf.Pi;

	public Player()
	{
		GD.Print("Hello World!");
	}

	public override void _Process(double delta)
	{
		//Rotation += _angularSpeed * (float)delta;

		//var velocity = Vector2.Up.Rotated(Rotation) * _speed;
		//Position += velocity * (float)delta;

		var actions = new List<(string, Vector2)> {
			("p1_left", Vector2.Left),
			("p1_right", Vector2.Right),
			("p1_up", Vector2.Up),
			("p1_down", Vector2.Down),
		};

		var direction = Vector2.Zero;
		foreach (var (key, vector) in actions)
		{
			if (Input.IsActionPressed(key))
			{
				direction += vector;
			}
		}

		Position += _speed * direction.Normalized() * (float)delta;
	}
}
