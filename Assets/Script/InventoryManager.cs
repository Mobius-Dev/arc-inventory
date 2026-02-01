using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    // A singleton which manages the overall inventory system
    public static InventoryManager Instance { get; private set; }

    private void Awake()
    {
        // Singleton enforcement
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }
}
