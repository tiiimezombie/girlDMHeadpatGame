using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerController : MonoBehaviour
{
    [SerializeField] private TimerPanel[] _timerArray;
    private bool _upgradeMode;




    public void ToggleUpgradeMode()
    {
        _upgradeMode = !_upgradeMode;
        foreach (var v in _timerArray)
        {
            v.SetView(_upgradeMode ? 2 : 1);
        }
    }
}

public enum TimerRunType
{
    AutoRerun,
    ManualRun,
    RerunAfterCollect,
}

public class TimedItem
{
    public string Name;
    public bool Enabled;
    public TimerRunType RunType;
    public long PurchaseCost;
    public long 
}