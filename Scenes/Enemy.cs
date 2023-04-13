using Godot;
using System;

public partial class Enemy : Godot.Node2D
{
	public Sprite2D Sprite { get; set; }
	public Vector2 _position { get; set; }
    private int squareSize = 30;

	public new string Name { get; set; }
	public int Health = 1000;
	public int MaxHealth = 1000;

	public Enemy(string name, Texture2D texture, Vector2 position)
	{
		Name = name;
        Sprite = new Sprite2D();
        Sprite.Texture = texture;
		_position = position;
		 
	}
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        Sprite.Centered = false;  
		UpdatePosition(_position);
		var size = Sprite.Texture.GetSize();
        Sprite.Scale = new Vector2(MainScript.SquareSize / size.X, MainScript.SquareSize / size.Y);		
		AddChild(Sprite);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void UpdatePosition(Vector2 vector2)
	{
        Sprite.Position = new Vector2(vector2.X * (squareSize + 1), vector2.Y * (squareSize + 1));
	}
}
