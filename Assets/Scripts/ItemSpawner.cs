using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public static ItemSpawner Instance { get; private set; }
    public GameObject _tilePrefab; // Prefab used to instantiate new content tiles

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }
    //(Slot SlotToSpawnAt, ItemDef itemDef, int quantity, Transform parentTransform
    public Tile SpawnNewItemTile(GameObject tileObjToClone, Stack stackToAssign, Transform parentTransform)
    {
        // Instantiate a new content tile representing the given item stack under the specified parent transform
        //GameObject newTileObj = Instantiate(_tilePrefab, parentTransform);
        //GameObject newTileObj2 = Instantiate(_)
        //Tile newTile = newTileObj.GetComponent<Tile>();

        GameObject newTileObj = Instantiate(tileObjToClone, parentTransform);
        Tile newTile = newTileObj.GetComponent<Tile>();
        newTile.StackStored = stackToAssign;
        newTile.Initialize();

        return newTileObj.GetComponent<Tile>();
    }
}
