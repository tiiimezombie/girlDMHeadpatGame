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

    private TimerType _timerType;

    private void Awake()
    {
        TimerController.SetPanelView += SetView;
    }

    private void OnDestroy()
    {
        TimerController.SetPanelView -= SetView;
    }

    public void Setup(TimerType timerType, string name, bool canPayXP, string xpCost, bool canPayMoney, string moneyCost)
    {
        _timerType = timerType;
        _viewOneHolderGO.SetActive(true);
        _viewTwoHolderGO.SetActive(false);
        _titleText.text = name;

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
        TimerController.Instance.PanelActionButtonClicked(_timerType);
    }

    public void BuyMultiplierUpgrade()
    {
        TimerController.Instance.PanelMultiplierUpgradeRequested(_timerType);
    }

    public void BuySpeedUpgrade()
    {
        TimerController.Instance.PanelSpeedUpgradeRequested(_timerType);
    }
}