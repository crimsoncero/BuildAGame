using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class HeroSelectionIcon : MonoBehaviour
{
    private static readonly int Saturate = Shader.PropertyToID("_Saturate");
    [field: SerializeField] public HeroData Hero { get; private set; }
    [field: SerializeField] public bool IsUnlocked { get; private set; }

    [SerializeField] private Image _heroImage;
    [SerializeField] private Image _lockedHeroImageOverlay;
    
    [SerializeField] private Sprite _openTeamSlotSprite;
    [SerializeField] private Sprite _lockedTeamSlotSprite;
    private Material _imageMaterial;
    
    private bool _isTeamIcon;
    private bool _isLockedTeamIcon = false;
    
    public void InitGridIcon(HeroData hero, bool isUnlocked = false)
    {
        _isTeamIcon = false;
        Hero = hero;
        IsUnlocked = isUnlocked;
        
        _imageMaterial = new Material(_heroImage.material);
        _heroImage.material = _imageMaterial;
        
        _heroImage.sprite = Hero.MugshotSprite;
        _imageMaterial.SetFloat(Saturate, IsUnlocked ? 1f : 0f);
       
        _lockedHeroImageOverlay.enabled = !IsUnlocked;
      
    }

    public void SetTeamIcon(HeroData hero)
    {
        _isTeamIcon = true;
        _isLockedTeamIcon = false;
        _heroImage.sprite = hero.MugshotSprite;
    }

    public void SetOpenTeamIcon()
    {
        _isTeamIcon = true;
        _isLockedTeamIcon = false;
        _heroImage.sprite = _openTeamSlotSprite;
    }
    public void SetLockedTeamIcon()
    {
        _isTeamIcon = true;
        _isLockedTeamIcon = true;
        _heroImage.sprite = _lockedTeamSlotSprite;
    }
    
    
}
