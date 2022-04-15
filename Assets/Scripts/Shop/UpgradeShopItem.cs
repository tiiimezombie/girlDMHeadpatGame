using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class ShopItem
{
    public string Name;
    public int Level;
    public int Requirement;
    public int Cost;
}

public class UpgradeShopItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private TextMeshProUGUI _value;
    private RedeemShop _shop;

    public void Init()
    {
        
    }

    public void OnClick()
    {
        //_shop.Buy
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
