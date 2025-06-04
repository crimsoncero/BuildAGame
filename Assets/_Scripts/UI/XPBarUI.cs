using UnityEngine;
using MoreMountains.Tools;
using TMPro;

public class XPBarUI : MonoBehaviour
{
    [SerializeField] private MMProgressBar _xpBar;
    [SerializeField] private TMP_Text _levelText;
    private void Start()
    {
        XPManager.Instance.OnXPChanged += UpdateBar;
    }

    private void UpdateBar()
    {
        float val = 0f;
        if(XPManager.Instance != null)
        {
            val = XPManager.Instance.CurrentXP01;
        }

        _xpBar.UpdateBar01(val);
        _levelText.text = "Lv " + XPManager.Instance.CurrentLevel.ToString();
    }
}
