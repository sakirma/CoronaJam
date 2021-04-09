using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private string _name;
    public string Name
    {
        get => _name;
        set => _name = value;
    }
}
