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
}

public enum CurrencyType
{
    Money,
    XP,
    Milestone
}

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

[System.Serializable]
public class MilestoneDictionary : SerializableDictionary<ShopType, ShopItem> { }