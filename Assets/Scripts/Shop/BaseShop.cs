using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class BaseShop : MonoBehaviour
{
    public static Action RefreshButtons;
    [SerializeField] protected GameObject _contents;
    [SerializeField] protected Transform _scrollerButtonHolder;
    [SerializeField] protected ShopItemButton _shopButtonPrefab;
        

    protected virtual void Start()
    {
        _contents.SetActive(false);

        //foreach (ShopType item in System.Enum.GetValues(typeof(ShopType)))
        //{
        //    ShopItemButton a = Instantiate(_shopButtonPrefab, _scrollerButtonHolder);
        //    a.Init(item, null);
        //}
    }

    public void Toggle()
    {
        bool a = !_contents.activeSelf;

        _contents.SetActive(a);
        if (a) RefreshShopButtons();
        
        EventSystem.current.SetSelectedGameObject(null);
    }

    protected abstract void RefreshShopButtons();

    public void Close()
    {
        _contents.SetActive(false);
    }

    //private void BuyItem(ShopType type)
    //{
    //    RefreshButtons?.Invoke();
    //}
}
