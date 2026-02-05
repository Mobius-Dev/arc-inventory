using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance { get; private set; }
    public GameObject _tilePrefab; // Prefab used to instantiate new content tiles

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    public Tile SpawnNewItemTile(GameObject tileObjToClone, Stack stackToAssign, Transform parentTransform)
    {
        // Instantiate a new content tile representing the given item stack under the specified parent transform
        GameObject newTileObj = Instantiate(tileObjToClone, parentTransform);
        Tile newTile = newTileObj.GetComponent<Tile>();
        newTile.AssignStack(stackToAssign);
        newTile.Initialize();

        return newTileObj.GetComponent<Tile>();
    }
}
