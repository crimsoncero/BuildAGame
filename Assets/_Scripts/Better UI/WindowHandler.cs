using System;
using MoreMountains.Feedbacks;
using UnityEngine;

public class WindowHandler : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private MMF_Player _showFeedback;
    [SerializeField] private MMF_Player _hideFeedback;

    public bool IsOpen
    {
        get => gameObject.activeSelf;
    }
    
    public void Show()
    {
        // Already open, no need to show again.
        if (IsOpen)
            return;
        
        gameObject.SetActive(true);
        _showFeedback.PlayFeedbacks();
    }

    public void Hide()
    {
        if (!IsOpen)
            return;
        _hideFeedback.PlayFeedbacks();
    }

}
