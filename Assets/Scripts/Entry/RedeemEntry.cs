using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RedeemEntry : BaseEntry
{
    [SerializeField] private TextMeshProUGUI _content;

    public void Setup(string contents, bool needsInteraction = false, Action<bool> interaction = null)
    {
        _content.text = contents;
        base.Setup(needsInteraction, interaction);
    }
}
