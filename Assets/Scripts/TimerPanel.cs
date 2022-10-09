using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimerPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private GameObject _viewOneHolderGO;
    [SerializeField] private Image _progressBar;
    [SerializeField] private Button _claimButton;
    [SerializeField] private TextMeshProUGUI _claimButtonText;
    [SerializeField] private GameObject _viewTwoHolderGO;
    [SerializeField] private Button _xpUpgradeButton;
    [SerializeField] private TextMeshProUGUI _xpUpgradeButtonText;
    [SerializeField] private Button _moneyUpgradeButton;
    [SerializeField] private TextMeshProUGUI _moneyUpgradeButtonText;

    private UpgradeableTimerType _timerType;
    private TimerRefreshType _timerRefreshType;

    private void Awake()
    {
        ShopTimerController.SetPanelView += SetView;
    }

    private void OnDestroy()
    {
        ShopTimerController.SetPanelView -= SetView;
    }

    public void Setup(UpgradeableTimerType timerType, TimerRefreshType refreshType, string name)
    {
        _timerType = timerType;
        _timerRefreshType = refreshType;
        _viewOneHolderGO.SetActive(true);
        _viewTwoHolderGO.SetActive(false);
        _titleText.text = name;

        if (refreshType == TimerRefreshType.AutoRun) _claimButtonText.text = "auto";
        _claimButton.interactable = false;
    }

    public void Refresh(bool canPayXP, string xpCost, bool canPayMoney, string moneyCost)
    {
        _xpUpgradeButton.interactable = canPayXP;
        _xpUpgradeButtonText.text = xpCost;
        _moneyUpgradeButton.interactable = canPayMoney;
        _moneyUpgradeButtonText.text = moneyCost;
    }

    public void SetProgress(float progress)
    {
        _progressBar.fillAmount = Mathf.Clamp(progress, 0, 1);
    }

    public void SetView(int viewIndex)
    {
        _viewOneHolderGO.SetActive(false);
        _viewTwoHolderGO.SetActive(false);

        if (viewIndex == 2)
        {
            _viewTwoHolderGO.SetActive(true);
        }
        else
        {
            _viewOneHolderGO.SetActive(true);
        }
    }

    public void ActionButton()
    {
        ShopTimerController.Instance.PanelActionButtonClicked(_timerType);
    }

    public void BuyMultiplierUpgrade()
    {
        ShopTimerController.Instance.PanelMultiplierUpgradeRequested(_timerType);
    }

    public void BuySpeedUpgrade()
    {
        ShopTimerController.Instance.PanelSpeedUpgradeRequested(_timerType);
    }
}