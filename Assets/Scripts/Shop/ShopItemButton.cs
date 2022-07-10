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
    private ShopType _type;
    private BaseShop _parentShop;
    private Action _onClick;

    public void Init(ShopType item, BaseShop parentShop)
    {
        BaseShop.RefreshButtons += Refresh;
        _type = item;
        _parentShop = parentShop;
        _name.text = CurrencyController.Instance.ShopLibrary.ShopDictionary[_type].Name;
        _image.sprite = CurrencyController.Instance.ShopLibrary.ShopDictionary[_type].Sprite;
        _value.text = CurrencyController.Instance.ShopLibrary.ShopDictionary[_type].GetCostText();
        Refresh();
    }

    public void OnClick()
    {
        CurrencyController.Instance.BuyShopItem(_type);
        //_onClick?.Invoke();
    }

    public void Refresh()
    {
        _value.text = CurrencyController.Instance.ShopLibrary.ShopDictionary[_type].GetCostText();
        _button.interactable = CurrencyController.Instance.CanBuyShopItem(_type);
    }
}
