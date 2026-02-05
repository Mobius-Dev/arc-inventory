using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SlotManager : MonoBehaviour
{
    // A singleton which holds knowledge of and runs operations on all inventory slots
    public static SlotManager Instance { get; private set; }

    private List<Slot> _allSlots = new();

    private void Awake()
    {
        // Singleton enforcement
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    public void RegisterSlot(Slot slot)
    {
        //Called by an instance by Slot when it is created to register itself with the manager
        if (!_allSlots.Contains(slot)) _allSlots.Add(slot);
        else Debug.LogWarning($"{slot.gameObject.name} tried to register multiple times!");
    }

    public void ReleaseSlotFromTile(Tile tile)
    {
        //Release a slot holding a given tile
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
        // Returns a slot holding a given tile
        Slot foundSlot = _allSlots.FirstOrDefault(slot => slot.TileStored == tile);
        return foundSlot;
    }

    public void PlaceTileFromDrag(Tile tileToPlace, Slot fallbackSlot)
    {
        Slot selectedSlot = GetClosestSlot(tileToPlace);

        PlacementResult placementResult = TryPlaceTileAt(selectedSlot, tileToPlace);

        switch(placementResult)
        {
            case PlacementResult.MergedFully:
                // Tile was fully merged, no need to place anything
                return;
            case PlacementResult.MovedToEmpty:
                selectedSlot.TileStored = tileToPlace;
                break;
            case PlacementResult.MergedPartially:
                // Partial Success: We merged some, but have leftovers.
                // The leftovers must go back to the fallback slot.
                SnapTileBack(tileToPlace, fallbackSlot);
                return;
            case PlacementResult.Failed:
            default:
                // Failure: We couldn't do anything at the target. 
                // Send the whole thing back.
                SnapTileBack(tileToPlace, fallbackSlot);
                return;
        }
    }

    private void SnapTileBack(Tile tileToPlace, Slot fallbackSlot)
    {
        PlacementResult placementResult = TryPlaceTileAt(fallbackSlot, tileToPlace);
        switch (placementResult)
        {
            case PlacementResult.MergedFully:
                // Tile was fully merged, no need to place anything
                return;
            case PlacementResult.MovedToEmpty:
                fallbackSlot.TileStored = tileToPlace;
                break;
            default:
                Debug.LogError($"Could not fully return tile {tileToPlace.name} to its fallback slot {fallbackSlot.name}. This should never happen!");
                break;
        }
    }

    private PlacementResult TryPlaceTileAt(Slot targetSlot, Tile targetTile)
    {
        // Handle Empty Slot (Guard Clause)
        if (!targetSlot.TileStored)
        {
            return PlacementResult.MovedToEmpty;
        }

        // Handle Occupied Slot (Attempt Merge)
        if (!StackManager.Instance.AttemptMerge(targetSlot.TileStored.StackStored, targetTile.StackStored))
        {
            return PlacementResult.Failed;
        }

        // Handle Cleanup
        if (targetTile.StackStored.QuantityStored == 0)
        {
            Destroy(targetTile.gameObject);
            return PlacementResult.MergedFully;
        }

        return PlacementResult.MergedPartially;
    }
    private Slot GetClosestSlot(Tile tile)
    {
        return _allSlots
            .OrderBy(item => (item.transform.position - tile.transform.position).sqrMagnitude)
            .ToList()[0]; // Find the slot closest to where the given tile is on the screen
    }
}