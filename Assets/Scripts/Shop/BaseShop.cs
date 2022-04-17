using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BaseShop : MonoBehaviour
{
    public static Action RefreshButtons;
    [SerializeField] private Transform _scrollerButtonHolder;
    [SerializeField] private ShopItemButton _shopButtonPrefab;
        

    protected void Start()
    {
        gameObject.SetActive(false);

        foreach (ShopType item in System.Enum.GetValues(typeof(ShopType)))
        {
            ShopItemButton a = Instantiate(_shopButtonPrefab, _scrollerButtonHolder);
            a.Init(item, null);
        }
    }

    public void Toggle()
    {
        gameObject.SetActive(!gameObject.activeSelf);
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    //private void BuyItem(ShopType type)
    //{
    //    RefreshButtons?.Invoke();
    //}
}
