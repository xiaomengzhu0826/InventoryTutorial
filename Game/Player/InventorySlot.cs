using Godot;
using System;

public partial class InventorySlot : Button
{
	public Item Item { get; set; }
	public int Quantity;
	private TextureRect _icon;
	private Label _quantityText;
	public Inventory Inventory { get; set; }

	public override void _EnterTree()
	{
		_icon = GetNode<TextureRect>("Icon");
		_quantityText = GetNode<Label>("QuantityText");
	}

	public void SetItem(Item newItem)
	{
		Item = newItem;
		Quantity = 1;
		if (Item == null)
		{
			_icon.Visible = false;
		}
		else
		{
			_icon.Visible = true;
			_icon.Texture = Item.Icon;
		}

		UpdateQuantityText();
	}

	public void AddItem()
	{
		Quantity += 1;
		UpdateQuantityText();
	}

	public void RemoveItem()
	{
		Quantity -= 1;
		UpdateQuantityText();

		if (Quantity == 0)
		{
			SetItem(null);
		}
	}

	public void UpdateQuantityText()
	{
		if (Quantity <= 1)
		{
			_quantityText.Text = "";
		}
		else
		{
			_quantityText.Text = Quantity.ToString();
		}
	}

	private void OnMouseEntered()
	{
		if (Item == null)
		{
			Inventory.InfoText.Text = "";
		}
		else
		{
			Inventory.InfoText.Text = Item.DisplayName;
		}
	}

	private void OnMouseExited()
	{
		Inventory.InfoText.Text = "";
	}

	private void OnPressed()
	{
		if (Item == null)
		{
			return;
		}
		var removeAfterUse = Item.OnUse(Inventory.GetParent() as Player);
		if (removeAfterUse)
		{
			RemoveItem();
		}
	}

	public void DropItem()
	{
		if (Item == null) return;

		Node3D worldItem = Item.WorldItemScene.Instantiate<Node3D>();
		AddChild(worldItem);

		Node3D parent = Inventory.GetParent<Node3D>();

		worldItem.Position =
			parent.Position +
			new Vector3(0, 1.5f, 0) -
			parent.Basis.Z;

		GD.Print(worldItem);

		RemoveItem();
	}

	private void OnGuiInput(InputEvent @event)
	{
		if (@event is InputEventMouseButton input && @event.IsPressed())
		{
			if (input.ButtonIndex == MouseButton.Right)
			{
				DropItem();
			}
		}
	}

}
