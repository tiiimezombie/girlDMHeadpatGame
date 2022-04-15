using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseEntry : MonoBehaviour
{
    [SerializeField] private Image _bar;
    [SerializeField] private Color _normalBarColor;
    [SerializeField] private Color _needyBarColor;

    protected RectTransform _rt;
    protected Button _button;

    protected bool _needsInteraction;
    protected event Action<bool> _interaction;

    protected void Awake()
    {
        _rt = transform as RectTransform;
        _button = GetComponent<Button>();
    }

    protected virtual void Setup(bool needsInteraction = false, Action<bool> interaction = null)
    {
        if (needsInteraction)
        {
            _interaction?.Invoke(false);
        }

        _interaction = interaction;
        SetInteraction(needsInteraction);

        LayoutRebuilder.ForceRebuildLayoutImmediate(_rt);
    }

    public void Click()
    {
        _needsInteraction = false;
        _interaction?.Invoke(true);

        SetInteraction(false);
    }

    protected virtual void SetInteraction(bool needs)
    {
        _needsInteraction = needs;
        _button.interactable = needs;
        _bar.color = _needsInteraction ? _needyBarColor : _normalBarColor;
    }
}
