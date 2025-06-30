using System;
using MoreMountains.Feedbacks;
using UnityEngine;

public class WindowHandler : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private MMF_Player _showFeedback;
    [SerializeField] private MMF_Player _hideFeedback;

    public bool IsOpen = false;

    private void Start()
    {
        IsOpen = gameObject.activeSelf;
    }

    public void Show()
    {
        // Already open, no need to show again.
        if (IsOpen)
            return;
        IsOpen = true;
        _hideFeedback.StopFeedbacks();
        gameObject.SetActive(true);
        _showFeedback.PlayFeedbacks();
    }

    public void Hide()
    {
        if (!IsOpen)
            return;
        IsOpen = false;
        _showFeedback.StopFeedbacks();
        _hideFeedback.PlayFeedbacks();
    }

}
