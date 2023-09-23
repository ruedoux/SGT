@tool
extends EditorPlugin


func _enter_tree() -> void:
	# Will probly load some stuff here in the future.
	print("Loaded SGT plugin.")


func _exit_tree() -> void:
	print("Unloaded SGT plugin.")
