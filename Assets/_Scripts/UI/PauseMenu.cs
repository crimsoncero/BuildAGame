using float_oat.Desktop90;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private WindowController _windowController;

    public void Open()
    {
        _windowController.Open();
    }
    public void ResumeGame()
    {
        _windowController.Close();
        GameManager.Instance.ResumeGame();
    }

    public void OpenSettings()
    {
        
    }
    
    public void ReturnToMainMenu()
    {
        _windowController.Close();
        GameManager.Instance.GameOver();
    }

    public void ExitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}
