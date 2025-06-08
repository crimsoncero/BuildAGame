using System;
using UnityEngine;

public class TestingModule : MonoBehaviour
{
    public WindowHandler window;
    
    private void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 100, 30), "Open"))
        {
            window.Show();
        }

        if (GUI.Button(new Rect(10, 30, 100, 30), "Close"))
        {
            window.Hide();
        }
    }
}
