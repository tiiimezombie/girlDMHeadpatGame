using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class CurrencyController : Singleton<CurrencyController>
{
    #region -- Money --
    public static event System.Action MoneyChanged;
    public long Money
    {
        get => _money;
        set
        {
            _money = value;
            _footerButtons.MoneyText.text = GameController.GetPrettyLong(_money);
            _moneyText.text = GameController.GetPrettyLong(_moneyOverall);
            MoneyChanged?.Invoke();
        }
    }
    private long _money;
    private long _moneyOverall;

    [SerializeField] private TextMeshProUGUI _moneyText;

    public void AddMoney(long money)
    {
        Money += money;
        _moneyOverall += money;
    }

    #endregion

    #region -- XP --
    public static event System.Action XPChanged;
    public long XP
    {
        get => _xp;
        set
        {
            _xp = value;
            _footerButtons.XPText.text = GameController.GetPrettyLong(_xp);
            XPChanged?.Invoke();
        }
    }
    private long _xp;

    public void AddXP(long xp)
    {
        XP += xp;
    }

    //public void AddXP(ShopType type)
    //{
    //    if (type == ShopType.ChatXPValue)
    //    {
    //        XP += ShopLibrary.ShopDictionary[type].Tier;
    //    } 
    //    else if (type == ShopType.RedeemXPValue)
    //    {
    //        XP += ShopLibrary.ShopDictionary[type].Tier;
    //    }
    //}

    #endregion

    #region -- Headpats --

    public long HeadpatsRedeemed
    {
        get => _patsRedeemed;
        set
        {
            _patsRedeemed = value;
            _patsRedeemedText.text = GameController.GetPrettyLong(_patsRedeemed); // _patsRedeemed.ToString("N0");
        }
    }
    private long _patsRedeemed = 0;

    public long PatsQueued
    {
        get => _patsQueued;
        private set
        {
            _patsQueued = value;
            _patsQueuedText.text = GameController.GetPrettyLong(_patsQueued); //_patsQueued.ToString("N0");
        }
    }
    private long _patsQueued;

    [SerializeField] private TextMeshProUGUI _patsQueuedText;
    [SerializeField] private TextMeshProUGUI _patsRedeemedText;
    [SerializeField] private MilestoneController _milestoneController;

    public void AddHeadpats(long number)
    {
        PatsQueued += number;
    }

    public long HeadPatRemainder(long combo, long numHeadPatsToCheck)
    {
        if (combo + numHeadPatsToCheck <= PatsQueued) return numHeadPatsToCheck;
        return PatsQueued - combo;
    }

    public void FulfillHeadpats(long number)
    {
        PatsQueued -= number;

        DOTween.To(() => HeadpatsRedeemed, x => HeadpatsRedeemed = x, HeadpatsRedeemed + number, 1).SetEase(Ease.OutCirc).OnComplete(() => {
            _milestoneController.CheckForHeadpatMilestone();
        });
    }

    #endregion

    public static Action RefreshShopButtons;
    [SerializeField] private FooterButtons _footerButtons;

    private void Start()
    {
        Money = 0;
        XP = 0;

        // TODO: remove in real game
        Debug.Log("boost for efficient testing");
        HeadpatsRedeemed = 9; //0;
        PatsQueued = 0;
    }

    public bool PaidShopCost(CurrencyType currency, int amt)
    {
        if (currency == CurrencyType.Money)
        {
            if (Money >= amt)
            {
                Money -= amt;
                return true;
            }
        }
        else if (currency == CurrencyType.XP)
        {
            if (XP >= amt)
            {
                XP -= amt;
                return true;
            }
        }

        return false;
    }

    //public void BuyShopItem(ShopType type)
    //{
    //    var a = ShopLibrary.ShopDictionary[type].Currency;
    //    if (a == CurrencyType.Money)
    //        Money -= ShopLibrary.ShopDictionary[type].FullCost;
    //    else if (a == CurrencyType.XP)
    //        XP -= ShopLibrary.ShopDictionary[type].FullCost;
    //    ShopLibrary.ShopDictionary[type].IncrementTier();

    //    RefreshShopButtons?.Invoke();

    //    AudienceController.Instance.SetTimerMax(type);
    //}

    //public bool CanBuyShopItem(ShopType type)
    //{
    //    var a = ShopLibrary.ShopDictionary[type].Currency;
    //    if (a == CurrencyType.Money)
    //        return Money >= ShopLibrary.ShopDictionary[type].FullCost;
    //    else if (a == CurrencyType.XP)
    //        return XP >= ShopLibrary.ShopDictionary[type].FullCost;
    //    else
    //        return false;
    //}
}
