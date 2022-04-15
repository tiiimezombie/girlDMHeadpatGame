using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MilestonePopup : MonoBehaviour
{
    [SerializeField] private MilestoneButton[] _milestoneButtons;

    private CanvasGroup _canvasGroup;

    private List<MilestoneRewardCollection> milestoneCollectionList = new List<MilestoneRewardCollection>();

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _canvasGroup.alpha = 0;
        _canvasGroup.blocksRaycasts = false;
    }

    public void Open()
    {
        _canvasGroup.DOFade(1, 0.2f).OnComplete(()=> { _canvasGroup.blocksRaycasts = true; });
        Setup();
    }

    private void Setup()
    {
        _milestoneButtons[0].Setup(milestoneCollectionList[0].Reward1);
        _milestoneButtons[1].Setup(milestoneCollectionList[0].Reward2);
        _milestoneButtons[2].Setup(milestoneCollectionList[0].Reward3);
    }

    public void AddMilestoneContents(MilestoneRewardCollection collection)
    {
        milestoneCollectionList.Add(collection);
    }

    public void SelectedMilestoneReward()
    {
        milestoneCollectionList.RemoveAt(0);

        if (milestoneCollectionList.Count > 0) Setup();
        else Close();
    }

    public void Close()
    {
        _canvasGroup.blocksRaycasts = false;
        _canvasGroup.DOFade(0, 0.2f);
    }
}

