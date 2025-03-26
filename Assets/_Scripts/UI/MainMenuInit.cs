using UnityEngine;

public class MainMenuInit : MonoBehaviour
{
    [SerializeField] private MainMenuManager _manager;
    void Start()
    {
        _manager.Initialize();
    }

}
