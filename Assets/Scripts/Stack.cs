using System;
using UnityEngine;

public class Stack
{
    public ItemDef ItemStored { get; private set; }

    private int _quantityStored;

    public int QuantityStored
    {
        get => _quantityStored;
        set
        {
            if (_quantityStored == value) return; // Optimization: don't notify if value hasn't changed
            _quantityStored = value;
            OnQuantityChanged?.Invoke(_quantityStored);
        }
    }

    public event Action<int> OnQuantityChanged;

    public Stack(ItemDef item, int quantity = 1)
    {
        ItemStored = item;
        QuantityStored = quantity;
    }
}
