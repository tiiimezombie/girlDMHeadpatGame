using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public abstract class BaseTimer
{
    public float CurrentDuration;
    public float CurrentTime;
    protected bool _skipCounting;

    public void Restart()
    {
        CurrentTime = 0;
        _skipCounting = false;
    }
}

public class StaticTimer : BaseTimer
{
    private StaticTimerData _data;
    private event System.Action OnClaim;

    public StaticTimer(StaticTimerData data, System.Action onClaim)
    {
        _data = data;
        OnClaim = onClaim;
        CurrentDuration = _data.InitialDuration;
        Restart();
    }

    public void Increment(float valueToIncrement)
    {
        if (_skipCounting) return;

        // Do timer
        CurrentTime += valueToIncrement;
        if (CurrentTime < CurrentDuration) return;

        // Claim
        if (_data.RefreshType == TimerRefreshType.NeedToClaim) return;

        OnClaim?.Invoke();

        _skipCounting = true;

        // Rerun
        if (_data.RefreshType == TimerRefreshType.AutoRun)
        {
            Restart();
        }
    }
}

public class UpgradeableTimer : BaseTimer
{
    public TimerRefreshType RefreshType { get => _data.RefreshType; }

    public long CurrentValue { get => _data.InitialReturnValue + CurrentMultiplierLevel * _data.MultiplierLevelValue; }

    public long CurrentCostToIncreaseMultiplier { get => CurrentMultiplierLevel * _data.MultiplierUpgradeCostPerLevel; }

    public int CurrentCostToDecreaseDuration
    {
        get
        {
            if (_data.DurationUpgradeList.Count == 0 || !HasDurationUpgrade)
                return 0;

            return _data.DurationUpgradeList[DurationIndex].Cost;
        }
    }
    public bool HasDurationUpgrade { get => DurationIndex > _data.DurationUpgradeList.Count; }

    private UpgradeableTimerData _data;
    private TimerPanel _timerPanel;

    private int CurrentMultiplierLevel;
    private int DurationIndex = 0;

    public UpgradeableTimer(UpgradeableTimerData data, TimerPanel panel)
    {
        _data = data;

        CurrentMultiplierLevel = 1;
        CurrentDuration = _data.InitialDuration;
        DurationIndex = 0;
        Restart();

        _timerPanel = panel;
        _timerPanel.Setup(_data.TimerType, _data.Name, CurrencyController.Instance.XP > CurrentCostToIncreaseMultiplier, GameController.GetPrettyLong(CurrentCostToIncreaseMultiplier), HasDurationUpgrade && CurrencyController.Instance.Money > CurrentCostToDecreaseDuration, CurrentCostToDecreaseDuration.ToString());
    }

    public void Increment()
    {
        if (_skipCounting) return;

        CurrentTime += Time.deltaTime;
        _timerPanel.SetProgress(CurrentTime / CurrentDuration);

        // Do timer
        if (CurrentTime < CurrentDuration) return;

        // Claim
        if (_data.RefreshType == TimerRefreshType.NeedToClaim) return;

        Claim();

        _skipCounting = true;

        // Rerun
        if (_data.RefreshType == TimerRefreshType.AutoRun)
        {
            Restart();
        }
    }

    public void UpgradeMultiplier()
    {
        CurrentMultiplierLevel++;
    }

    public void UpgradeSpeed()
    {
        DurationIndex++;
        CurrentDuration = _data.DurationUpgradeList[DurationIndex].NewDuration;
    }

    public void Claim()
    {
        ShopTimerController.Instance.Claim(_data.ReturnCurrency, CurrentValue);
    }
}
