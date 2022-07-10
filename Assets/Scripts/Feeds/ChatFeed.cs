using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatFeed : BaseFeed
{
    [SerializeField] private ChatEntry[] _entryArray;

    private void Start()
    {
        foreach (var v in _entryArray)
        {
            v.gameObject.SetActive(false);
        }
    }

    public void AddEntry (ChatType type, string username, string message)
    {
        if (!_showingAllMessages)
        {
            _entryArray[_index].gameObject.SetActive(true);
            if (_index == _entryArray.Length) _showingAllMessages = true;
        }

        _entryArray[_index].transform.SetAsLastSibling();

        if (type == ChatType.Complaint)
        {
            _entryArray[_index].Setup(username, message, string.Empty, true, (state)=> { CurrencyController.Instance.AddXP(ShopType.ChatXPValue); });
        }
        else
        {
            _entryArray[_index].Setup(username, message);
        }        
        
        _index++;
        _index %= _entryArray.Length;

        StartCoroutine(RebuildLate());
    }
}