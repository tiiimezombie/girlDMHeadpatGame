using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using DG.Tweening;

public class ClickTracker : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _comboText;

    private double ComboCount
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
    private double _comboCount = 0;

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
                HeadpatController.Instance.FulfillHeadpats(finalCombo);
            }
        }
    }

    public void ClickDM()
    {
        //Debug.Log("pat");
        double increase = 0;

        if (Random.Range(0, 20) < 1) // 5%
        {
            increase = HeadpatController.Instance.HeadPatRemainder(ComboCount, CurrencyController.Instance.ShopLibrary.ShopDictionary[ShopType.ClickValue].Tier + (int)System.Math.Pow(CurrencyController.Instance.ShopLibrary.ShopDictionary[ShopType.ClickCritValue].Tier, 2));
            Debug.Log("CRIT");
        }
        else
        {
            increase = HeadpatController.Instance.HeadPatRemainder(ComboCount, CurrencyController.Instance.ShopLibrary.ShopDictionary[ShopType.ClickValue].Tier);
        }

        if (increase == 0) return;
        ComboCount += increase;

        _comboTween.Kill();
        _comboText.transform.localScale = Vector3.one;
        _comboTween = _comboText.transform.DOPunchScale(new Vector3(1.4f, 1.4f, 1), 0.3f, 1);
    }
}
