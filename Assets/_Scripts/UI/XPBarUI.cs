using UnityEngine;
using MoreMountains.Tools;

public class XPBarUI : MonoBehaviour
{
    [SerializeField] private MMProgressBar _xpBar;

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
    }
}
