using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StreamTimer : MonoBehaviour
{
    private TextMeshProUGUI _text;

    private DateTime _startTime;
    TimeSpan _uptime;
    WaitForSeconds _oneSecondDelay = new WaitForSeconds(1);
    bool _updating;


    void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();

        ResetTime();
        StartCoroutine(IncrementSeconds());
    }

    public void ResetTime()
    {
        _startTime = DateTime.Now;
    }

    IEnumerator IncrementSeconds()
    {
        _updating = true;
        while (_updating)
        {
            _uptime = DateTime.Now - _startTime;
            _text.text = _uptime.ToString(@"hh\:mm\:ss");
            yield return _oneSecondDelay;
        }
        _updating = false;
    }
}
