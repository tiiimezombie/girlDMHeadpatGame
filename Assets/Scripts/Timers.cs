using System;
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

    public int CurrentCostToIncreaseMultiplier { get => (int)Mathf.Pow(_data.MultiplierUpgradeCostPerLevel, CurrentMultiplierLevel); }

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

        CurrentMultiplierLevel = 0;
        CurrentDuration = _data.InitialDuration;
        DurationIndex = 0;
        Restart();

        _timerPanel = panel;
        _timerPanel.Setup(_data.TimerType, _data.RefreshType, _data.Name);
        RefreshPanel();
    }

    public void Increment()
    {
        if (_skipCounting) return;

        CurrentTime += Time.deltaTime;
        _timerPanel.SetProgress(CurrentTime / CurrentDuration);

        // Do timer
        if (CurrentTime < CurrentDuration) return;

        // Claim
        if (_data.RefreshType == TimerRefreshType.NeedToClaim)
        {
            RefreshPanel();
            return;
        }

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
        RefreshPanel();
    }

    public void UpgradeSpeed()
    {
        DurationIndex++;
        CurrentDuration = _data.DurationUpgradeList[DurationIndex].NewDuration;
        RefreshPanel();
    }

    public void Claim()
    {
        ShopTimerController.Instance.Claim(_data.ReturnCurrency, CurrentValue);
        RefreshPanel();
    }

    public void RefreshPanel()
    {
        _timerPanel.Refresh(CurrencyController.Instance.XP >= CurrentCostToIncreaseMultiplier, CurrentCostToIncreaseMultiplier > 0 ? GameController.GetPrettyLong(CurrentCostToIncreaseMultiplier) : "-", HasDurationUpgrade &&
            CurrencyController.Instance.Money >= CurrentCostToDecreaseDuration, CurrentCostToDecreaseDuration > 0 ? CurrentCostToDecreaseDuration.ToString() : "-");
    }
}


public abstract class BaseTimerData
{
    public string Name;
    public TimerRefreshType RefreshType;

    // Duration
    public int InitialDuration = 50;
}

public enum UpgradeableTimerType
{
    Headpat,
    CritPat,
    GroupAidedPat,

    Phrase,
    Emote,
    Accessory,

    Merch,
    Ramen,
    AdPayout,

    TwitterPost,
    TiktokVideo,
    YoutubeVideo,
}

public enum StaticTimerType
{
    Raid,
    Tier,
    Gift,
    BonusChest,
}

public enum TimerRefreshType
{
    AutoRun,
    NeedToClaim,
    NeedToStart
}