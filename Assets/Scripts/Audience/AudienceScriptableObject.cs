using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class AudienceScriptableObject : ScriptableObject
{
#if UNITY_EDITOR
    [MenuItem("Assets/Create/TZ/Audience")]
    public static AudienceScriptableObject Create()
    {
        AudienceScriptableObject asset = ScriptableObject.CreateInstance<AudienceScriptableObject>();

        AssetDatabase.CreateAsset(asset, "Assets/AudienceScriptableObject.asset");
        AssetDatabase.SaveAssets();
        return asset;
    }
#endif

    [SerializeField] private RedeemDictionary RedeemDictionary = new RedeemDictionary();
    [SerializeField] private AudienceDemographic GoodAudience = new AudienceDemographic();
    [SerializeField] private AudienceDemographic NeutralAudience = new AudienceDemographic();
    [SerializeField] private AudienceDemographic EvilAudience = new AudienceDemographic();
    [SerializeField] private ChatDictionary ChatDictionary = new ChatDictionary();
    [SerializeField] private string[] MutualNames;

    //[SerializeField] private AudienceRedeemPreferenceDictionary RedeemPreferenceDictionary = new AudienceRedeemPreferenceDictionary();
    //[SerializeField] private AudienceChatPreferenceDictionary ChatPreferenceDictionary = new AudienceChatPreferenceDictionary();
    //[SerializeField] private AudienceUsernameDictionary AudienceUsernameDictionary = new AudienceUsernameDictionary();

    private Dictionary<AudienceMemberType, int[]> _chatPreferenceDictionary = new Dictionary<AudienceMemberType, int[]>();
    private Dictionary<AudienceMemberType, int[]> _redeemPreferenceDictionary = new Dictionary<AudienceMemberType, int[]>();

    internal void Init()
    {
        //foreach(var v in RedeemDictionary.Values)
        //{
        //    v.Reset();
        //}

        _chatPreferenceDictionary.Add(AudienceMemberType.Good, GoodAudience.GetChatPrefProbabilityArray());
        _chatPreferenceDictionary.Add(AudienceMemberType.Neutral, NeutralAudience.GetChatPrefProbabilityArray());
        _chatPreferenceDictionary.Add(AudienceMemberType.Evil, EvilAudience.GetChatPrefProbabilityArray());

        _redeemPreferenceDictionary.Add(AudienceMemberType.Good, GoodAudience.GetRedeemPrefProbabilityArray());
        _redeemPreferenceDictionary.Add(AudienceMemberType.Neutral, GoodAudience.GetRedeemPrefProbabilityArray());
        _redeemPreferenceDictionary.Add(AudienceMemberType.Evil, GoodAudience.GetRedeemPrefProbabilityArray());
    }

    #region -- Redeems --
    internal RedeemType ChooseRedeemTypeByAudience(AudienceMemberType viewer)
    {
        return (RedeemType)GameController.GetWeightedRandomFromArray(_redeemPreferenceDictionary[viewer]);

        //switch (viewer)
        //{
        //    case AudienceMemberType.Good:



        //        return GoodAudience.RedeemPrefs[].Type;
        //    case AudienceMemberType.Evil:
        //        return EvilAudience.RedeemPrefs[(RedeemType)GameController.GetItemFromArray(_redeemPreferenceDictionary[viewer])];
        //    default:
        //        return NeutralAudience.RedeemPrefs[(RedeemType)GameController.GetItemFromArray(_redeemPreferenceDictionary[viewer])];
        //}
    }

    internal Redeem GetRedeem(RedeemType redeemType)
    {
        try
        {
            return RedeemDictionary[redeemType];
        }
        catch
        {
            Debug.LogError(redeemType + " not in dictionary wtf");
            return null;
        }
    }

    #endregion

    #region -- Chat --

    internal ChatType ChooseChatTypeByAudience(AudienceMemberType viewer)
    {
        return (ChatType)GameController.GetItemFromArray(_chatPreferenceDictionary[viewer]);
        //switch (viewer)
        //{
        //    case AudienceMemberType.Good:
        //        return GoodAudience.ChatPrefs[].Type;
        //    case AudienceMemberType.Evil:
        //        return EvilAudience.ChatPrefs[GameController.GetItemFromArray(_chatPreferenceDictionary[viewer])].Type;
        //    default:
        //        return NeutralAudience.ChatPrefs[GameController.GetItemFromArray(_chatPreferenceDictionary[viewer])].Type;
        //}
    }    

    internal string GetChat(ChatType chat)
    {
        return GameController.GetItemFromArray(ChatDictionary[chat].data);
    }

    #endregion

    internal string GetAudienceUsername(AudienceMemberType viewer, bool addSubIcon = false)
    {
        switch (viewer)
        {
            case AudienceMemberType.Good:
                return GameController.GetItemFromArray(GoodAudience.Usernames);
            case AudienceMemberType.Evil:
                return GameController.GetItemFromArray(EvilAudience.Usernames);
            default:
                return GameController.GetItemFromArray(NeutralAudience.Usernames);
        }
    }

    internal string GetMutualUsername()
    {
        return GameController.GetItemFromArray(MutualNames);
    }
}

public enum RedeemType
{
    Headpat,
    Hydrate,
    Cheer,
    Outfit
}

public enum ChatType
{
    Chatter,
    Emote,
    Complaint
}

[System.Serializable]
public class AudienceDemographic
{
    public AudienceMemberType Type;
    public AudienceChatPreferenceDictionary ChatPrefs;
    public AudienceRedeemPreferenceDictionary RedeemPrefs;
    public string[] Usernames;

    public int[] GetChatPrefProbabilityArray()
    {
        int[] b = new int[System.Enum.GetValues(typeof(ChatType)).Length];
        foreach (var v in ChatPrefs)
        {
            b[(int)v.Key] = v.Value;
        }

        return b;
    }

    public int[] GetRedeemPrefProbabilityArray()
    {
        int[] b = new int[System.Enum.GetValues(typeof(RedeemType)).Length];
        foreach (var v in RedeemPrefs)
        {
            b[(int)v.Key] = v.Value;
        }

        return b;
    }
}


[System.Serializable]
public class Redeem
{
    public string Name;
    public int Cost;
    public bool NeedsInteraction;
}

[System.Serializable]
public class AudiencePreference
{
    public int Chance = 1;
    public AudienceMemberType Type;
}

[System.Serializable]
public class ChatPreference
{
    public int Chance = 1;
    public ChatType Type;
}

[System.Serializable]
public class RedeemPreference
{
    public int Chance = 1;
    public RedeemType Type;
}

// Serialized dictionary classes
[System.Serializable]
public class RedeemDictionary : SerializableDictionary<RedeemType, Redeem> { }

[System.Serializable]
public class ChatDictionary : SerializableDictionary<ChatType, StringArrayStorage> { }

[System.Serializable]
public class AudienceRedeemPreferenceDictionary : SerializableDictionary<RedeemType, int> { }

[System.Serializable]
public class AudienceChatPreferenceDictionary : SerializableDictionary<ChatType, int> { }

[System.Serializable]
public class AudienceUsernameDictionary : SerializableDictionary<AudienceMemberType, StringArrayStorage> { }


// Serialized dictionary value arrays
[System.Serializable]
public class AudiencePreferenceStorage : SerializableDictionary.Storage<AudiencePreference[]> { }

[System.Serializable]
public class StringArrayStorage : SerializableDictionary.Storage<string[]> { }