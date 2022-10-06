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
    private Dictionary<TimerType, UpgradeableTimer> _panelTimerDictionary = new Dictionary<TimerType, UpgradeableTimer>();

    private bool _upgradeMode;

    void Start()
    {
        MakeTimer(TimerType.Headpat);
    }

    private void MakeTimer(TimerType type)
    {
        if (_panelTimerDictionary.ContainsKey(type)) return;

        var a = Instantiate(_timerPanelPrefab, _timerHolder);
        _panelTimerDictionary.Add(TimerType.Headpat, new UpgradeableTimer(_timerScriptableObject.ShopDictionary[type], a));
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

    public void PanelActionButtonClicked(TimerType timer)
    {
        if (!_panelTimerDictionary.ContainsKey(timer)) return;

        switch (_panelTimerDictionary[timer].RefreshType)
        {
            case TimerRefreshType.NeedToClaim:
                _panelTimerDictionary[timer].Claim();
                break;
            case TimerRefreshType.NeedToStart:
                _panelTimerDictionary[timer].Restart();
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