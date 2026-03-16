using Godot;
using System;

public partial class InteractableController : RayCast3D
{
        private Label _interactPromptLabel;

    public override void _Ready()
    {
        _interactPromptLabel = GetNode<Label>("InteractionPrompt");
    }

    public override void _Process(double delta)
    {
        var obj = GetCollider();
        _interactPromptLabel.Text = "";

        if (obj != null && obj is InteractableObject interactable)
        {
            if (!interactable.CanInteract)
                return;

            _interactPromptLabel.Text = "[E] " + interactable.InteractPrompt;

            if (Input.IsActionJustPressed("interact"))
            {
                interactable.Interact();
            }
        }
    }
}
