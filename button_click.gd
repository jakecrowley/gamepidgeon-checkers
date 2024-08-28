extends Button


# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	pass


func _pressed() -> void:
	if name == "SendButton":
		print("Send clicked")
	elif name == "UndoButton":
		print("Undo clicked")
		var board: CheckersBoardTop = get_node("../CheckersBoardTop")
		board.undo_move()
