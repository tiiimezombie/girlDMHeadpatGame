using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ShopTimerController : Singleton<ShopTimerController>
{
    public static event Action<int> SetPanelView;

    [SerializeField] private ShopScriptableObject _timerScriptableObject;
    [SerializeField] private TimerPanel _timerPanelPrefab;
    [SerializeField] private Transform _timerHolder;
    private Dictionary<TimerType, TimerWithPanel> _panelTimerDictionary = new Dictionary<TimerType, TimerWithPanel>();

    private bool _upgradeMode;

    void Start()
    {
        MakeTimer(TimerType.Headpat);
    }

    private void MakeTimer(TimerType type)
    {
        if (_panelTimerDictionary.ContainsKey(type)) return;

        var a = Instantiate(_timerPanelPrefab, _timerHolder);
        _panelTimerDictionary.Add(TimerType.Headpat, _timerScriptableObject.ShopDictionary[type]);
        _panelTimerDictionary[type].Setup(a);
    }

    void Update()
    {
        if (!GameController.IsPlaying) return;

        foreach (var v in _panelTimerDictionary.Values)
        {
            v.Increment();
        }
    }

    public void ToggleUpgradeMode()
    {
        _upgradeMode = !_upgradeMode;
        SetPanelView?.Invoke(_upgradeMode ? 2 : 1);
    }

    public void Claim(CurrencyType timerType, long value)
    {
        switch (timerType)
        {
            case CurrencyType.Headpats:
                CurrencyController.Instance.AddHeadpats(AudienceController.Instance.HeadPatsFromAudience);
                break;
            case CurrencyType.Money:
                CurrencyController.Instance.AddMoney(value);
                break;
            case CurrencyType.XP:
                CurrencyController.Instance.AddXP(value);
                break;
            case CurrencyType.Favor:
                AudienceController.Instance.AddFavor(value);
                break;
            case CurrencyType.ViewerCap:
                AudienceController.Instance.IncreaseViewerCap(value);
                break;
            default:
                Debug.Log(timerType.ToString());
                break;
        }
    }

    public void PanelActionButtonClicked(TimerType type)
    {
        if (!_panelTimerDictionary.ContainsKey(type)) return;

        switch (_panelTimerDictionary[type].RefreshType)
        {
            case TimerRefreshType.NeedToClaim:
                _panelTimerDictionary[type].Claim();
                break;
            case TimerRefreshType.NeedToStart:
                _panelTimerDictionary[type].Restart();
                break;
        }
    }

    public void PanelMultiplierUpgradeRequested(TimerType type)
    {
        if (!_panelTimerDictionary.ContainsKey(type)) return;

        if (CurrencyController.Instance.XP > _panelTimerDictionary[type].CurrentCostToIncreaseMultiplier)
        {
            CurrencyController.Instance.XP -= _panelTimerDictionary[type].CurrentCostToIncreaseMultiplier;
            _panelTimerDictionary[type].UpgradeMultiplier();
        }
    }

    public void PanelSpeedUpgradeRequested(TimerType type)
    {
        if (!_panelTimerDictionary.ContainsKey(type)) return;

        if (CurrencyController.Instance.Money > _panelTimerDictionary[type].CurrentCostToDecreaseDuration)
        {
            CurrencyController.Instance.Money -= _panelTimerDictionary[type].CurrentCostToDecreaseDuration;
            _panelTimerDictionary[type].UpgradeSpeed();
        }
    }
}