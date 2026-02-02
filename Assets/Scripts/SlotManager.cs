using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class SlotManager : MonoBehaviour
{
    // A singleton which holds knowledge of and runs operations on all inventory slots
    public static SlotManager Instance { get; private set; }

    private List<Slot> _allSlots = new();
    
    public void RegisterSlot(Slot slot)
    {
        //Called by an instance if Slot when it is created to register itself with the manager

        if (!_allSlots.Contains(slot)) _allSlots.Add(slot);
        else Debug.LogWarning($"{slot.gameObject.name} tried to register multiple times!");
    }

    public void PlaceTileFromDrag(Tile tileToPlace, Slot fallbackSlot)
    {
        Slot closestSlot = GetClosestSlot(tileToPlace);
        Slot selectedSlot = null;

        if (!TryPlaceTileAt(closestSlot, tileToPlace)) // Try place tile at slot closest to the dragged element
        {
            if (closestSlot != fallbackSlot && TryPlaceTileAt(fallbackSlot, tileToPlace)) // On failure, try instead the slot the tile came from (unless it's the same slot)
            {
                selectedSlot = fallbackSlot;
            }
            else
            {
                Debug.LogError($"{tileToPlace.name} has no valid slot to occupy");
                return;
            }
        }
        else
        {
            selectedSlot = closestSlot;
        }

        if (tileToPlace.StackStored.QuantityStored > 0) // Checks if a merge occured, if it did dragged tile was destroyed (workaround because Destroy(gameobject) doesn't work on the same frame)
        {
            selectedSlot.TileStored = tileToPlace; // Update the slot to store the placed tile
            tileToPlace.RefreshTile();
        }
    }

    private bool TryPlaceTileAt(Slot targetSlot, Tile targetTile)
    {
        if (!targetSlot.TileStored)
        {
            // 1. Slot is empty, place the tile here
            return true;
        }
        else if (StackManager.Instance.AttemptMerge(targetSlot.TileStored.StackStored, targetTile.StackStored))
        {
            // 2. Slot is not empty, but tried to merge stacks and succeeded, remove the dragged tile and exit
            targetSlot.TileStored.RefreshTile();
            Destroy(targetTile.gameObject);
            return true;
        }
        else
        {
            // 3. Slot is not empty, merge attempt has failed, unable to place
            return false;
        }
    }
    private Slot GetClosestSlot(Tile tile)
    {
        return _allSlots
            .OrderBy(item => (item.transform.position - tile.transform.position).sqrMagnitude)
            .ToList()[0]; // Find the slot closest to where the given tile is on the screen
    }

    public void ReleaseSlotFromTile(Tile tile)
    {
        Slot slotToBeReleased = GetSlotWithTile(tile);

        // Safety Check: Only try to empty the slot if we actually found one
        if (slotToBeReleased != null)
        {
            slotToBeReleased.TileStored = null;
        }
        else
        {
            Debug.LogError($"Could not find a slot containing {tile.name}");
        }
    }

    public Slot GetSlotWithTile(Tile tile)
    {
        // TODO redudancy to be removed

        Slot foundSlot = _allSlots.FirstOrDefault(slot => slot.TileStored == tile);

        return foundSlot;
    }

    private void Awake()
    {
        // Singleton enforcement

        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }
}