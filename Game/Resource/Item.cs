using Godot;
using System;

public partial class Item : Resource
{
    [Export] public string DisplayName {get;set;}
    [Export] public Texture2D Icon {get;set;}
    [Export] public int MaxStackSize {get;set;}
    [Export] public PackedScene WorldItemScene {get;set;}

    public bool OnUse(Player player)
    {
        GD.Print("Use");
        return false;
    }
}
