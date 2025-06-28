using System;
using MoreMountains.Feedbacks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Message : MonoBehaviour
{
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _messageText;
    [SerializeField] private Image _characterSprite;
    [SerializeField] private MMF_Player _popFeedback;
    
    [SerializeField] private CanvasGroup _canvasGroup;

    public bool IsActive { get { return _popFeedback.IsPlaying; } }

    private void Awake()
    {
        _canvasGroup.alpha = 0;
    }

    public void PopRandomMessage(HeroData character, string message, float duration)
    {
        _nameText.text = character.Messages.Name;
        _characterSprite.sprite = character.CharacterSprite;

        _messageText.text = message;
        var pauseFB = _popFeedback.GetFeedbackOfType<MMF_Pause>();
        pauseFB.PauseDuration = duration;
        _popFeedback.PlayFeedbacks();
    }

    
}
