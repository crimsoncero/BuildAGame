using MoreMountains.Feedbacks;
using UnityEngine;

public class WindowHandler : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private MMF_Player _showFeedback;
    [SerializeField] private MMF_Player _hideFeedback;

    [field: SerializeField, Tooltip("Define the initial state of this window in the UI")]
    public bool IsOpen { get; private set; } = false;
    
    public void Show()
    {
        // Already open, no need to show again.
        if (IsOpen)
            return;
        IsOpen = true;
        _showFeedback.PlayFeedbacks();
    }

    public void Hide()
    {
        if (!IsOpen)
            return;
        IsOpen = false;
        _hideFeedback.PlayFeedbacks();
    }
}
