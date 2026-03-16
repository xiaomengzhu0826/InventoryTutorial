using Godot;
using System;

public partial class SignalManager : Node
{
    public static SignalManager Instance {get;private set;}
    
    public override void _Ready()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            QueueFree();
        }
    }

    [Signal] public delegate void OnGivePlayerItemEventHandler(Item item,int amount);
    
    public static void EmitOnGivePlayerItem(Item item,int amount)
    {
        Instance.EmitSignal(SignalName.OnGivePlayerItem,item, amount);
    }
}
