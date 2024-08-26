using Godot;

namespace SpaceGame.Scenes.Space;

public partial class Main : Node2D
{
	[Export] public required PackedScene PlayerScene { get; set; }
	[Export] public required PackedScene AsteroidScene { get; set; }

	public override void _Ready()
	{
		var size = GetViewportRect().Size * Scale.Inverse();
		GetNode<TextureRect>("Background").Size = size;
	}

	public override void _Input(InputEvent inputEvent)
	{
		if (inputEvent is InputEventKey eventKey
			&& eventKey.Keycode == Key.Escape)
		{
			GetTree().Quit();
		}

		if (inputEvent.IsAction("p1_fire") && !HasNode("Player1"))
		{
			var player = PlayerScene.Instantiate<Player>();
			player.Name = "Player1";
			player.Died += PlayerDied;
			player.Start(new(5000, 3000));

			AddChild(player);
		}
	}

	private void PlayerDied(Player player)
	{
		GD.Print("PlayerDied!");
		player.Start(new(5000, 3000));
	}
}
