using Godot;
using System;

public partial class BoardHighlight : Node
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Tween tween = GetTree().CreateTween();
		tween.TweenMethod(Callable.From<float>(SetEnergy), 5.0f, 20.0f, 0.5f).SetTrans(Tween.TransitionType.Sine);
		tween.TweenMethod(Callable.From<float>(SetEnergy), 20.0f, 5.0f, 0.5f).SetTrans(Tween.TransitionType.Sine);
		tween.SetLoops();
	}

	private void SetEnergy(float value) { 
		SetMeta("Energy", value);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta){}
}
