using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class ButtonClick : Button
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready(){}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta){}

    public override void _Pressed()
    {
		if (Name.Equals("SendButton"))
		{

		}
		else if(Name.Equals("UndoButton"))
		{
			GD.Print("Undo clicked");
			var board = GetNode<CheckersBoardTop>("../CheckersBoardTop");
			board.undoMove();
		}

        base._Pressed();
    }
}
