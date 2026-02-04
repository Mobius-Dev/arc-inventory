public enum ItemType // General item categories
{
    Weapon,
    Consumable,
    CraftingMaterial
}

public enum SlotType // Slot acceptance criteria
{
    Any,
    WeaponOnly,
    ConsumableOnly,
    CraftingMaterialOnly
}

public enum ItemID // Specific item identifiers
{
    Gun_Pistol,
    Material_Metal,
    Material_Wood,
    Consumable_Medkit
}

public enum PlacementResult // Result of attempting to place a tile into a slot
{
    Failed,
    MovedToEmpty,
    MergedPartially,
    MergedFully
}

public enum MergeResult // Result of attempting to merge two stacks
{
    Failed,
    Full,
    Partial
}