using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// AnySerializableDictionaryPropertyDrawer



public enum ColorType
{
    ChatBarNormal,
    ChatBarNeedy,
    RedeemNormal,
    RedeemNeedy,
    AnnouncementNormal,
    AnnouncementNeedy
}

[System.Serializable]
public class ThemeColorDictionary : SerializableDictionary<ColorType, Color> { }



//[System.Serializable]
//public class ChatUsernameDictionary : SerializableDictionary<RedeemType, Redeem> { }



public class GeneralDataScriptableObject : ScriptableObject
{
    public ThemeColorDictionary ThemeColorDictionary = new ThemeColorDictionary();

    //public RedeemDictionary dfsd;
    
    //public string GetRedeemText(RedeemType type)
    //{
    //    return RedeemDictionary[type].Name + " <sprite name=\"channelPoint\"> " + RedeemDictionary[type].Cost;
    //}





    //[System.Serializable]
    //public class ArrayTest
    //{
    //    public int[] myArray;
    //}

    //[System.Serializable]
    //public class ClassTest
    //{
    //    public string id;
    //    public float test;
    //    public string test2;
    //    public Quaternion quat;
    //}

    //[SerializeField, ID("id")]
    //private S_GenericDictionary _stringGeneric;

    //[System.Serializable]
    //public class S_GenericDictionary : SerializableDictionaryBase<string, ClassTest> { }
}

//[System.Serializable]
//public class TestThing
//{
//    public string id;
//    public int Number;
//    public SerializedRedeemDictionary iosduf;
//}

//[System.Serializable]
//public class SerializedRedeemDictionary : SerializableDictionaryBase<RedeemType, Redeem> { };