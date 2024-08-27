using Godot;
using System;
using System.Collections.Generic;

public partial class CheckersBoardTop : Sprite2D
{
	string replay = "board:0,2,0,2,0,2,0,2,2,0,2,0,2,0,2,0,0,2,0,2,0,2,0,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,1,0,1,0,1,0,0,1,0,1,0,1,0,1,1,0,1,0,1,0,1,0|move:5,2,4,3|board:0,2,0,2,0,2,0,2,2,0,2,0,2,0,2,0,0,2,0,2,0,0,0,2,0,0,0,0,2,0,0,0,0,0,0,0,0,0,0,0,1,0,1,0,1,0,1,0,0,1,0,1,0,1,0,1,1,0,1,0,1,0,1,0";
	Sprite2D movePiece;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Sprite2D redPiece = this.GetNode<Sprite2D>("CheckerPieceRed");
		Sprite2D blackPiece = this.GetNode<Sprite2D>("CheckerPieceBlack");

		var prevBoard = replay.Split('|')[0].Substring(6).Split(",");
		var move = replay.Split("|")[1].Substring(5).Split(",");
		var nextBoard = replay.Split('|')[2].Substring(6).Split(",");

		for (int y = 0; y < 8; y++)
		{
			for (int x = 0; x < 8; x++)
			{
				string val = prevBoard[(7 - y) * 8 + x];
				Sprite2D newPiece = null;
				if (val.Equals("2"))
				{
					newPiece = (Sprite2D)blackPiece.Duplicate();
				}
				else if (val.Equals("1"))
				{
					newPiece = (Sprite2D)redPiece.Duplicate();
				}

				if (newPiece != null)
				{
					this.AddChild(newPiece);
					newPiece.Position = new Vector2(redPiece.Position.X + (135 * x), redPiece.Position.Y + (135 * y));
					newPiece.Visible = true;
					newPiece.Name = x + "," + (7 - y);
					GD.Print(newPiece.Name, " : ", newPiece.Position);
					if (newPiece.Name.Equals(move[0] + "," + move[1]))
					{
						Vector2 newPos = new Vector2(redPiece.Position.X + (135 * int.Parse(move[2])), redPiece.Position.Y + (135 * (int.Parse(move[3])+1)));
						Tween tween = newPiece.GetTree().CreateTween();
						tween.TweenProperty(newPiece, "position", newPos, 1.0f).SetTrans(Tween.TransitionType.Sine);
						movePiece = newPiece;
					}
				}
			}
		}

		// Sprite2D newPiece = (Sprite2D)redPiece.Duplicate();
		// newPiece.Visible = true;
	}

	public override void _Input(InputEvent @event) {
		// Mouse in viewport coordinates.
		if (@event is InputEventMouseButton eventMouseButton) {
			if(eventMouseButton.IsPressed()) {
				int x = 7 - (int)Math.Ceiling(eventMouseButton.Position.X / 80);
				int y = 7 - ((int)Math.Ceiling(eventMouseButton.Position.Y / 80) - 3);
				GD.Print("board position at ", x, ",", y, " clicked");
			}
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta){}
}
