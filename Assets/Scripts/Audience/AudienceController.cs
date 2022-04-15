using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public enum AudienceMemberType
{
    Good,
    Neutral,
    Evil
}

public class AudienceController : Singleton<AudienceController>
{
    public static event Action<AudienceMemberType> DemandedRedeem;
    public static event Action<AudienceMemberType> DemandedChat;

    [SerializeField] private Announcement _announcement;

    [SerializeField] private AudienceScriptableObject _dataObject;

    [SerializeField] private TextMeshProUGUI _viewerCountText;

    private bool _playing;

    // good neutral evil
    private int[] _audienceCompositionPercentArray_standard =
    {
        15, 70, 15,
    };

    private int[] _audienceCompositionPercentArray_postSubs =
    {
        35, 60, 5,
    };

    private int[] _generosityPercentArray =
    {
        0, 8, 4, 2,
    };

    public double ViewerCount
    {
        get => _viewerCount;
        set
        {
            _viewerCount = value;
            if (_viewerCount < 0) _viewerCount = 0;
            _viewerCountText.text = "<sprite name=\"viewerIcon\"> " + _viewerCount.ToString("N0");
        }
    }
    private double _viewerCount;

    // Timers
    private class Timer
    {
        private event Action TimedAction;
        private float TimerMax = 5;
        private float _timer;

        public Timer(Action action, int max)
        {
            TimedAction = action;
            TimerMax = max;
        }

        public void Increment()
        {
            _timer += Time.deltaTime;

            if (_timer >= TimerMax)
            {
                TimedAction?.Invoke();
                _timer = 0;
            }
        }

        public void ChangeTimerMax (int max)
        {
            TimerMax = max;
            _timer = 0;
        }
    }

    private List<Timer> _timerList;
    private Timer _giftSubModeTimer;
    private bool _giftSubMode;

    private enum TimerType
    {
        Redeem,
        Chat,
        ViewerCount,
        RandomEvent
    }

    private enum RandomEventType
    {
        Donation,
        GiftSubs,
        Raid,
        Bits
    }

    private void Start()
    {
        _dataObject.Init();

        _timerList = new List<Timer>
        {
            new Timer(()=>{ TimeUp(TimerType.Redeem); }, 5),
            new Timer(()=>{ TimeUp(TimerType.Chat); }, 7),
            new Timer(()=>{ TimeUp(TimerType.ViewerCount); }, 10),
            new Timer(()=>{ TimeUp(TimerType.RandomEvent); }, 20),
        };

        _giftSubModeTimer = new Timer(() => { _giftSubMode = false; }, 0);

        ViewerCount = 25;

        _playing = true;
    }

    private void TimeUp(TimerType type)
    {
        var viewer = (AudienceMemberType)GameController.GetWeightedRandomFromArray(_audienceCompositionPercentArray_standard);

        // based on chat comp
        // so x number of neutral chat + y evil chat + z sub chat
        // pick from percents
        // Probably just off/on of "Sub Mode" where temporarily chat improves?

        switch (type)
        {
            case TimerType.Chat:
                DemandedChat?.Invoke(viewer);
                break;
            case TimerType.Redeem:
                DemandedRedeem?.Invoke(viewer);
                break;
            case TimerType.ViewerCount:
                ViewerCount += UnityEngine.Random.Range(-5, 20);
                break;
            case TimerType.RandomEvent:
                var a = (RandomEventType)UnityEngine.Random.Range(0, 4); // TODO: does she have a bits announcement
                switch (a)
                {
                    case RandomEventType.Donation:
                        int donation = 5 * GameController.GetWeightedRandomFromArray(_generosityPercentArray);
                        CurrencyController.Instance.AddMoney(donation);
                        _announcement.AddMessage(_dataObject.GetAudienceUsername(AudienceMemberType.Good), "donated $" + donation);
                        break;
                    case RandomEventType.GiftSubs:
                        int subs = GameController.GetWeightedRandomFromArray(_generosityPercentArray);

                        _giftSubMode = true;
                        _giftSubModeTimer.ChangeTimerMax(subs * 10);

                        _announcement.AddMessage(_dataObject.GetAudienceUsername(AudienceMemberType.Good), "is gifting " + subs + " sub(s)!");
                        break;
                    case RandomEventType.Raid:
                        int newViewers = 50 * GameController.GetWeightedRandomFromArray(_generosityPercentArray);
                        ViewerCount += newViewers;
                        _announcement.AddMessage(_dataObject.GetMutualUsername(), "raided!!");
                        break;
                    case RandomEventType.Bits:
                        int bits = GameController.GetWeightedRandomFromArray(_generosityPercentArray);
                        CurrencyController.Instance.AddMoney(bits);
                        _announcement.AddMessage(_dataObject.GetAudienceUsername(AudienceMemberType.Good), "cheered x" + (bits * 100));
                        break;
                }

                break;
        }
    }



    void Update()
    {
        if (!_playing) return;

        for (int i = 0; i < _timerList.Count; i++)
        {
            _timerList[i].Increment();
        }

        if (_giftSubMode) _giftSubModeTimer.Increment();
    }

    internal void AddSubsAsReward(int v)
    {
        throw new NotImplementedException();
    }

    internal void AddViewersAsReward(int v)
    {
        throw new NotImplementedException();
    }
}
