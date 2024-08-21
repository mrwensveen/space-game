using Godot;

namespace SpaceGame;

public partial class Asteroid : RigidBody2D
{
	public override void _Ready()
	{
		GD.Print("Asteroid!");
	}

	public override void _PhysicsProcess(double delta)
	{
		//base._PhysicsProcess(delta);
		var collision = MoveAndCollide(Vector2.Zero, testOnly: true);
		if (collision != null)
		{
			GD.Print(collision);
		}
	}
}
