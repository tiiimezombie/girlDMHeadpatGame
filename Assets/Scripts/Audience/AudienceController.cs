using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using DG.Tweening;

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
    private int[] _audienceCompositionPercentArray_current;

    private readonly int[] _audienceCompositionPercentArray_standard =
    {
        15, 70, 15,
    };

    private readonly int[] _audienceCompositionPercentArray_postRaid =
    {
        10, 55, 35,
    };

    private readonly int[] _audienceCompositionPercentArray_postSubs =
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
    private double _realViewerCount;

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
    private Timer _audienceChangeMode;
    private bool _audienceChanged;

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

    private enum ViewerSourceType
    {
        Normal,
        Raid,
        Reward
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

        _audienceChangeMode = new Timer(() => { _audienceCompositionPercentArray_current = _audienceCompositionPercentArray_standard; }, 0);

        _realViewerCount = 25;
        ViewerCount = _realViewerCount;
        _audienceCompositionPercentArray_current = _audienceCompositionPercentArray_standard;

        _playing = true;
    }

    private void TimeUp(TimerType type)
    {
        var viewer = (AudienceMemberType)GameController.GetWeightedRandomFromArray(_audienceCompositionPercentArray_current);

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
                AddViewers(UnityEngine.Random.Range(-5, 20), ViewerSourceType.Normal);
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
                        AddSubs(subs, false);
                        break;
                    case RandomEventType.Raid:
                        int newViewers = 50 * GameController.GetWeightedRandomFromArray(_generosityPercentArray);
                        AddViewers(newViewers, ViewerSourceType.Raid);
                        break;
                    case RandomEventType.Bits:
                        int bits = GameController.GetWeightedRandomFromArray(_generosityPercentArray);
                        CurrencyController.Instance.AddMoney(bits);
                        _announcement.AddMessage(_dataObject.GetAudienceUsername(AudienceMemberType.Good), "threw coins x" + (bits * 100));
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

        if (_audienceChanged) _audienceChangeMode.Increment();
    }

    internal void AddSubs(int subs, bool asReward)
    {
        _audienceChangeMode.ChangeTimerMax(subs * 10);
        _audienceCompositionPercentArray_current = _audienceCompositionPercentArray_postSubs;
        _audienceChanged = true;

        if (asReward)
            _announcement.AddMessage("girl_dm_", "is gifting " + subs + " sub(s)!");
        else
            _announcement.AddMessage(_dataObject.GetAudienceUsername(AudienceMemberType.Good), "is gifting " + subs + " sub(s)!");
    }

    internal void AddViewersAsReward(int viewers)
    {
        AddViewers(viewers, ViewerSourceType.Reward);
    }

    Coroutine _viewerAddCoroutine;
    bool _counting;
    private void AddViewers(int viewers, ViewerSourceType source)
    {
        _audienceChangeMode.ChangeTimerMax(viewers / 5);
        _audienceCompositionPercentArray_current = _audienceCompositionPercentArray_postRaid;
        _audienceChanged = true;

        if (source == ViewerSourceType.Raid)
        {
            _announcement.AddMessage(_dataObject.GetMutualUsername(), "raided!!!");
        } 
        else if (source == ViewerSourceType.Reward)
        {
            _announcement.AddMessage("girl_dm_", "invited her friends");
        }

        if (_viewerAddCoroutine != null) StopCoroutine(_viewerAddCoroutine);
        _realViewerCount += viewers;
        if (_realViewerCount < 0) _realViewerCount = 0;

        _viewerAddCoroutine = StartCoroutine(SlowlyAddViewers());
    }

    WaitForSeconds boo = new WaitForSeconds(0.5f);

    IEnumerator SlowlyAddViewers()
    {
        var a = _realViewerCount - _viewerCount;
        a /= 10;
        a = Math.Round(a);
        var remainder = (_realViewerCount - _viewerCount) - (a * 10);

        _viewerCount += remainder;

        for (int i = 0; i < 10; i++)
        {
            ViewerCount += a;
            yield return boo;
        }
    }
}
