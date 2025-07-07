using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HeroSelectionIcon : MonoBehaviour, IPointerClickHandler
{
    private static readonly int Saturate = Shader.PropertyToID("_Saturate");
    [field: SerializeField] public HeroData Hero { get; private set; }
    [field: SerializeField] public bool IsUnlocked { get; private set; }

    [SerializeField] private Image _heroImage;
    [SerializeField] private Image _lockedHeroImageOverlay;
    
    [SerializeField] private Sprite _openTeamSlotSprite;
    [SerializeField] private Sprite _lockedTeamSlotSprite;

    [SerializeField] private MMF_Player _onSelect;
    [SerializeField] private MMF_Player _onDeselect;
    [SerializeField] private MMF_Player _onPointerEnter;
    [SerializeField] private MMF_Player _onPointerExit;
    
    private Material _imageMaterial;
    
    private bool _isTeamIcon;
    private bool _isLockedTeamIcon = false;

    private TeamSelection _teamSelection;
    private bool _isSelected = false;
    
    public void InitGridIcon( TeamSelection teamSelection, HeroData hero, bool isUnlocked = false)
    {
        _teamSelection = teamSelection;
        _isTeamIcon = false;
        Hero = hero;
        IsUnlocked = isUnlocked;
        
        _imageMaterial = new Material(_heroImage.material);
        _heroImage.material = _imageMaterial;
        
        _heroImage.sprite = Hero.MugshotSprite;
        _imageMaterial.SetFloat(Saturate, IsUnlocked ? 1f : 0f);
       
        _lockedHeroImageOverlay.enabled = !IsUnlocked;
      
    }

    public void InitTeamIcon(TeamSelection teamSelection)
    {
        _teamSelection = teamSelection;
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

    public void SetSelected(bool isSelected)
    {
        _isSelected = isSelected;
        if (isSelected)
        {
            _onSelect.PlayFeedbacks();
        }
        else
        {
            _onDeselect.PlayFeedbacks();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!IsUnlocked) return;
        
        if (eventData.clickCount == 1)
        {
            if(!_isSelected)
                _teamSelection.SelectHero(Hero);
        }

        if (eventData.clickCount == 2)
        {
            
        }
    }

    
}
