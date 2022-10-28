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

    //public ShopDictionary ShopDictionary = new ShopDictionary();
    //public MilestoneDictionary MilestoneDictionary = new MilestoneDictionary();
    //public StaticTimerDictionary StaticTimerDictionary = new StaticTimerDictionary();
    public UpgradeableTimerDictionary ShopDictionary = new UpgradeableTimerDictionary();
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

//[System.Serializable]
//public class MilestoneDictionary : SerializableDictionary<ShopType, ShopItem> { }

#region -- Shop Items --
//public enum ShopType
//{
//    HeadpatDelay,
//    HeadpatValue,
//    ChatXPValue,
//    DonationDelay,
//    DonationValue,
//    RedeemXPValue,
//    SubDelay,
//    SubValue,
//    ChatCommands,
//    HypeTrainDelay,
//    ClickValue,
//    ClickCritValue,

//    Socials,
//    RaidValue,
//    Partnerships,
//    SubDiscount,
//}

//[System.Serializable]
//public class ShopItem
//{
//    public string Name;
//    public Sprite Sprite;
//    public CurrencyType Currency = CurrencyType.XP;
//    public int Tier = 1;
//    public int BaseCost = 1;
//    //    public int UnlockRequirement;

//    public int FullCost { get => BaseCost * Tier; }

//    public void IncrementTier()
//    {
//        Tier++;
//    }



//    public void Reset()
//    {
//        Tier = 1;
//    }
//}

//[System.Serializable]
//public class ShopDictionary : SerializableDictionary<ShopType, ShopItem> { }

#endregion

#region -- Timers --


[System.Serializable]
public class UpgradeableTimerData : BaseTimerData
{
    public CurrencyType ReturnCurrency;
    public int InitialReturnValue = 1;

    #region --- Unlock ---
    public Sprite Icon;
    public CurrencyType UnlockCurrency = CurrencyType.XP;
    public int UnlockCost = 5;

    public string GetUnlockCostShopText()
    {
        if (UnlockCurrency == CurrencyType.Money)
            return "$" + UnlockCost;
        else if (UnlockCurrency == CurrencyType.XP)
            return "<sprite name=\"viewerIcon\"> " + UnlockCost;
        else
            return UnlockCost.ToString();
    }

    #endregion

    // Duration
    public List<TimerDurationUpgade> DurationUpgradeList = new List<TimerDurationUpgade>();
    
    // Multiplier
    public int MultiplierLevelValue = 1;
    public int MultiplierUpgradeCostPerLevel = 2;
}

[System.Serializable]
public class TimerDurationUpgade
{
    public int Cost;
    public float NewDuration;
}

[System.Serializable]
public class UpgradeableTimerDictionary : SerializableDictionary<UpgradeableTimerType, UpgradeableTimerData> { }

#endregion