using Godot;
using System;

public partial class InteractableObject : Node3D
{
    [Export] public string InteractPrompt { get; set; }

    [Export] public bool CanInteract { get; set; } = true;

    public virtual void Interact()
    {
        GD.Print("Override this function.");
    }
}
