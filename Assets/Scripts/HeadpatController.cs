using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class HeadpatController : Singleton<HeadpatController>
{
    [SerializeField] private TextMeshProUGUI _patsQueuedText;
    [SerializeField] private TextMeshProUGUI _patsRedeemedText;

    private MilestoneController _milestoneController;

    public double PatsRedeemed
    {
        get => _patsRedeemed;
        set
        {
            _patsRedeemed = value;
            _patsRedeemedText.text = GameController.GetPrettyDouble(_patsRedeemed); // _patsRedeemed.ToString("N0");
        }
    }
    private double _patsRedeemed = 0;

    public double PatsQueued
    {
        get => _patsQueued;
        private set
        {
            _patsQueued = value;
            _patsQueuedText.text = GameController.GetPrettyDouble(_patsQueued); //_patsQueued.ToString("N0");
        }
    }
    private double _patsQueued;

    protected override void Awake()
    {
        base.Awake();
        _milestoneController = GetComponent<MilestoneController>();
    }

    private void Start()
    {
        PatsRedeemed = 0;
        PatsQueued = 0;
    }

    public void AddHeadpats(int number)
    {
        PatsQueued += number;
    }

    public double HeadPatRemainder(double combo, int numHeadPatsToCheck)
    {
        if (combo + numHeadPatsToCheck <= PatsQueued) return numHeadPatsToCheck;
        return PatsQueued - combo;
    }

    public void FulfillHeadpats(double number)
    {
        PatsQueued -= number;

        DOTween.To(() => PatsRedeemed, x => PatsRedeemed = x, PatsRedeemed + number, 1).OnComplete(()=> { _milestoneController.CheckForMilestone(PatsRedeemed); });
    }
}
