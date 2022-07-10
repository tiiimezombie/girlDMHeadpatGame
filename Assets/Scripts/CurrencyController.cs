using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;



public class CurrencyController : Singleton<CurrencyController>
{
    #region -- Money --
    public double Money
    {
        get => _money;
        set
        {
            _money = value;
            _moneyText.text = _money.ToString("N0");
        }
    }
    private double _money;

    [SerializeField] private TextMeshProUGUI _moneyText;

    public void AddMoney(double money)
    {
        Money += money;
    }

    #endregion

    #region -- XP --

    public double XP
    {
        get => _xp;
        set
        {
            _xp = value;
            _xpText.text = _xp.ToString("N0");
        }
    }
    private double _xp;

    [SerializeField] private TextMeshProUGUI _xpText;

    public void AddXP(double xp)
    {
        XP += xp;
    }

    public void AddXP(ShopType type)
    {
        if (type == ShopType.ChatXPValue)
        {
            XP += ShopLibrary.ShopDictionary[type].Tier;
        } 
        else if (type == ShopType.RedeemXPValue)
        {
            XP += ShopLibrary.ShopDictionary[type].Tier;
        }
    }

    #endregion

    public static Action RefreshShopButtons;
    public ShopScriptableObject ShopLibrary;

    //public static Dictionary<ShopType, int> ShopTiersPurchased = new Dictionary<ShopType, int>();

    private void Start()
    {
        Money = 0;
        XP = 0;

        foreach (var thing in ShopLibrary.ShopDictionary)
        {
            thing.Value.Reset();
        }

        //foreach (var v in System.Enum.GetValues(typeof(ShopType)))
        //{
        //    ShopTiersPurchased.Add((ShopType)v, 1);
        //}
    }

    public void BuyShopItem(ShopType type)
    {
        var a = ShopLibrary.ShopDictionary[type].Currency;
        if (a == CurrencyType.Money)
            Money -= ShopLibrary.ShopDictionary[type].FullCost;
        else if (a == CurrencyType.XP)
            XP -= ShopLibrary.ShopDictionary[type].FullCost;
        ShopLibrary.ShopDictionary[type].IncrementTier();

        RefreshShopButtons?.Invoke();

        AudienceController.Instance.SetTimerMax(type);
    }

    public bool CanBuyShopItem(ShopType type)
    {
        var a = ShopLibrary.ShopDictionary[type].Currency;
        if (a == CurrencyType.Money)
            return Money >= ShopLibrary.ShopDictionary[type].FullCost;
        else if (a == CurrencyType.XP)
            return XP >= ShopLibrary.ShopDictionary[type].FullCost;
        else
            return false;
    }
}
