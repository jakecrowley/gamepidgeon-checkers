using Godot;
using System;
using System.Collections.Generic;

public partial class CheckersBoardTop : Sprite2D
{
	string replay = "board:0,2,0,2,0,2,0,2,2,0,2,0,2,0,2,0,0,2,0,2,0,2,0,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,1,0,1,0,1,0,0,1,0,1,0,1,0,1,1,0,1,0,1,0,1,0|move:5,2,4,3|board:0,2,0,2,0,2,0,2,2,0,2,0,2,0,2,0,0,2,0,2,0,0,0,2,0,0,0,0,2,0,0,0,0,0,0,0,0,0,0,0,1,0,1,0,1,0,1,0,0,1,0,1,0,1,0,1,1,0,1,0,1,0,1,0";
	BoardHighlight boardHighlight;

	List<BoardHighlight> highlights = new List<BoardHighlight>();
	Sprite2D clickedPiece;
	Vector2 clickedPiecePos;
	List<Vector2> moves = new List<Vector2>();
	Vector2[] prevMove = new Vector2[2];

	Sprite2D redPiece;

	bool hasMoved = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		redPiece = this.GetNode<Sprite2D>("CheckerPieceRed");
		Sprite2D blackPiece = this.GetNode<Sprite2D>("CheckerPieceBlack");
		boardHighlight = GetNode<BoardHighlight>("BoardHighlight");

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
					AddChild(newPiece);
					newPiece.Position = new Vector2(redPiece.Position.X + (135 * x), redPiece.Position.Y + (135 * y));
					newPiece.Visible = true;
					newPiece.Name = x + "," + (7 - y);
					GD.Print(newPiece.Name, " : ", newPiece.Position);
					if (newPiece.Name.Equals(move[0] + "," + move[1]))
					{
						Vector2 newPos = new Vector2(redPiece.Position.X + (135 * int.Parse(move[2])), redPiece.Position.Y + (135 * (int.Parse(move[3])+1)));
						Tween tween = newPiece.GetTree().CreateTween();
						tween.TweenProperty(newPiece, "position", newPos, 1.0f).SetTrans(Tween.TransitionType.Sine);
						newPiece.Name = move[2] + "," + move[3];
					}
				}
			}
		}

		// Sprite2D newPiece = (Sprite2D)redPiece.Duplicate();
		// newPiece.Visible = true;
	}

	public void clearHighlights() {
		foreach (var highlight in highlights) {
			highlight.Free();
		}
		highlights.Clear();
	}

	public void genMoves() {
		moves.Clear();
		List<Vector2> diagonals = new List<Vector2>();
		var name = clickedPiece.Texture.ResourcePath;
		if(name.Contains("black")){
			diagonals.Add(new Vector2(-1f, 1f));
			diagonals.Add(new Vector2(1f, 1f));
		} else if(name.Contains("red")){
			diagonals.Add(new Vector2(1f, -1f));
			diagonals.Add(new Vector2(-1f, -1f));
		}

		GD.Print(clickedPiece.Texture.ResourcePath);
		foreach (var diagonal in diagonals) {
			var pos = new Vector2(clickedPiecePos.X + diagonal.X, clickedPiecePos.Y + diagonal.Y);
			if (GetNode(pos.X + "," + pos.Y) == null) {
				moves.Add(pos);
				GD.Print(pos);
				addHighlight((int)pos.X, 7 - (int)pos.Y);
			}
		}
	}

	public void addHighlight(int x, int y) {
		BoardHighlight newHighlight = (BoardHighlight)boardHighlight.Duplicate();
		highlights.Add(newHighlight);
		AddChild(newHighlight);
		Vector2 basePos = (Vector2)boardHighlight.GetMeta("Position"); 
		Vector2 newPos = new Vector2(basePos.X + (81f * x), basePos.Y + (81f * y));
		newHighlight.SetMeta("Position", newPos);
		newHighlight.SetMeta("Visible", true);
		GD.Print("Highlighting: ", x, ",", y);
	}

	public void movePiece(Sprite2D piece, int x, int y) {
		Vector2 newPos = new Vector2(redPiece.Position.X + (135 * x), redPiece.Position.Y + (135 * y));
		Tween tween = piece.GetTree().CreateTween();
		tween.TweenProperty(piece, "position", newPos, 0.5f).SetTrans(Tween.TransitionType.Sine);
		piece.Name = x + "," + (7-y);
	}

	public void undoMove() {
		Sprite2D piece = GetNode<Sprite2D>(prevMove[1].X + "," + prevMove[1].Y);
		movePiece(piece, (int)prevMove[0].X, Math.Abs((int)prevMove[0].Y - 7));
		hasMoved = false;
		GetParent().GetNode<Button>("UndoButton").Disabled = true;
	}

	public override void _Input(InputEvent @event) {
		// Mouse in viewport coordinates.
		if (@event is InputEventMouseButton eventMouseButton) {
			if(eventMouseButton.IsPressed() && !hasMoved) {
				int x = (int)Math.Ceiling(eventMouseButton.Position.X / 80) - 1;
				int y = (int)Math.Ceiling(eventMouseButton.Position.Y / 80) - 3 - 1;
				GD.Print("board position at ", x, ",", 7-y, " clicked");

				var clickedPiece = GetNode<Sprite2D>(x + "," + (7-y));
				if (clickedPiece != null) {
					clearHighlights();
					this.clickedPiece = clickedPiece;
					clickedPiecePos = new Vector2(x, 7-y);
					addHighlight(x, y);
					genMoves();
				} else if(this.clickedPiece != null && moves.Contains(new Vector2(x, 7-y))){
					movePiece(this.clickedPiece, x, y);
					this.clickedPiece = null;
					clearHighlights();
					hasMoved = true;
					prevMove[0] = clickedPiecePos;
					prevMove[1] = new Vector2(x, 7-y);
					GetParent().GetNode<Button>("UndoButton").Disabled = false;
				}
			}
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta){}
}
