using UnityEngine;

public class StackManager : MonoBehaviour
{
    // A singleton which manages Stack related operations

    public static StackManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    public bool AttemptMerge(Stack stackA, Stack stackB)
    {
        // Attempt to merge stackB into stackA

        if (stackA.ItemStored.ItemID == stackB.ItemStored.ItemID)
        {
            int totalQuantity = stackA.QuantityStored + stackB.QuantityStored;

            if (totalQuantity <= stackA.ItemStored.MaxStackSize)
            {
                stackA.QuantityStored = totalQuantity;
                stackB.QuantityStored = 0; // Emptied stackB
                return true;
            }
            else
            {
                int spaceLeft = stackA.ItemStored.MaxStackSize - stackA.QuantityStored;
                stackA.QuantityStored += spaceLeft;
                stackB.QuantityStored -= spaceLeft;
                return true;
            }
        }
        return false; // Requirements for merging not met
    }

    public bool AttemptSplit(Stack originalStack, out Stack newStack)
    {
        // Attempt to split the original stack into two stacks of equal quantity

        if (originalStack.QuantityStored > 1)
        {
            int splitQuantity = originalStack.QuantityStored / 2;
            originalStack.QuantityStored -= splitQuantity;
            newStack = new Stack(originalStack.ItemStored, splitQuantity);
            return true;
        }

        newStack = null;
        return false; // Not enough quantity to split
    }
}
