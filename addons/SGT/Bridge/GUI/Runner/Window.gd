extends VBoxContainer

@onready var MoveHook = get_node("MoveHook")

var isDraggerd := false
var dragStartPos: Vector2
var windowDragStartPos: Vector2


func _input(_event: InputEvent) -> void:
	move_window()


func move_window() -> void:
	if Input.is_mouse_button_pressed(MOUSE_BUTTON_LEFT):
		if !isDraggerd and is_mouse_on_element(MoveHook):
			dragStartPos = get_global_mouse_position()
			windowDragStartPos = position
			isDraggerd = true
	else:
		isDraggerd = false

	if isDraggerd:
		position = windowDragStartPos + (get_global_mouse_position() - dragStartPos)


static func is_mouse_on_element(element: Control) -> bool:
	return element.get_global_rect().has_point(element.get_global_mouse_position())
