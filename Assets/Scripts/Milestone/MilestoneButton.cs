using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MilestoneButton : MonoBehaviour
{
    [SerializeField] private MilestonePopup _parent;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _rewardText;
    private event Action _clickAction;

    public void Setup(MilestoneReward reward)
    {
        _nameText.text = reward.Name;
        _rewardText.text = reward.Icon;
        _clickAction = reward.Action;
    }

    public void OnClick()
    {
        _clickAction?.Invoke();
        _parent.SelectedMilestoneReward();
    }
}
