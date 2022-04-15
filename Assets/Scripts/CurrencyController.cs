using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum ShopType
{
    HeadpatRedeemTier,
    ClickValue,
    ClickCritChance,
    ClickCritValue
}

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

    #endregion

    public static Dictionary<ShopType, int> ShopTiersPurchased = new Dictionary<ShopType, int>();

    private void Start()
    {
        Money = 0;
        XP = 0;

        foreach (var v in System.Enum.GetValues(typeof(ShopType)))
        {
            ShopTiersPurchased.Add((ShopType)v, 1);
        }
    }
}
