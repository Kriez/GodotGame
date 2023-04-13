using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

public partial class Node2D : Godot.Node2D
{
	Vector2 boardSize = new Vector2(10, 10);
	Vector2 playerPosition = new Vector2(0, 0);
	private int squareSize = 30;

	private LineEdit _lineEdit;
	private RichTextLabel _richTextLabel;

	private List<string> _text;

	private Sprite2D _sprite;
	private Sprite2D _demonSprite;
    Vector2 demonPosition = new Vector2(3, 5);

    private List<Enemy> _enemies;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		var resource = ResourceLoader.Load("res://icon.png") as Texture2D;
		var demonResource = ResourceLoader.Load("res://Assets/demon.png") as Texture2D;

		_lineEdit = GetNode<LineEdit>("LineEdit");
		_richTextLabel = GetNode<RichTextLabel>("RichTextLabel");

		_text = new List<string>();
		_sprite = new Sprite2D();
		_sprite.Centered = false;
		_sprite.Texture = resource;		
		UpdatePosition(_sprite, playerPosition);
		var size = _sprite.Texture.GetSize();
		_sprite.Scale = new Vector2(squareSize / size.X, squareSize / size.Y);
		AddChild(_sprite);

        var enemy1 = new Enemy(demonResource, demonPosition);
        //  AddChild(enemy1);
        _enemies = new List<Enemy>();
        _enemies.Add(enemy1);
        //_demonSprite = new Sprite2D();
        //_demonSprite.Centered = false;
        //_demonSprite.Texture = demonResource;
        //UpdatePosition(_demonSprite, demonPosition);
        //var demonSize = _demonSprite.Texture.GetSize();
        //_demonSprite.Scale = new Vector2(squareSize / demonSize.X, squareSize / demonSize.Y);
        //AddChild(_demonSprite);

        //_npcs.Add(new Character(new Vector2(5, 4), demonResource, squareSize));
        foreach (Enemy enemy in _enemies)
        {
            AddChild(enemy);
        }
    }

    class Character
    {
        public Character(Vector2 position, Texture2D texture, int squareSize)
        {
            var size = texture.GetSize();

            Sprite = new Sprite2D();
            Sprite.Centered = false;
            Sprite.Texture = texture;
            Sprite.Scale = new Vector2(squareSize / size.X, squareSize / size.Y);

            Position = position;
        }

        public Sprite2D Sprite { get; set; }
        public Vector2 Position { get; set; }
    }

    public override void _Draw()
	{
		for (int x = 0; x < boardSize.X; x++)
		{
			for (int y = 0; y < boardSize.Y; y++)
			{                
				Rect2 rect = new Rect2(new Vector2((squareSize + 1) * x, (squareSize + 1) * y), new Vector2(squareSize, squareSize));
				Color color = new Color(123);
				DrawRect(rect, color);
			}
		}

	    base._Draw();
	}

	//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	//  public override void _Process(float delta)
	//  {
	//      
	//  }

	private void _on_line_edit_text_submitted(string new_text)
	{
		if (string.IsNullOrEmpty(new_text))
		{
			return;
		}
		_lineEdit.Text = string.Empty;
		_text.Add(new_text);
		_richTextLabel.Text += "\n";
		_richTextLabel.Text += $"[color={Colors.Gray.ToHtml()}]>> {new_text}[/color]";

		switch (new_text)
		{
            case "a":
                if (
                    (demonPosition.Y - 1 == playerPosition.Y && demonPosition.X == playerPosition.X) ||
                    (demonPosition.Y + 1 == playerPosition.Y && demonPosition.X == playerPosition.X) ||
                    (demonPosition.Y == playerPosition.Y && demonPosition.X + 1== playerPosition.X) ||
                    (demonPosition.Y == playerPosition.Y && demonPosition.X - 1== playerPosition.X)
                    )
                {
                    Random rnd = new Random();   
                    _richTextLabel.Text += $"\n{DamageMessage("Player", "Demon", rnd.Next(0, 150))} ";
                    return;
                }
                _richTextLabel.Text += "\n";
                _richTextLabel.Text += $"[color={Colors.White.ToHtml()}]cant find anyone[/color]";
                break;
			case "n":
				MoveCommand(_sprite, playerPosition, MovementType.North);
                UpdatePosition(_sprite, playerPosition);
                break;
			case "e":
                MoveCommand(_sprite, playerPosition, MovementType.East);
                UpdatePosition(_sprite, playerPosition);
                break;
			case "s":
                MoveCommand(_sprite, playerPosition, MovementType.South);
                UpdatePosition(_sprite, playerPosition);
                break;
			case "w":
                MoveCommand(_sprite, playerPosition, MovementType.West);
				UpdatePosition(_sprite, playerPosition);
                break;
        }
    }
	private void UpdatePosition(Sprite2D sprite, Vector2 vector2)
	{
        sprite.Position = new Vector2(vector2.X * (squareSize + 1), vector2.Y * (squareSize + 1));
    }

    string DamageMessage(string attacker, string target, int dam)
    {
        var blue = $"[color={Colors.DarkBlue.ToHtml()}]";
        var green = $"[color={Colors.DarkGreen.ToHtml()}]";
        var yellow = $"[color={Colors.Yellow.ToHtml()}]";

        string vp = string.Empty;
        var damTypeTable = new string[]{
        "punch",
    "slice",  "stab",  "slash", "whip", "claw",
    "blast",  "pound", "crush", "grep", "bite",
    "pierce", "blow"
    };
        Random rnd = new Random();
        string vtype = damTypeTable[rnd.Next(0, damTypeTable.Length)];

        if (dam == 0) { vp = "and miss"; }
        else if (dam <= 5) { vp = $"{blue}barely[/color]"; }
        else if (dam <= 20) { vp = $"{blue}slightly[/color]"; }
        else if (dam <= 35) { vp = $"{blue}moderately[/color]"; }
        else if (dam <= 50) { vp = $"{blue}fairly well[/color]"; }
        else if (dam <= 75) { vp = $"{green}somewhat hard[/color]"; }
        else if (dam <= 100) { vp = $"{green}hard[/color]"; }
        else if (dam <= 125) { vp = $"{green}very hard[/color]"; }
        else if (dam <= 150) { vp = $"{green}amazingly hard[/color]"; }
        else if (dam <= 200) { vp = $"{green}insanely hard[/color]"; }
        else if (dam <= 250) { vp = "`Yskillfully[/color]"; }
        else if (dam <= 350) { vp = "`Yvery skillfully[/color]"; }
        else if (dam <= 500) { vp = "`Yamazingly skillfully[/color]"; }
        else if (dam <= 750) { vp = "`rpowerfully[/color]"; }
        else if (dam <= 1000) { vp = "`rvery powerfully[/color]"; }
        else if (dam <= 1500) { vp = "`ramazingly powerfully[/color]"; }
        else if (dam <= 2000) { vp = "`Rwith AWESOME force[/color]"; }
        else if (dam <= 2500) { vp = "`Rwith GODLIKE force[/color]"; }
        else if (dam <= 3500) { vp = "`Rwith UNHOLY force[/color]"; }
        else if (dam <= 5000) { vp = "`WLIKE A BITCH[/color]!"; }
        else if (dam <= 10000) { vp = "`WMACK TRUCK HARD[/color]!"; }
        else if (dam <= 15000) { vp = "`WWITH FURIOUS ANGER[/color]!!"; }
        else if (dam <= 30000) { vp = "`WLIKE A `RRED HEADED STEPCHILD[/color]!!"; }
        else { vp = "`dWR[/color]AT`WHF[/color]UL`dLY[/color]"; }

        return $"Your {vtype} strikes {target} {vp} [{dam}]";
        // return $"{attacker}'s {vtype} strikes {target} {vp} [{dam}]";
    }

    
    private void MoveCommand(Sprite2D sprite, Vector2 position, MovementType movementType)
	{
        switch (movementType)
        {
            case MovementType.North:
				if (demonPosition.Y == playerPosition.Y - 1 &&
				    demonPosition.X == playerPosition.X )
				{
                    _richTextLabel.Text += "\nSomeone is standing there";
                    return;
                }
                if (playerPosition.Y - 1 < 0)
                {
                    _richTextLabel.Text += "\nCan't move there stupid";
                    return;
                }
                _richTextLabel.Text += "\nYou move north";
                playerPosition.Y -= 1;
                break;
            case MovementType.East:
                if (demonPosition.Y == playerPosition.Y &&
                    demonPosition.X == playerPosition.X + 1)
                {
                    _richTextLabel.Text += "\nSomeone is standing there";
                    return;
                }
                if (playerPosition.X + 1 >= boardSize.X)
                {
                    _richTextLabel.Text += "\nCan't move there stupid";
                    return;
                }
                _richTextLabel.Text += "\nYou move east";
                playerPosition.X += 1;
                break;
            case MovementType.South:
                if (demonPosition.Y == playerPosition.Y + 1 &&
                    demonPosition.X == playerPosition.X)
                {
                    _richTextLabel.Text += "\nSomeone is standing there";
                    return;
                }
                if (playerPosition.Y + 1 >= boardSize.Y)
                {
                    _richTextLabel.Text += "\nCan't move there stupid";
                    return;
                }
                _richTextLabel.Text += "\nYou move south";
                playerPosition.Y += 1;
                break;
            case MovementType.West:
                if (demonPosition.Y == playerPosition.Y &&
                    demonPosition.X == playerPosition.X - 1)
                {
                    _richTextLabel.Text += "\nSomeone is standing there";
                    return;
                }
                if (playerPosition.X - 1 < 0)
                {
                    _richTextLabel.Text += "\nCan't move there stupid";
                    return;
                }
                _richTextLabel.Text += "\nYou move west";
                playerPosition.X -= 1;
                break;
        }

    }

    enum MovementType
	{
		North,
		East,
		South,
		West
	}
}

