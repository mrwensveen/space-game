using Godot;

namespace SpaceGame.Scenes.Space;

public partial class Main : Node2D
{
	[Export] public required PackedScene PlayerScene { get; set; }
	[Export] public required PackedScene NextScene { get; set; }

	private AudioStreamPlayer? _ambience;
	private Node2D? _asteroid;
	private Player? _player;

	public override void _Ready()
	{
		var size = GetViewportRect().Size * Scale.Inverse();
		GetNode<TextureRect>("Background").Size = size;

		_asteroid = GetNode<Node2D>("Asteroid");
		_ambience = GetNode<AudioStreamPlayer>("Audio/Ambience");
		_ambience.Play();

		_player = GetNode<Player>("Player");
	}

	public override void _Process(double delta)
	{
		if (_player!.Visible && _ambience!.Playing)
		{
			var distance = _player.Position.DistanceTo(_asteroid!.Position);
			_ambience.VolumeDb = -(distance / 200);
		}
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
		_player!.Start(new(5000, 4500), 0, 2000);
	}

	private void OnPlayerDied(Player player)
	{
		GD.Print("Player died!");
		GetNode<Timer>("StartTimer").Start(1);
	}

	private void OnPlayerLanded(Player player)
	{
		GD.Print("Player landed!");
		GetTree().CallDeferred("change_scene_to_packed", NextScene);
	}
}
