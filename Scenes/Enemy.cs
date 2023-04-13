using Godot;
using System;

public partial class Enemy : Node2D
{
	private Sprite2D _sprite { get; set; }
	private Vector2 _position { get; set; }
    private int squareSize = 30;

	public Enemy(Texture2D texture, Vector2 position)
	{		
		_sprite = new Sprite2D();		
		_sprite.Texture = texture;
		_position = position;
		
	}
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{   
		_sprite.Centered = false;  
		UpdatePosition(_sprite, _position);
		var size = _sprite.Texture.GetSize();
		_sprite.Scale = new Vector2(squareSize / size.X, squareSize / size.Y);		
		AddChild(_sprite);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void UpdatePosition(Sprite2D sprite, Vector2 vector2)
	{
		_sprite.Position = new Vector2(vector2.X * (squareSize + 1), vector2.Y * (squareSize + 1));
	}
}
