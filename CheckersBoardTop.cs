using Godot;
using System;
using System.Collections.Generic;

public partial class CheckersBoardTop : Sprite2D
{
	string replay = "board:0,2,0,2,0,2,0,2,2,0,2,0,2,0,2,0,0,2,0,2,0,2,0,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,1,0,1,0,1,0,0,1,0,1,0,1,0,1,1,0,1,0,1,0,1,0|move:5,2,4,3|board:0,2,0,2,0,2,0,2,2,0,2,0,2,0,2,0,0,2,0,2,0,0,0,2,0,0,0,0,2,0,0,0,0,0,0,0,0,0,0,0,1,0,1,0,1,0,1,0,0,1,0,1,0,1,0,1,1,0,1,0,1,0,1,0";
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Sprite2D redPiece = this.GetNode<Sprite2D>("CheckerPieceRed");
		Sprite2D blackPiece = this.GetNode<Sprite2D>("CheckerPieceBlack");

		var prevBoard = replay.Split('|')[2].Substring(6).Split(",");

		for (int y = 0; y < 8; y++)
		{
			for (int x = 0; x < 8; x++)
			{
				string val = prevBoard[y * 8 + (7 - x)];
				Sprite2D newPiece = null;
				if (val.Equals("1"))
				{
					newPiece = (Sprite2D)blackPiece.Duplicate();
				}
				else if (val.Equals("2"))
				{
					newPiece = (Sprite2D)redPiece.Duplicate();
				}

				if (newPiece != null)
				{
					this.AddChild(newPiece);
					newPiece.Position = new Vector2(redPiece.Position.X + (135 * x), redPiece.Position.Y + (135 * y));
					newPiece.Visible = true;
					newPiece.Name = (x+1) + "," + (y+1);
					GD.Print(x + 1, ", ", y + 1, " : ", newPiece.Position);
				}
			}
		}

		// Sprite2D newPiece = (Sprite2D)redPiece.Duplicate();
		// newPiece.Visible = true;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
