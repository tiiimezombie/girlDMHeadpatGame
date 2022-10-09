using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChatEntry : BaseEntry
{
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private TextMeshProUGUI _subIcon;
    [SerializeField] private TextMeshProUGUI _message;

    public void Setup(string username, string message, string subIcon = "", bool needsInteraction = false, Action<bool> interaction = null)
    {
        _name.text = username;
        _subIcon.text = string.IsNullOrWhiteSpace(subIcon) ? string.Empty : "<sprite name=\"" + subIcon + "\">";
        _message.text = message;

        base.Setup(needsInteraction, interaction);
    }
}
