using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseFeed : MonoBehaviour
{
    [SerializeField] protected VerticalLayoutGroup _layoutGroup;

    protected int _index;
    protected bool _showingAllMessages;
    private RectTransform _rt;

    protected virtual void Awake()
    {
        _rt = transform as RectTransform;
    }

    protected IEnumerator RebuildLate()
    {
        yield return null;
        yield return null;
        LayoutRebuilder.ForceRebuildLayoutImmediate(_rt);
    }
}
