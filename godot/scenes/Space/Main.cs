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
	}

	private void OnStartTimerTimeout()
	{
		var player = PlayerScene.Instantiate<Player>();
		player.Name = "Player1";
		player.Died += OnPlayerDied;
		player.Landed += OnPlayerLanded;
		player.Start(new(5000, 4500), 0, 2000);

		AddChild(player);
	}

	private void OnPlayerDied(Player player)
	{
		GD.Print("Player died!");
		GetNode<Timer>("StartTimer").Start(1);
	}

	private void OnPlayerLanded(Player player)
	{
		GD.Print("Player landed!");
		GetTree().ChangeSceneToFile("res://scenes/Surface/Surface.tscn");
	}
}
