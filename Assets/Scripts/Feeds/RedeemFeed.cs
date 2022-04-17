using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class RedeemFeed : BaseFeed
{
    [SerializeField] private AudienceScriptableObject _dataObject;
    [SerializeField] private RedeemEntry[] _entryArray;

    private void Start()
    {
        foreach (var v in _entryArray)
        {
            v.gameObject.SetActive(false);
        }

        AudienceController.DemandedRedeem += SelectRedeem;
    }

    protected void OnDestroy()
    {
        AudienceController.DemandedRedeem -= SelectRedeem;
    }

    private void SelectRedeem(AudienceMemberType viewer)
    {
        var redeemType = _dataObject.ChooseRedeemTypeByAudience(viewer);
        var redeem = _dataObject.GetRedeem(redeemType);

        if (redeemType == RedeemType.Headpat)
        {
            HeadpatController.Instance.AddHeadpats(CurrencyController.Instance.ShopLibrary.ShopDictionary[ShopType.HeadpatRedeemTier].Tier);
        }

        AddEntry(_dataObject.GetAudienceUsername(viewer), redeem);
    }

    public void AddEntry(string username, Redeem redeem, bool isHeadpat = false)
    {
        if (!_showingAllMessages)
        {
            _entryArray[_index].gameObject.SetActive(true);
            if (_index == _entryArray.Length) _showingAllMessages = true;
        }

        _entryArray[_index].transform.SetAsLastSibling();
        if (isHeadpat)
        {
            int tier = CurrencyController.Instance.ShopLibrary.ShopDictionary[ShopType.HeadpatRedeemTier].Tier;
            _entryArray[_index].Setup(username + " redeemed " + redeem.Name + " x " + tier + " <sprite name=\"channelPoint\"> " + (redeem.Cost * tier));
        }
        else
        {
            _entryArray[_index].Setup(username + " redeemed " + redeem.Name + " <sprite name=\"channelPoint\"> " + redeem.Cost);
        }

        _index++;
        _index %= _entryArray.Length;

        StartCoroutine(RebuildLate());
    }
}
