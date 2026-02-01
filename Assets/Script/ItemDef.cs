using UnityEngine;

[CreateAssetMenu(fileName = "New Item Definition", menuName = "Inventory/Item Definition")]
public class ItemDef : ScriptableObject
{
    public string DisplayName;
    public ItemType Type;
    public ItemID ItemID;
    public Sprite Sprite;
    public int MaxStackSize = 1;
}