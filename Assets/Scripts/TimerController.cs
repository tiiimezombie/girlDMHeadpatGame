using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerController : MonoBehaviour
{
    [SerializeField] private BaseTimer[] _timers;
    [SerializeField] private TimerPanel[] _timerArray;
    private bool _upgradeMode;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleUpgradeMode()
    {
        _upgradeMode = !_upgradeMode;
        foreach (var v in _timerArray)
        {
            v.SetView(_upgradeMode ? 2 : 1);
        }
    }
}

public class Timer
{
    public string Name;
    
    public float Duration;
    public float CurrentTime;

    public int InitialMultiplierTier = 1;
    public int InitialMultiplierUpgradeCost;

    public int InitialSpeedTier = 1;
    public int dsfsd;
}