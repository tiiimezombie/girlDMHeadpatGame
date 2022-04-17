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
    [SerializeField] private TextMeshProUGUI _value;
    private ShopType _type;
    private Action _onClick;

    public void Init(ShopType item, Action onClick)
    {
        BaseShop.RefreshButtons += Refresh;
        _type = item;
        _onClick = onClick;
    }

    public void OnClick()
    {
        CurrencyController.Instance.BuyShopItem(_type);
        //_onClick?.Invoke();
    }

    private void Refresh()
    {
        _button.interactable = CurrencyController.Instance.CanBuyShopItem(_type);
    }
}
