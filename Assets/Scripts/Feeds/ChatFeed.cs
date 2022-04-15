using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatFeed : BaseFeed
{
    [SerializeField] private AudienceScriptableObject _dataObject;
    [SerializeField] private ChatEntry[] _entryArray;

    private void Start()
    {
        foreach (var v in _entryArray)
        {
            v.gameObject.SetActive(false);
        }

        AudienceController.DemandedChat += SelectChat;
    }

    protected void OnDestroy()
    {
        AudienceController.DemandedChat -= SelectChat;
    }

    private void SelectChat(AudienceMemberType viewer)
    {
        var chat = _dataObject.ChooseChatTypeByAudience(viewer);
        AddEntry(_dataObject.GetAudienceUsername(viewer), _dataObject.GetChat(chat));
    }

    public void AddEntry (string username, string message)
    {
        if (!_showingAllMessages)
        {
            _entryArray[_index].gameObject.SetActive(true);
            if (_index == _entryArray.Length) _showingAllMessages = true;
        }

        _entryArray[_index].transform.SetAsLastSibling();
        _entryArray[_index].Setup(username, message);
        
        _index++;
        _index %= _entryArray.Length;

        StartCoroutine(RebuildLate());
    }
}