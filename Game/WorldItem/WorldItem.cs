using Godot;
using System;

public partial class WorldItem : InteractableObject
{
	[Export] private string _itemName;

    public override void Interact()
    {
        var item = GD.Load<Item>("res://Game/Resource/ItemData/"+_itemName+".tres");
        SignalManager.EmitOnGivePlayerItem(item,1);
        QueueFree();
    }
}
