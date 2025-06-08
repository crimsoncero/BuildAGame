using System;
using System.Collections.Generic;
using float_oat.Desktop90;
using MoreMountains.Feedbacks;
using UnityEngine;

public class UpgradeMenu : MonoBehaviour
{
    [SerializeField] private WindowController _windowController;
    [SerializeField] private List<UpgradePanel> _panelList;
    [SerializeField] private MMF_Player _particlePlayer;
    
    public void ShowUpgradePanel()
    {
        var upgradeList = HeroManager.Instance.GetUpgradesToShow();

        for (int i = 0; i < upgradeList.Count; i++)
        {
            _panelList[i].Initialize(upgradeList[i], this);
            _panelList[i].gameObject.SetActive(true);
        }

        _windowController.Open();
        _particlePlayer.PlayFeedbacks();
    }

    public void OnUpgradeClick()
    {
        foreach (var panel in _panelList)
        {
            panel.gameObject.SetActive(false);
        }
        
        GameManager.Instance.ResumeGame();
        _windowController.Close();
    }
}
