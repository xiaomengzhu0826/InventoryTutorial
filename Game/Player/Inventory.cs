using Godot;
using System;
using System.Collections.Generic;

public partial class Inventory : Node
{
    private List<InventorySlot> _slot = new();
    private Panel _window;
    public Label InfoText {get;set;}

    [Export] private Godot.Collections.Array<Item> _starterItems;

    public override void _EnterTree()
    {
        _window = GetNode<Panel>("InventoryWindow");
        InfoText = GetNode<Label>("InventoryWindow/InfoText");
    }

    public override void _Ready()
    {
        ToggleWindow(false);
        foreach (InventorySlot child in _window.GetNode<GridContainer>("SlotContainer").GetChildren())
        {
            if (child is InventorySlot slot)
            {
                _slot.Add(slot);
                slot.SetItem(null);
                slot.Inventory = this;
            }
        }
        if (_starterItems != null)
        {
            foreach (Item item in _starterItems)
            {
                AddItem(item);
            }
        }

        SignalManager.Instance.OnGivePlayerItem += OnGivePlayerItem;
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("inventory"))
        {
            ToggleWindow(!_window.Visible);
        }
    }

    public override void _ExitTree()
    {
        SignalManager.Instance.OnGivePlayerItem -= OnGivePlayerItem;
    }

    public void ToggleWindow(bool open)
    {
        _window.Visible = open;
        if (open)
        {
            Input.MouseMode = Input.MouseModeEnum.Visible;
        }
        else
        {
            Input.MouseMode = Input.MouseModeEnum.Captured;
        }
    }

    public void OnGivePlayerItem(Item item, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            AddItem(item);
        }
    }

    public void AddItem(Item item)
    {
        var slot = GetSlotToAdd(item);
        if(slot==null)
        {
            return;
        }
        if(slot.Item==null)
        {
            slot.SetItem(item);
        }
        else if(slot.Item==item)
        {
            slot.AddItem();
        }
    }

    public void RemoveItem(Item item)
    {
        var slot=GetSlotToRemove(item);
        if(slot == null || slot.Item==null)
        {
            return;
        }

        slot.RemoveItem();
    }

    public InventorySlot GetSlotToAdd(Item item)
    {
        foreach (InventorySlot slot in _slot)
        {
            if(slot.Item == item && slot.Quantity<item.MaxStackSize)
            {
                return slot;
            }
        }

        foreach (InventorySlot slot in _slot)
        {
            if(slot.Item == null)
            {
                return slot;
            }
        }

        return null;

    }

    public InventorySlot GetSlotToRemove(Item item)
    {
        foreach (InventorySlot slot in _slot)
        {
            if(slot.Item == item)
            {
                return slot;
            }
        }
        return null;
    }

    public int GetNumberOfItem(Item item)
    {
        var total = 0;
        foreach (InventorySlot slot in _slot)
        {
            if(slot.Item == item)
            {
                total+= slot.Quantity;
            }
        }
        return total;
    }
}
