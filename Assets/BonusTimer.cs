using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusTimer : MonoBehaviour
{
    [SerializeField] private GameObject _bonusButtonGO;
    private SpecialTimer _bonusChestTimer;

    void Start()
    {
        _bonusButtonGO.SetActive(false);

        _bonusChestTimer = new SpecialTimer()
        {
            TimerType = TimerType.BonusChest,
            RefreshType = TimerRefreshType.NeedToClaim,
            CurrentDuration = 60 * 5,
        };
        _bonusChestTimer.Setup(GiveBonus);
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
