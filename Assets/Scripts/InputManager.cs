using UnityEngine;

public class InputManager : MonoBehaviour
{

    public static InputManager Instance { get; private set; }

    [SerializeField] private KeyCode _splitModifierKey = KeyCode.LeftShift;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    public bool IsSplitModifierPressed()
    {
        if (Input.GetKey(_splitModifierKey)) Debug.Log("LShift is dectected");
        return Input.GetKey(_splitModifierKey);
    }
}
