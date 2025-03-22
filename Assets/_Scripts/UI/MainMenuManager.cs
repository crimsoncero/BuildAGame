using System;
using System.Collections.Generic;
using float_oat.Desktop90;
using MoreMountains.Tools;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private WindowController _characterSelect;
    
    private List<WindowController> _windows;

    private void Start()
    {
        _windows = new List<WindowController>();
        _windows.Add(_characterSelect);
        
        MMPlaylistStopEvent.Trigger(0);
        MMPlaylistPlayEvent.Trigger(0);
    }

    public void OpenCharacterSelect()
    {
        CloseAllWindows();
        _characterSelect.Open();
    }

    public void CloseAllWindows()
    {
        foreach (WindowController window in _windows)
        {
            if(window.gameObject.activeSelf)
                window.Close();
        }
    }
    
    public void QuitGame()
    {
        // Add saving check method
        
        
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}
