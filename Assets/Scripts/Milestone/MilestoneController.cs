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
        3, 10, 15, 20, 25,

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
        Debug.Log(index);
        switch (index)
        {
            case 1:
                CurrencyController.Instance.AddXP(1);
                _announcement.AddMessage("Milestone Reached!", "+1 xp");
                break;
            case 2:
                break;
            case 3:
                break;

            default:
                Debug.Log("zoinks");
                break;
        }
    }
}