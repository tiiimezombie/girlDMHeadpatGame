using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class Announcement : MonoBehaviour
{
    [SerializeField] private RectTransform _baseRT;
    [SerializeField] private TextMeshProUGUI _usernameText;
    [SerializeField] private TextMeshProUGUI _thingDidText;

    List<string[]> _messageList = new List<string[]>();

    bool running;
    bool _redeemed;

    void Start()
    {
        _baseRT.anchoredPosition = new Vector2(0, 300);

        //AddMessage("xxzczzx", "gifted 5 subs");
        //AddMessage("erwer", "gifted 5 subs");
        //AddMessage("sdfsdf", "gifted 5 subs");
    }

    public void AddMessage(string username, string thing)
    {
        _messageList.Add(new []{ username, thing });
        ShowMessages();
    }

    public void Click()
    {
        if (_redeemed) return;
        CurrencyController.Instance.AddXP(1);
        _redeemed = true;
    }

    private void ShowMessages()
    {
        if (running) return;

        StartCoroutine(ShowMessageRoutine());

        // if showing
    }

    Sequence seq;
    WaitForSeconds _pauseBetweenDuration = new WaitForSeconds(0.5f);
    WaitForSeconds _displayDuration = new WaitForSeconds(7f);

    private IEnumerator ShowMessageRoutine()
    {
        running = true;
        while (_messageList.Count > 0) {

            yield return _pauseBetweenDuration;

            _usernameText.text = _messageList[0][0];
            _thingDidText.text = _messageList[0][1];
            _redeemed = false;

            seq = DOTween.Sequence();
            seq.Append(_baseRT.DOAnchorPosY(0, 1f))
                .AppendInterval(5)
                .Append(_baseRT.DOAnchorPosY(300, 1f));

            seq.Play();

            yield return _displayDuration;

            _messageList.RemoveAt(0);
        }
        running = false;
    }
}
