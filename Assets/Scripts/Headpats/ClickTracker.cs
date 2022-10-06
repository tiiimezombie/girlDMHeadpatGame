using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using DG.Tweening;

public class ClickTracker : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _comboText;

    private long ComboCount
    {
        get => _comboCount;
        set
        {
            if (value > _comboCount)
            {
                _comboActive = true;
                _comboTimer = _comboTimeout;
            }

            _comboCount = value;
            _comboText.text = _comboCount <= 0 ? string.Empty : _comboCount.ToString("N0");
        }
    }
    private long _comboCount = 0;

    private long _bonusClicks;

    private float _comboTimeout = 2;
    private float _comboTimer;
    private bool _comboActive;
    private Tween _comboTween;

    protected void Start()
    {
        ComboCount = 0;
    }

    private void Update()
    {
        if (_comboActive)
        {
            _comboTimer -= Time.deltaTime;
            if (_comboTimer <= 0)
            {
                _comboActive = false;
                var finalCombo = ComboCount;
                ComboCount = 0;
                CurrencyController.Instance.FulfillHeadpats(finalCombo);
            }
        }
    }

    public void ClickDM()
    {
        //Debug.Log("pat");
        long increase = 0;

        if (Random.Range(0, 20) < 1) // 5%
        {
            _bonusClicks += 5;
            //increase = HeadpatController.Instance.HeadPatRemainder(ComboCount, CurrencyController.Instance.ShopLibrary.ShopDictionary[ShopType.ClickValue].Tier + (int)System.Math.Pow(CurrencyController.Instance.ShopLibrary.ShopDictionary[ShopType.ClickCritValue].Tier, 2));
            Debug.Log("CRIT");
        }
        //else
        //{
        //    //increase = HeadpatController.Instance.HeadPatRemainder(ComboCount, CurrencyController.Instance.ShopLibrary.ShopDictionary[ShopType.ClickValue].Tier);
        //}

        increase = CurrencyController.Instance.HeadPatRemainder(ComboCount, 1 + _bonusClicks);
        _bonusClicks = 0;

        if (increase == 0) return;
        ComboCount += increase;

        _comboTween.Kill();
        _comboText.transform.localScale = Vector3.one * (ComboCount/100 + 1);
        _comboTween = _comboText.transform.DOPunchScale(new Vector3(1.4f, 1.4f, 1), 0.3f, 1);
    }
}
