using Godot;
using System;

public partial class HighlightChild : PointLight2D
{
	Node parent;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		parent = GetParent();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Energy = (float)parent.GetMeta("Energy");
		Position = (Vector2)parent.GetMeta("Position");
	}
}
