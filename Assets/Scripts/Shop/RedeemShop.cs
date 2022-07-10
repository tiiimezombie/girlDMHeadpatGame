using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RedeemShop : BaseShop
{
    [SerializeField] private ShopScriptableObject _data;
    private List<ShopItemButton> _shopItemArray = new List<ShopItemButton>();

    //[SerializeField] private Button[] _redeemArray;

    protected override void Start()
    {
        base.Start();

        foreach (var item in _data.ShopDictionary)
        {
            ShopItemButton a = Instantiate(_shopButtonPrefab, _scrollerButtonHolder);
            a.Init(item.Key, this);
            _shopItemArray.Add(a);
        }

        //_data.ShopDictionary

        //foreach (ShopType item in System.Enum.GetValues(typeof(ShopType)))
        //{
        //    ShopItemButton a = Instantiate(_shopButtonPrefab, _scrollerButtonHolder);
        //    a.Init(item, null);
        //}

        CurrencyController.RefreshShopButtons += RefreshShopButtons;
    }

    protected void OnDestroy()
    {
        CurrencyController.RefreshShopButtons -= RefreshShopButtons;
    }

    protected override void RefreshShopButtons()
    {
        foreach (var v in _shopItemArray)
        {
            v.Refresh();
        }
    }
}
