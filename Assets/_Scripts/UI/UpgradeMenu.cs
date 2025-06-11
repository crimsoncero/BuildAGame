using System;
using System.Collections.Generic;
using float_oat.Desktop90;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Serialization;

public class UpgradeMenu : MonoBehaviour
{
    [FormerlySerializedAs("_windowController")] [SerializeField] private WindowHandler _windowHandler;
    [SerializeField] private List<UpgradePanel> _panelList;
    
    public void ShowUpgradePanel()
    {
        var upgradeList = HeroManager.Instance.GetUpgradesToShow();

        for (int i = 0; i < upgradeList.Count; i++)
        {
            _panelList[i].Initialize(upgradeList[i], this);
            _panelList[i].gameObject.SetActive(true);
        }
        
        _windowHandler.Show();
    }

    public void OnUpgradeClick()
    {
        foreach (var panel in _panelList)
        {
            panel.gameObject.SetActive(false);
        }
        
        GameManager.Instance.ResumeGame();
        _windowHandler.Hide();
    }
}
