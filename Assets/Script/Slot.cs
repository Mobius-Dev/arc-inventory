using UnityEngine;

public class Slot : MonoBehaviour
{
    // This class handles an slot in the inventory which can store a tile representing a Content

    [SerializeField] private Content _content;

    public Content ReadContent => _content;

    // TODO requirements, i.e this slot only accepts weapon-type Content
    public void OccupySlot(Content newContent)
    {
        if (_content)
        {
            _content.MergeTogether(newContent); // Merge the two contents if the slot is already occupied
            return;
        }
        else _content = newContent; // Otherwise, occupy the slot with the new content
    }

    public void ReleaseSlot()
    {
        _content = null;
        // TODO is this enough?
    }

    private void Start()
    {
        SlotManager.Instance.RegisterSlot(this);
    }
}