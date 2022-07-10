using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(StringStringDictionary))]
[CustomPropertyDrawer(typeof(ObjectColorDictionary))]
[CustomPropertyDrawer(typeof(StringColorArrayDictionary))]

[CustomPropertyDrawer(typeof(RedeemDictionary))]
[CustomPropertyDrawer(typeof(ChatDictionary))]

[CustomPropertyDrawer(typeof(AudienceRedeemPreferenceDictionary))]
[CustomPropertyDrawer(typeof(AudienceChatPreferenceDictionary))]

[CustomPropertyDrawer(typeof(ShopDictionary))]
[CustomPropertyDrawer(typeof(MilestoneDictionary))]

[CustomPropertyDrawer(typeof(ThemeColorDictionary))]

public class AnySerializableDictionaryPropertyDrawer : SerializableDictionaryPropertyDrawer {}

[CustomPropertyDrawer(typeof(AudiencePreferenceStorage))]
[CustomPropertyDrawer(typeof(StringArrayStorage))]

[CustomPropertyDrawer(typeof(ColorArrayStorage))]
public class AnySerializableDictionaryStoragePropertyDrawer: SerializableDictionaryStoragePropertyDrawer {}
