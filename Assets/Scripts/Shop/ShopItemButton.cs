using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItemButton : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _value;
    private UpgradeableTimerType _type;
    private RedeemShop _parentShop;
    private Action _onClick;

    //private int _cost;
    //private CurrencyType _currency;

    public void Init(UpgradeableTimerType item, RedeemShop parentShop, string name, Sprite icon) //, CurrencyType costCurrency, int cost
    {
        //BaseShop.RefreshButtons += Refresh;
        _type = item;
        _parentShop = parentShop;
        _name.text = name;
        _image.sprite = icon;
        //_cost = cost;
        //_currency = costCurrency;
        //Refresh();
    }

    public void OnClick()
    {
        _parentShop.BuyTimer(_type);
        //_onClick?.Invoke();
    }

    public void Refresh(bool purchaseable, string priceText)
    {
        _value.text = priceText;//CurrencyController.Instance.ShopLibrary.ShopDictionary[_type].GetCostText();
        _button.interactable = purchaseable;
    }
}
