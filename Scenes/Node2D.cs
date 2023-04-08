using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

public partial class Node2D : Godot.Node2D
{
	private int width = 10;
	private int height = 10;

	private int squareSize = 30;

	private LineEdit _lineEdit;
	private RichTextLabel _richTextLabel;

	private List<string> _text;

	private Sprite2D _sprite;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var resource = ResourceLoader.Load("res://icon.png") as Texture2D;

		_lineEdit = GetNode<LineEdit>("LineEdit");
		_richTextLabel = GetNode<RichTextLabel>("RichTextLabel");
		var Sprite2D = GetNode<Sprite2D>("Sprite2D");
		_text = new List<string>();
		_sprite = new Sprite2D();
		_sprite.Centered = false;
		_sprite.Texture = resource;		
		_sprite.Position = new Vector2(0, 0);

		var size = _sprite.Texture.GetSize();
        _sprite.Scale = new Vector2((size.X / (size.X / squareSize)) / 50, (size.Y / (size.Y / squareSize)) / 50);

        AddChild(_sprite);
		
    }

	public override void _Draw()
	{
		for (int i = 0; i < width; i++)
		{
			for (int j = 0; j < height; j++)
			{                
				Rect2 rect = new Rect2(new Vector2((squareSize +1)* i, (squareSize + 1)* j), new Vector2(squareSize, squareSize));
				GD.Print($"{rect.Position.X} {rect.Position.Y}");
				Color color = new Color(123);
				DrawRect(rect, color);
			}
		}

	   // base._Draw();
	}

	//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	//  public override void _Process(float delta)
	//  {
	//      
	//  }

	private void _on_line_edit_text_submitted(string new_text)
	{
		GD.Print($">> [color={Colors.Red.ToHtml()}]{new_text}[/color]");
		_lineEdit.Text = string.Empty;
		_text.Add(new_text);
        _richTextLabel.Text += "\n";
        _richTextLabel.Text += $"[color={Colors.Gray.ToHtml()}]>> {new_text}[/color]";

		switch (new_text)
		{
			case "n":
                _richTextLabel.Text += "\nYou move north";
                _sprite.Position = new Vector2(_sprite.Position.X + 33, _sprite.Position.Y);
				break;
		}
	}

}

