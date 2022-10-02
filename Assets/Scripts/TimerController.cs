using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerController : MonoBehaviour
{
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
