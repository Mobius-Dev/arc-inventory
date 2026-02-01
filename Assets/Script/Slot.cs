using UnityEngine;

public class Slot : MonoBehaviour
{
    // This class handles an slot in the inventory which can store a tile representing a Content

    public Tile Content;

    // TODO requirements, i.e this slot only accepts weapon-type Content
    private void Start()
    {
        SlotManager.Instance.RegisterSlot(this);
    }
    public void OccupySlot(Tile newContent)
    {
        if (Content && newContent != Content)
        {
            Content.MergeTogether(newContent); // Merge the two contents if the slot is already occupied
            return;
        }
        else Content = newContent; // Otherwise, occupy the slot with the new content
    }
}