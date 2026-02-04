using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tile : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    // This class handles visual representation of an item stack in the inventory UI

    public Stack StackStored;

    public Slot LastOccupiedSlot;
    // If dragging we need to remember where the tile came from in case no new valid slot for this item is found
    // TODO remove hack this shouldnt ever be set manually

    [SerializeField] private Image _image; //Image element to show the item's icon
    [SerializeField] private TextMeshProUGUI _itemCount; //Text element to show the quantity of items in this tile

    [Header("Debug")]
    [SerializeField] ItemDef _itemDef;
    [Header("Force count")]
    [Tooltip("Sets this to positive value to enforce a given item count on awake")]
    [SerializeField] private int _forceCount = -1;

    private void Awake()
    {
        if (!_itemCount) Debug.LogError(($"{gameObject.name} is missing its item count text element!"));
        if (!_image) Debug.LogError(($"{gameObject.name} is missing its image element!"));

        Initialize();
    }

    public void Initialize()
    {
        if (StackStored == null) // TODO Remove Hack
        {
            StackStored = new Stack(_itemDef, 1);
        }
        if (_forceCount > 0)
        {
            StackStored.QuantityStored = _forceCount;
            _forceCount = -1; // TODO remove hack
        }

        _image.sprite = StackStored.ItemStored.Sprite;

        RefreshTile();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Slot _lastOccupied = SlotManager.Instance.GetSlotWithTile(this);

        if (InputManager.Instance.IsSplitModifierPressed() && StackManager.Instance.AttemptSplit(this.StackStored, out Stack splitStack))
        {
            RefreshTile();
            Tile splitTile = ItemSpawner.Instance.SpawnNewItemTile(this.gameObject, splitStack, this.transform.parent);
            DragManager.Instance.StartDragging(splitTile, _lastOccupied, eventData);
        }
        else
        {
            SlotManager.Instance.ReleaseSlotFromTile(this);
            DragManager.Instance.StartDragging(this, _lastOccupied, eventData);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        DragManager.Instance.UpdatePosition(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        DragManager.Instance.FinishDragging(eventData);
    }

    public void RefreshTile()
    {
        //Update the visual representation of this tile based on its current data

        Slot currentSlot = SlotManager.Instance.GetSlotWithTile(this);
        if (currentSlot)
        {
            Transform slotTransform = SlotManager.Instance.GetSlotWithTile(this).transform;
            transform.SetParent(slotTransform.transform, false); // Move the content tile to be a child of the slot
            transform.localPosition = Vector3.zero; // Center the content tile in the slot
        }

        _itemCount.text = StackStored.QuantityStored.ToString();
    }
}
