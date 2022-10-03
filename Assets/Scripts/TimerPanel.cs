using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimerPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private GameObject _viewOneHolderGO;
    [SerializeField] private Image _progressBar;
    [SerializeField] private Button _claimButton;
    [SerializeField] private TextMeshProUGUI _claimButtonText;
    [SerializeField] private GameObject _viewTwoHolderGO;
    [SerializeField] private Button _xpUpgradeButton;
    [SerializeField] private TextMeshProUGUI _xpUpgradeButtonText;
    [SerializeField] private Button _moneyUpgradeButton;
    [SerializeField] private TextMeshProUGUI _moneyUpgradeButtonText;

    public void Setup(string name)
    {
        _viewOneHolderGO.SetActive(true);
        _viewTwoHolderGO.SetActive(false);
        _titleText.text = name;
    }

    public void DoUpdate()
    {

    }

    public void SetView(int viewIndex)
    {
        _viewOneHolderGO.SetActive(false);
        _viewTwoHolderGO.SetActive(false);

        if (viewIndex == 2)
        {
            _viewTwoHolderGO.SetActive(true);
        }
        else
        {
            _viewOneHolderGO.SetActive(true);
        }
    }
}