using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using DG.Tweening;
using Random = UnityEngine.Random;

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

    [SerializeField] private AudienceScriptableObject _dataObject;
    [SerializeField] private ChatFeed _chatFeed;
    [SerializeField] private RedeemFeed _redeemFeed;
    [SerializeField] private Announcement _announcement;
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

    public long HeadPatsFromAudience
    {
        get
        {
            return 1 + ViewerCount / 10; // TODO: + random() range, probably
        }
    }

    public long ViewerCount
    {
        get => _viewerCount;
        set
        {
            _viewerCount = value;
            if (_viewerCount < 0) _viewerCount = 0;
            _viewerCountText.text = "<sprite name=\"viewerIcon\"> " + _viewerCount.ToString("N0");
        }
    }
    private long _viewerCount;
    private long _realViewerCount;

    private StaticTimerData _chatMessage;
    private StaticTimerData _gift;
    private StaticTimerData _raid;

    //private double _subCount;

    //private double _headPatterCount;
    //private double _chatterCount;
    //private double _redeemerCount;
    //private double _donatorCount;
    //private double _gifterCount;
    //private double _lurkerCount;

    //private List<Timer> _timerList;
    private bool _audienceChanged;

    //private enum TimerType
    //{
    //    Headpat,
    //    Chat,
    //    Redemption,
    //    Donation,
    //    SubGift,
    //    SubMonthEnd,

    //    ViewerCount,
    //    Partnerships,
    //    HypeTrain,
    //    RandomEvent
    //}

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

        // Audience controller has a list of events that happen regularly
        // When they occur, they change values, notify various UI systems, and set the timer
        // TODO some way to track which timer needs to happen faster when the shop item is bought :/
        //_timerDictionary.Add(TimerType.Headpat, new Timer(() => { HeadPatTimeUp(); }, 8));
        // _timerDictionary.Add(TimerType.Chat, new Timer(() => { ChatTimeUp(); }, 8));
        //_timerDictionary.Add(TimerType.Redemption, new Timer(() => { RedemptionTimeUp(); }, 24));
        //_timerDictionary.Add(TimerType.Donation, new Timer(() => { DonationTimeUp(); }, 30));
        //_timerDictionary.Add(TimerType.SubGift, new Timer(() => { SubGiftTimeUp(); }, 45));
        //_timerDictionary.Add(TimerType.SubMonthEnd, new Timer(() => { SubMonthEndTimeUp(); }, 120));

        //_timerDictionary.Add(TimerType.ViewerCount, new Timer(() => { ViewerCountTimeUp(); }, 10));
        //_timerDictionary.Add(TimerType.Partnerships, new Timer(() => { PartnershipTimeUp(); }, 60));
        //_timerDictionary.Add(TimerType.HypeTrain, new Timer(() => { HypeTrainTimeUp(); }, 90));
        // random event?

        //_audienceChangeMode = new Timer(() => { _audienceCompositionPercentArray_current = _audienceCompositionPercentArray_standard; }, 0);

        _realViewerCount = 5;
        ViewerCount = _realViewerCount;
        _audienceCompositionPercentArray_current = _audienceCompositionPercentArray_standard;

        // 
        //_chatMessage = new SpecialTimer()
        //{
        //    TimerType = TimerType.Chat,
        //    RefreshType = TimerRefreshType.NeedToClaim,
        //    CurrentDuration = 60 * 5,
        //};
        //_chatMessage.Setup(GiveBonus);
    }

    #region -- TimeUp --

    private void DoChat()
    {
        // it's either flat rate of x% for xp generation
        // or based on recent raid/ % audience composition

        // chat percent array

        var viewer = (AudienceMemberType)GameController.GetWeightedRandomFromArray(_audienceCompositionPercentArray_current);
        var chat = _dataObject.ChooseChatTypeByAudience(viewer);
        _chatFeed.AddEntry(chat, _dataObject.GetAudienceUsername(viewer), _dataObject.GetChat(chat));
    }

    public void IncreaseViewerCap(long value)
    {
        Debug.Log("add viewer cap " + value);
    }

    public void AddFavor(long value)
    {
        Debug.Log("favor " + value);
    }

    //private void HeadPatTimeUp()
    //{
    //    HeadpatController.Instance.AddHeadpats(CurrencyController.Instance.ShopLibrary.ShopDictionary[ShopType.HeadpatValue].Tier);
    //    _redeemFeed.AddEntry("Headpat fan 100", _dataObject.GetRedeem(RedeemType.Headpat), true);
    //}



    private void RedemptionTimeUp()
    {
        // Redeem percent array

        var viewer = (AudienceMemberType)GameController.GetWeightedRandomFromArray(_audienceCompositionPercentArray_current);

        var redeemType = _dataObject.ChooseRedeemTypeByAudience(viewer);
        _redeemFeed.AddEntry(_dataObject.GetAudienceUsername(viewer), _dataObject.GetRedeem(redeemType));
    }

    //private void DonationTimeUp()
    //{
    //    if (_realViewerCount < 64) return;

    //    // normal bits, 1.5x bits, 2x bits, $1
    //    int[] aa = { 15, 5, 1 };
    //    int[] multiplierArray = { 1, 2, 5 };

    //    int generosityIndex = GameController.GetWeightedRandomFromArray(aa);
    //    int multiplier = multiplierArray[generosityIndex];

    //    int donation = CurrencyController.Instance.ShopLibrary.ShopDictionary[ShopType.DonationValue].Tier * multiplier;

    //    // or set generosity
    //    if (Random.Range(0, 10) < 3)
    //    {
    //        // dollars
    //        CurrencyController.Instance.AddMoney(donation);
    //        _announcement.AddMessage(_dataObject.GetAudienceUsername(AudienceMemberType.Good), "donated $" + donation);
    //    }
    //    else
    //    {
    //        // bits
    //        CurrencyController.Instance.AddMoney(donation);
    //        _announcement.AddMessage(_dataObject.GetAudienceUsername(AudienceMemberType.Good), "threw coins x" + (donation * 100));
    //    }
    //}

    //private void SubGiftTimeUp()
    //{
    //    if (_realViewerCount < 32) return;

    //    int subs =
    //        CurrencyController.Instance.ShopLibrary.ShopDictionary[ShopType.DonationValue].Tier *
    //        GameController.GetWeightedRandomFromArray(_generosityPercentArray);

    //    if (CurrencyController.Instance.ShopLibrary.MilestoneDictionary[ShopType.SubDiscount].Tier > 0)
    //        subs *= CurrencyController.Instance.ShopLibrary.MilestoneDictionary[ShopType.SubDiscount].Tier;

    //    _subCount += subs;

    //    AddSubs(subs, false);
    //}

    //private void SubMonthEndTimeUp()
    //{
    //    _announcement.AddMessage("Sub Renewal", GameController.GetPrettyLong(_subCount) + " sub(s)");

    //    CurrencyController.Instance.AddMoney(_subCount);
    //}

    //private void ViewerCountTimeUp()
    //{
    //    if (Random.Range(0, 10) < 3 && CurrencyController.Instance.ShopLibrary.MilestoneDictionary[ShopType.RaidValue].Tier > 0)
    //    {
    //        int newViewers =
    //            GameController.GetWeightedRandomFromArray(_generosityPercentArray) *
    //            CurrencyController.Instance.ShopLibrary.MilestoneDictionary[ShopType.RaidValue].Tier *
    //            25;
    //        if (newViewers > 0) AddViewers(newViewers, ViewerSourceType.Raid);
    //    }
    //    else
    //    {
    //        // var viewer = (AudienceMemberType)GameController.GetWeightedRandomFromArray(_audienceCompositionPercentArray_current);
    //        //AddViewers(
    //        //    (int) Math.Pow(Random.Range(-5, 10), CurrencyController.Instance.ShopLibrary.MilestoneDictionary[ShopType.Socials].Tier),
    //        //    ViewerSourceType.Normal);
    //    }
    //}

    private void PartnershipTimeUp()
    {
        //CurrencyController.Instance.AddMoney(CurrencyController.Instance.ShopLibrary.MilestoneDictionary[ShopType.Partnerships].Tier);
    }

    private void HypeTrainTimeUp()
    {
        if (_realViewerCount < 128) return;

        Debug.LogError("SET HYPE TRAIN UP BETTER");
        //CurrencyController.Instance.AddMoney(CurrencyController.Instance.ShopLibrary.MilestoneDictionary[ShopType.Partnerships].Tier);
    }

    #endregion

    void Update()
    {
        if (!_playing) return;

        //foreach (var v in _timerDictionary.Values)
        //{
        //    v.Increment();
        //}

        //if (_audienceChanged) _audienceChangeMode.Increment();
    }

    internal void AddSubs(int subs, bool asReward)
    {
        //_audienceChangeMode.ChangeTimerMax(subs * 10);
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

    //internal void SetTimerMax(ShopType type)
    //{
    //    switch (type)
    //    {
    //        case ShopType.HeadpatDelay:
    //            _timerDictionary[TimerType.Headpat].DecreaseTimerMax(1);
    //            break;
    //        case ShopType.DonationDelay:
    //            _timerDictionary[TimerType.Donation].DecreaseTimerMax(1);
    //            break;
    //        case ShopType.SubDelay:
    //            _timerDictionary[TimerType.SubGift].DecreaseTimerMax(1);
    //            break;
    //        case ShopType.HypeTrainDelay:
    //            _timerDictionary[TimerType.HypeTrain].DecreaseTimerMax(2);
    //            break;
    //    }
    //}

    Coroutine _viewerAddCoroutine;
    bool _counting;
    private void AddViewers(int viewers, ViewerSourceType source)
    {
        //_audienceChangeMode.ChangeTimerMax(viewers / 5);
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
        //a = Math.Round(a);
        var remainder = (_realViewerCount - _viewerCount) - (a * 10);

        _viewerCount += remainder;

        for (int i = 0; i < 10; i++)
        {
            ViewerCount += a;
            yield return boo;
        }
    }
}
