using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private string _name;
    public string Name
    {
        get => _name;
        set => _name = value;
    }

    private bool _alive;
    public bool Alive
    {
        get => _alive;
        set
        {
            _alive = value;
        }
    }

    public void StartGameInput(InputAction.CallbackContext context)
    {
        GameController.INSTANCE.StartGameInput();
    }
}
