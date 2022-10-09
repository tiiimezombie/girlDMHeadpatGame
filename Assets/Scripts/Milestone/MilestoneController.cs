using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MilestoneController : MonoBehaviour
{
    [SerializeField] private Announcement _announcement;
    private int _milestoneIndex = 0;

    private int[] _milestoneCostArray = new int[]
    {
        1, 10, 15, 20, 25,

    };

    void Start()
    {
        _milestoneIndex = 0;
    }

    public void CheckForMilestone(long amountRedeemed)
    {
        if (_milestoneIndex >= _milestoneCostArray.Length) return;

        for (int i = _milestoneIndex; i < _milestoneCostArray.Length; i++)
        {
            if (_milestoneIndex >= _milestoneCostArray.Length || amountRedeemed < _milestoneCostArray[i])
            {
                _milestoneIndex = i;
                break;
            }

            DoMilestone(i);
        }
    }

    private void DoMilestone(int index)
    {
        Debug.Log("Milestone " + index + " reached!");
        switch (index)
        {
            case 0:
                CurrencyController.Instance.AddMoney(1);
                ShopTimerController.Instance.PanelSpeedUpgradeRequested(UpgradeableTimerType.Headpat);
                _announcement.AddMessage("Milestone Reached!", "Faster Headpat Rate");
                break;
            case 1:
                //AudienceController.Instance.AddViewersAsReward(2);
                ShopTimerController.Instance.Unlock(UpgradeableTimerType.Phrase);
                break;
            case 2:
                
                break;
            case 3:
                Debug.Log("weasa");
                break;

            default:
                Debug.Log("zoinks - nothing happened");
                break;
        }

        // Plans

        // faster headpats
        // first way to get xp (can unlock other methods now or later)
        // first way to get money
        // first way to get follwers
        // min size for raid
        // min size for hype train
        // 
    }
}