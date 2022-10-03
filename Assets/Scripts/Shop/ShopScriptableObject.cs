using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ShopScriptableObject : ScriptableObject
{
#if UNITY_EDITOR
    [MenuItem("Assets/Create/TZ/Shop")]
    public static ShopScriptableObject Create()
    {
        ShopScriptableObject asset = ScriptableObject.CreateInstance<ShopScriptableObject>();

        AssetDatabase.CreateAsset(asset, "Assets/ShopScriptableObject.asset");
        AssetDatabase.SaveAssets();
        return asset;
    }
#endif

    public ShopDictionary ShopDictionary = new ShopDictionary();
    public MilestoneDictionary MilestoneDictionary = new MilestoneDictionary();
    public PanelTimerDictionary TimerDictionary = new PanelTimerDictionary();
}

public enum CurrencyType
{
    Money,
    XP,
    Milestone,
    Favor,
    Headpats,
    ViewerCap
}

[System.Serializable]
public class MilestoneDictionary : SerializableDictionary<ShopType, ShopItem> { }

#region -- Shop Items --
public enum ShopType
{
    HeadpatDelay,
    HeadpatValue,
    ChatXPValue,
    DonationDelay,
    DonationValue,
    RedeemXPValue,
    SubDelay,
    SubValue,
    ChatCommands,
    HypeTrainDelay,
    ClickValue,
    ClickCritValue,

    Socials,
    RaidValue,
    Partnerships,
    SubDiscount,
}

[System.Serializable]
public class ShopItem
{
    public string Name;
    public Sprite Sprite;
    public CurrencyType Currency = CurrencyType.XP;
    public int Tier = 1;
    public int BaseCost = 1;
    //    public int UnlockRequirement;

    public int FullCost { get => BaseCost * Tier; }

    public void IncrementTier()
    {
        Tier++;
    }

    public string GetCostText()
    {
        if (Currency == CurrencyType.Money)
            return "$" + FullCost;
        else if (Currency == CurrencyType.XP)
            return "<sprite name=\"viewerIcon\"> " + FullCost;
        else
            return FullCost.ToString();
    }

    public void Reset()
    {
        Tier = 1;
    }
}

[System.Serializable]
public class ShopDictionary : SerializableDictionary<ShopType, ShopItem> { }

#endregion

#region -- Timers --

public enum TimerType
{
    Raid,
    Tier,
    Gift,

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

    BonusChest,
}

public enum TimerRefreshType
{
    AutoRun,
    NeedToClaim,
    NeedToStart
}

public abstract class BaseTimer
{
    public string Name;
    public TimerType TimerType;
    public TimerRefreshType RefreshType;

    protected float CurrentTime;
    protected bool _skipIncrement;

    // Duration
    public float InitialDuration = 50;
    public float CurrentDuration { get; set; }

    // Return Value
    public int InitialReturnValue = 1;

    public void Restart()
    {
        CurrentTime = 0;
        _skipIncrement = false;
    }
}

public class SpecialTimer : BaseTimer
{
    //public long CurrentValue;
    private event System.Action OnClaim;

    public void Setup(System.Action onClaim)
    {
        OnClaim = onClaim;
        CurrentDuration = InitialDuration;
        Restart();
    }

    public void Increment(float valueToIncrement)
    {
        if (_skipIncrement) return;

        // Do timer
        CurrentTime += valueToIncrement;
        if (CurrentTime < CurrentDuration) return;

        // Claim
        if (RefreshType == TimerRefreshType.NeedToClaim) return;

        OnClaim?.Invoke();

        _skipIncrement = true;

        // Rerun
        if (RefreshType == TimerRefreshType.AutoRun)
        {
            Restart();
        }
    }
}


[System.Serializable]
public class TimerWithPanel : BaseTimer
{
    public CurrencyType ReturnCurrency;
    private TimerPanel _timerPanel;

    // Duration
    private int DurationIndex = 0;
    public List<TimerDurationUpgade> DurationUpgradeList = new List<TimerDurationUpgade>();
    public int CurrentCostToDecreaseDuration
    {
        get
        {
            if (DurationUpgradeList.Count == 0 || !HasDurationUpgrade)
                return 0;

            return DurationUpgradeList[DurationIndex].Cost;
        }
    }
    public bool HasDurationUpgrade { get => DurationIndex > DurationUpgradeList.Count; }

    // Multiplier
    public int InitalMultiplerLevel = 1;
    private int CurrentMultiplierLevel;
    public int MultiplierLevelValue = 1;
    public int MultiplierUpgradeCostPerLevel = 2;
    
    public long CurrentValue { get => InitialReturnValue + CurrentMultiplierLevel * MultiplierLevelValue; }

    public long CurrentCostToIncreaseMultiplier { get => CurrentMultiplierLevel * MultiplierUpgradeCostPerLevel; }

    public void Setup(TimerPanel panel)
    {
        CurrentMultiplierLevel = InitalMultiplerLevel;

        DurationIndex = 0;
        CurrentDuration = InitialDuration;
        _skipIncrement = false;
        _timerPanel = panel;
        _timerPanel.Setup(TimerType, Name, CurrencyController.Instance.XP > CurrentCostToIncreaseMultiplier, GameController.GetPrettyDouble(CurrentCostToIncreaseMultiplier), HasDurationUpgrade && CurrencyController.Instance.Money > CurrentCostToDecreaseDuration, CurrentCostToDecreaseDuration.ToString());
    }

    public void Increment()
    {
        if (_skipIncrement) return;

        CurrentTime += Time.deltaTime;
        _timerPanel.SetProgress(CurrentTime / CurrentDuration);

        // Do timer
        if (CurrentTime < CurrentDuration) return;

        // Claim
        if (RefreshType == TimerRefreshType.NeedToClaim) return;

        Claim();

        _skipIncrement = true;

        // Rerun
        if (RefreshType == TimerRefreshType.AutoRun)
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
        CurrentDuration = DurationUpgradeList[DurationIndex].NewDuration;
    }

    public void Claim()
    {
        TimerController.Instance.Claim(ReturnCurrency, CurrentValue);
    }
}

[System.Serializable]
public class TimerDurationUpgade
{
    public int Cost;
    public float NewDuration;
}

[System.Serializable]
public class PanelTimerDictionary : SerializableDictionary<TimerType, TimerWithPanel> { }

#endregion