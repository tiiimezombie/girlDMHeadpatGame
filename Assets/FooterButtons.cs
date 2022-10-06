using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FooterButtons : MonoBehaviour
{
    public TextMeshProUGUI XPText
    {
        get => _xpText;
    }

    public TextMeshProUGUI MoneyText
    {
        get => _moneyText;
    }

    [SerializeField] private TextMeshProUGUI _xpText;
    [SerializeField] private TextMeshProUGUI _moneyText;
    [SerializeField] private GameObject _bonusChestButtonGO;


    void Update()
    {
        
    }
}
