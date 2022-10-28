using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MilestoneController : MonoBehaviour
{
    [SerializeField] private Announcement _announcement;
    [SerializeField] private RedeemShop _redeemShop;
    [SerializeField] private ClickTracker _clickTracker;

    void Start()
    {
        _headPatMilestoneIndex = 0;
        _viewerMilestoneIndex = 0;
    }

    #region -- Headpats --

    private int _headPatMilestoneIndex = 0;
    private int[] _headpatMilestoneCostArray = new int[]
    {
        3, 10, 25, 50, 100, 150
    };

    public void CheckForHeadpatMilestone()
    {
        if (_headPatMilestoneIndex >= _headpatMilestoneCostArray.Length) return;

        while (_headPatMilestoneIndex < _headpatMilestoneCostArray.Length && CurrencyController.Instance.HeadpatsRedeemed >= _headpatMilestoneCostArray[_headPatMilestoneIndex])
        {
            DoHeadpatMilestone();
            _headPatMilestoneIndex++;
        }
    }

    private void DoHeadpatMilestone()
    {
        Debug.Log("Pat Milestone " + _headPatMilestoneIndex + " reached!");

        switch (_headPatMilestoneIndex)
        {
            case 0:
                //CurrencyController.Instance.AddMoney(1);
                ShopTimerController.Instance.PanelSpeedUpgradeRequested(UpgradeableTimerType.Headpat, false);
                _announcement.AddMessage("Milestone Reached!", "Faster Headpat Rate");
                break;
            case 1:
                //AudienceController.Instance.AddViewersAsReward(2);
                ShopTimerController.Instance.EnableTimer(UpgradeableTimerType.Phrase);
                _announcement.AddMessage("Headpat Milestone!", "Redeem Granted");
                break;
            case 2:
                _clickTracker.UnlockCritPats();
                _announcement.AddMessage("Headpat Milestone!", "Critical Headpats");
                break;
            case 3:
                ShopTimerController.Instance.EnableTimer(UpgradeableTimerType.Emote);
                _announcement.AddMessage("Headpat Milestone!", "Redeem Unlocked");
                break;
            case 4:
                ShopTimerController.Instance.EnableTimer(UpgradeableTimerType.Accessory);
                _announcement.AddMessage("Headpat Milestone!", "Redeem Unlocked");
                break;
            case 5:
                ShopTimerController.Instance.EnableTimer(UpgradeableTimerType.GroupAidedPat);
                _announcement.AddMessage("Headpat Milestone!", "Crowd Aid");
                break;
            default:
                Debug.Log("zoinks - nothing happened");
                break;
        }
    }

    #endregion

    #region -- Viewer Count --

    private int _viewerMilestoneIndex = 0;
    private int[] _viewerMilestoneCostArray = new int[]
    {
        10, 25, 50, 75, 100,
        125, 150, 200, 250,
        300
    };

    public void CheckForViewerMilestone()
    {
        if (_viewerMilestoneIndex >= _viewerMilestoneCostArray.Length) return;

        for (int i = _viewerMilestoneIndex; i < _viewerMilestoneCostArray.Length; i++)
        {
            if (_viewerMilestoneIndex >= _viewerMilestoneCostArray.Length || AudienceController.Instance.ViewerCount < _viewerMilestoneCostArray[i])
            {
                _viewerMilestoneIndex = i;
                break;
            }

            DoViewerMilestone();
        }
    }

    private void DoViewerMilestone()
    {
        Debug.Log("Milestone " + _viewerMilestoneIndex + " reached!");
        switch (_viewerMilestoneIndex)
        {
            case 0:
                AudienceController.Instance.UnlockTimer(StaticTimerType.Chat);
                _announcement.AddMessage("Viewer Milestone!", "Chatty viewers");
                break;
            case 1:
                AudienceController.Instance.UnlockTimer(StaticTimerType.Gift);
                _announcement.AddMessage("Viewer Milestone!", "Generosity Enabled");
                break;
            case 2:
                AudienceController.Instance.UnlockTimer(StaticTimerType.Raid);
                _announcement.AddMessage("Viewer Milestone!", "Raids inbound!");
                break;
            case 3:
                ShopTimerController.Instance.EnableTimer(UpgradeableTimerType.AdPayout);
                _announcement.AddMessage("Viewer Milestone!", "Ads Paying Off");
                break;
            case 4:
                _redeemShop.UnlockTimer(UpgradeableTimerType.Merch);
                _announcement.AddMessage("Viewer Milestone!", "Merch Secured");
                break;
            case 5:
                _redeemShop.UnlockTimer(UpgradeableTimerType.YoutubeVideo);
                _announcement.AddMessage("Viewer Milestone!", "Youtube Unlocked");
                break;
            case 6:
                _redeemShop.UnlockTimer(UpgradeableTimerType.Ramen);
                _announcement.AddMessage("Viewer Milestone!", "Ramen Time");
                break;
            case 7:
                _redeemShop.UnlockTimer(UpgradeableTimerType.TwitterPost);
                _announcement.AddMessage("Viewer Milestone!", "Twitter Mastered");
                break;
            case 8:
                _redeemShop.UnlockTimer(UpgradeableTimerType.TiktokVideo);
                _announcement.AddMessage("Viewer Milestone!", "TikTok Hits");
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

    #endregion
}