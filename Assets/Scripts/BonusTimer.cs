using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusTimer : MonoBehaviour
{
    [SerializeField] private GameObject _bonusButtonGO;
    private StaticTimer _bonusChestTimer;

    void Start()
    {
        _bonusButtonGO.SetActive(false);

        var a = new StaticTimerData()
        {
            TimerType = TimerType.BonusChest,
            RefreshType = TimerRefreshType.NeedToClaim,
            InitialDuration = 60 * 5,
        };
        _bonusChestTimer = new StaticTimer(a, GiveBonus);
    }

    void Update()
    {
        if (!GameController.IsPlaying) return;

        _bonusChestTimer.Increment(Time.deltaTime);
    }

    private void GiveBonus()
    {
        Debug.Log("dskfljsdklfdssdf");
        // TODO: balance
        CurrencyController.Instance.AddXP(1);
    }
}
