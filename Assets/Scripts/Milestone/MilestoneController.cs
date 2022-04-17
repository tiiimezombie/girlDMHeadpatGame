using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MilestoneController : MonoBehaviour
{
    public static int MilestoneCount;

    [SerializeField] private MilestonePopup _popup;
    [SerializeField] private GameObject _xpTextGO;
    [SerializeField] private GameObject _milestoneButtonGO;
    [SerializeField] private Button _shopButton;

    [SerializeField] private List<int> _baseMilestoneList;
    private List<int> _milestoneList;

    // we could move this to a scriptable object but idk just as easy to have here
    [SerializeField] private string[] _moneyRewardNames;
    [SerializeField] private string[] _subRewardNames;
    [SerializeField] private string[] _viewerRewardNames;

    private int[] chanceTime =
    {
        1, 1, 1
    };

    void Start()
    {
        _milestoneList = _baseMilestoneList;
        MilestoneCount = 0;

        _xpTextGO.SetActive(true);
        _milestoneButtonGO.SetActive(false);
    }

    public void CheckForMilestone(double amountRedeemed)
    {
        if (_milestoneList.Count <= 0) return;
        for (int i = 0; i < _milestoneList.Count; i++)
        {
            if (_milestoneList.Count <= 0 || amountRedeemed < _milestoneList[0]) break;
            
            AddMilestone();
            _milestoneList.RemoveAt(0);
        }
    }

    private void AddMilestone()
    {
        MilestoneCount++;
        _xpTextGO.SetActive(false);
        _milestoneButtonGO.SetActive(true);
        _shopButton.enabled = false;
        _popup.AddMilestoneContents(CreateMilestoneCollection());
    }

    private void RemoveMilestone()
    {
        MilestoneCount--;
        if (MilestoneCount <= 0)
        {
            _xpTextGO.SetActive(true);
            _milestoneButtonGO.SetActive(false);
            _shopButton.enabled = true;
        }
    }

    private MilestoneRewardCollection CreateMilestoneCollection()
    {
        return new MilestoneRewardCollection
        {
            Reward1 = CreateMilestone(),
            Reward2 = CreateMilestone(),
            Reward3 = CreateMilestone(),
        };
    }

    private MilestoneReward CreateMilestone()
    {
        int selectedIndex = GameController.GetWeightedRandomFromArray(chanceTime);
        for(int i = 0; i< chanceTime.Length; i++)
        {
            if (i == selectedIndex) chanceTime[i] = 0;
            else chanceTime[i]++;
        }

        int currentMilestone = MilestoneCount;

        switch ((MilestoneRewardType)selectedIndex)
        {
            case MilestoneRewardType.Money:
                return new MilestoneReward
                {
                    Name = _moneyRewardNames[Random.Range(0, _moneyRewardNames.Length)],
                    Icon = "+<sprite name=\"viewerIcon\">",
                    Action = () => { 
                        CurrencyController.Instance.AddMoney(currentMilestone);
                        RemoveMilestone();
                    }
                };
            case MilestoneRewardType.Subs:
                return new MilestoneReward
                {
                    Name = _moneyRewardNames[Random.Range(0, _moneyRewardNames.Length)],
                    Icon = "+<sprite name=\"viewerIcon\">",
                    Action = () => { 
                        AudienceController.Instance.AddSubs(currentMilestone * 2, true);
                        RemoveMilestone();
                    }
                };
            case MilestoneRewardType.Viewers:
                return new MilestoneReward
                {
                    Name = _moneyRewardNames[Random.Range(0, _moneyRewardNames.Length)],
                    Icon = "+<sprite name=\"viewerIcon\">",
                    Action = () => { 
                        AudienceController.Instance.AddViewersAsReward(currentMilestone * 10);
                        RemoveMilestone();
                    }
                };
        }
        return null;
    }
}

public enum MilestoneRewardType
{
    Money,
    Subs,
    Viewers,
}

public struct MilestoneRewardCollection
{
    public MilestoneReward Reward1;
    public MilestoneReward Reward2;
    public MilestoneReward Reward3;
}

[System.Serializable]
public class MilestoneReward
{
    public string Name;
    public string Icon;
    public System.Action Action;
}