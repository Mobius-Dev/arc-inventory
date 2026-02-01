using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class SlotManager : MonoBehaviour
{
    // A singleton which holds knowledge of and runs operations on all inventory slots
    public static SlotManager Instance { get; private set; }

    [SerializeField] private List<Slot> _allSlots = new();
    
    public void RegisterSlot(Slot slot)
    {
        //Called by an empty slot at awake to register itself

        if (!_allSlots.Contains(slot)) _allSlots.Add(slot);
        else Debug.LogWarning($"{slot.gameObject.name} tried to register multiple times!");
    }

    public void PlaceTile(Tile tile)
    {
        // Find the best slot for the given content and place it there

        Slot neareastSlot = _allSlots
            .OrderBy(item => (item.transform.position - tile.transform.position).sqrMagnitude)
            .ToList()[0]; // Find the slot nearest to where the dragged content tile is

        Slot selectedSlot = null;

        if (ValidateSlot(neareastSlot, tile)) selectedSlot = neareastSlot; // Validate this slot before using it
        else selectedSlot = tile.LastOccupiedSlot; // Fallback to last occupied slot if no valid slot found

        selectedSlot.OccupySlot(tile);
        tile.LastOccupiedSlot = selectedSlot;
        tile.transform.SetParent(selectedSlot.transform, false); // Move the content tile to be a child of the slot
        tile.transform.localPosition = Vector3.zero; // Center the content tile in the slot
    }

    private void Awake()
    {
        // Singleton enforcement

        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    private bool ValidateSlot(Slot slot, Tile contentToValidate)
    {
        Tile contentInSlot = slot.Content;

        if (contentInSlot)
        {
            // Slot is occupied, check if stacking is possible

            if (contentInSlot.ItemStored.ItemID == contentToValidate.ItemStored.ItemID
                && contentInSlot.QuantityStored + contentToValidate.QuantityStored < contentInSlot.ItemStored.MaxStackSize)
            {
                // Same item and merge won't exceed max stack size, valid for stacking
                return true;
            }

            else return false;
        }

        // TODO implement checks for special slot types (e.g. equipment slots)

        else return true; // Slot is empty, valid
    }
}