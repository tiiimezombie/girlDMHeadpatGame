using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RedeemShop : BaseShop
{
    [SerializeField] private ShopScriptableObject _data;
    private Dictionary<TimerType, ShopItemButton> _shopItemButtons = new Dictionary<TimerType, ShopItemButton>();
    private Dictionary<TimerType, PurchaseStateType> _purchasedItems = new Dictionary<TimerType, PurchaseStateType>();
    private GameObject _noItemsGO;

    private enum PurchaseStateType
    {
        Locked,
        Ready,
        Purchased,
    }

    //[SerializeField] private Button[] _redeemArray;

    protected override void Start()
    {
        base.Start();

        foreach (var item in _data.ShopDictionary)
        {
            ShopItemButton a = Instantiate(_shopButtonPrefab, _scrollerButtonHolder);
            a.Init(item.Key, this, item.Value.Name, item.Value.Icon, item.Value.InitialCostCurrency, item.Value.InitialCost);
            _shopItemButtons.Add(item.Key, a);
            _purchasedItems.Add(item.Key, PurchaseStateType.Locked);
        }

        RefreshShopButtons();

        CurrencyController.RefreshShopButtons += RefreshShopButtons;
    }

    protected void OnDestroy()
    {
        CurrencyController.RefreshShopButtons -= RefreshShopButtons;
    }

    public void BuyTimer(TimerType type)
    {
        if (CurrencyController.Instance.PaidShopCost(_data.ShopDictionary[type].InitialCostCurrency, _data.ShopDictionary[type].InitialCost))
        {
            //_shopItemButtons[type].gameObject.SetActive(false);
            _purchasedItems[type] = PurchaseStateType.Purchased;
            _shopItemButtons[type].Refresh(_purchasedItems[type] == PurchaseStateType.Ready, aaa(type));
        }
    }

    protected override void RefreshShopButtons()
    {
        foreach (var v in _shopItemButtons)
        {
            v.Value.Refresh(_purchasedItems[v.Key] == PurchaseStateType.Ready, aaa(v.Key));
        }
    }

    protected string aaa(TimerType type)
    {
        switch (_purchasedItems[type])
        {
            case PurchaseStateType.Locked:
                return "Locked";
            case PurchaseStateType.Purchased:
                return "Out of Stock";
            default:
                return _data.ShopDictionary[type].GetInitialCostShopText();
        }
    }
}
