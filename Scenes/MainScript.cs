using Godot;
using System;
using System.Collections.Generic;

public partial class MainScript : Godot.Node2D
{
    Vector2 boardSize = new Vector2(10, 10);
    Vector2 playerPosition = new Vector2(0, 0);
    public static int SquareSize = 30;

    private LineEdit _lineEdit;
    private RichTextLabel _richTextLabel;

    private List<string> _text;

    private Sprite2D _sprite;

    private List<Enemy> _enemies;
    private RichTextLabel enemyHealth;

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
        _sprite.Scale = new Vector2(SquareSize / size.X, SquareSize / size.Y);
        AddChild(_sprite);

        _enemies = new List<Enemy>
        {
            new Enemy("Demon", demonResource, new Vector2(3, 5))
        };

        enemyHealth = new RichTextLabel();
        enemyHealth.Position = new Vector2(30, boardSize.Y * SquareSize + 10);
        enemyHealth.Size = new Vector2(700, 100);
        

        AddChild(enemyHealth);
        foreach (Enemy enemy in _enemies)
        {
            enemyHealth.Text = $"Demon {HealthStatus(enemy.Health, enemy.MaxHealth)}";
            AddChild(enemy);
        }
    }

    public override void _Draw()
    {
        for (int x = 0; x < boardSize.X; x++)
        {
            for (int y = 0; y < boardSize.Y; y++)
            {
                Rect2 rect = new Rect2(new Vector2((SquareSize + 1) * x, (SquareSize + 1) * y), new Vector2(SquareSize, SquareSize));
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
                foreach (Enemy enemy in _enemies)
                {
                    if (
                        (enemy._position.Y - 1 == playerPosition.Y && enemy._position.X == playerPosition.X) ||
                        (enemy._position.Y + 1 == playerPosition.Y && enemy._position.X == playerPosition.X) ||
                        (enemy._position.Y == playerPosition.Y && enemy._position.X + 1 == playerPosition.X) ||
                        (enemy._position.Y == playerPosition.Y && enemy._position.X - 1 == playerPosition.X)
                       )
                    {
                        Random rnd = new Random();
                        int damage = rnd.Next(0, 150);
                        _richTextLabel.Text += $"\n{DamageMessage("Player", "Demon", damage)} ";
                        enemy.Health -= damage;
                        enemyHealth.Text = $"{enemy.Name} {HealthStatus(enemy.Health, enemy.MaxHealth)}";

                        return;
                    }
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
            default:
                _richTextLabel.Text += "\n";
                _richTextLabel.Text += $"Huh?";
                break;

        }
    }
    private void UpdatePosition(Sprite2D sprite, Vector2 vector2)
    {
        sprite.Position = new Vector2(vector2.X * (SquareSize + 1), vector2.Y * (SquareSize + 1));
    }

    public string HealthStatus(int health, int maxhealth)
    {
        var percent = (health * 100) / maxhealth;

        if (percent >= 100)
            return " is in excellent condition.";
        else if (percent >= 90)
            return " has a few scratches.";
        else if (percent >= 75)
            return " has some small wounds and bruises.";
        else if (percent >= 50)
            return " has quite a few wounds.";
        else if (percent >= 30)
            return " has some big nasty wounds and scratches.";
        else if (percent >= 15)
            return " looks pretty hurt.";
        else if (percent >= 0)
            return " is in awful condition.";
        else
            return " is bleeding to death.";
    }

    string DamageMessage(string attacker, string target, int dam)
    {
        var blue = $"[color={Colors.DarkBlue.ToHtml()}]";
        var green = $"[color={Colors.DarkGreen.ToHtml()}]";
        var yellow = $"[color={Colors.Yellow.ToHtml()}]";
        var red = $"[color={Colors.Red.ToHtml()}]";
        var darkRed = $"[color={Colors.DarkRed.ToHtml()}]";
        var gray = $"[color={Colors.Gray.ToHtml()}]";

        string vp = string.Empty;
        var damTypeTable = new string[]{
        "punch", "slice",  "stab",  "slash", "whip", "claw",
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
        else if (dam <= 250) { vp = $"`{green}skillfully[/color]"; }
        else if (dam <= 350) { vp = $"`{green}very skillfully[/color]"; }
        else if (dam <= 500) { vp = $"`{green}amazingly skillfully[/color]"; }
        else if (dam <= 750) { vp = $"{darkRed}powerfully[/color]"; }
        else if (dam <= 1000) { vp = $"{darkRed}very powerfully[/color]"; }
        else if (dam <= 1500) { vp = $"{darkRed}amazingly powerfully[/color]"; }
        else if (dam <= 2000) { vp = $"{red}with AWESOME force[/color]"; }
        else if (dam <= 2500) { vp = $"{red}with GODLIKE force[/color]"; }
        else if (dam <= 3500) { vp = $"{red}with UNHOLY force[/color]"; }
        else if (dam <= 5000) { vp = $"{gray}LIKE A BITCH[/color]!"; }
        else if (dam <= 10000) { vp = $"{gray}MACK TRUCK HARD[/color]!"; }
        else if (dam <= 15000) { vp = $"{gray}WITH FURIOUS ANGER[/color]!!"; }
        else if (dam <= 30000) { vp = $"{gray}LIKE A {red}RED HEADED STEPCHILD[/color]!!"; }
        else { vp = "`dWR[/color]AT`WHF[/color]UL`dLY[/color]"; }

        return $"Your {vtype} strikes {target} {vp} [{dam}]";
        // return $"{attacker}'s {vtype} strikes {target} {vp} [{dam}]";
    }


    private void MoveCommand(Sprite2D sprite, Vector2 position, MovementType movementType)
    {
        switch (movementType)
        {
            case MovementType.North:
                foreach (var enemy in _enemies)
                {
                    if (enemy._position.Y == playerPosition.Y - 1 &&
                        enemy._position.X == playerPosition.X)
                    {
                        _richTextLabel.Text += "\nSomeone is standing there";
                        return;
                    }
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
                foreach (var enemy in _enemies)
                {
                    if (enemy._position.Y == playerPosition.Y &&
                        enemy._position.X == playerPosition.X + 1)
                    {
                        _richTextLabel.Text += "\nSomeone is standing there";
                        return;
                    }
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
                foreach (var enemy in _enemies)
                {
                    if (enemy._position.Y == playerPosition.Y + 1 &&
                        enemy._position.X == playerPosition.X)
                    {
                        _richTextLabel.Text += "\nSomeone is standing there";
                        return;
                    }
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
                foreach (var enemy in _enemies)
                {
                    if (enemy._position.Y == playerPosition.Y &&
                        enemy._position.X == playerPosition.X - 1)
                    {
                        _richTextLabel.Text += "\nSomeone is standing there";
                        return;
                    }
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

