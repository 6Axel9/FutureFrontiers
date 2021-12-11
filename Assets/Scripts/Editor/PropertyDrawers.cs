using UnityEditor;

[CustomPropertyDrawer(typeof(GamePhasesDictionary))]
[CustomPropertyDrawer(typeof(SpawnableDictionary))]
public class CustomDictionaryPropertyDrawer : SerializableDictionaryPropertyDrawer { }

//[CustomPropertyDrawer(typeof(CustomDictionaryStorage))]
public class CustomDictionaryStoragePropertyDrawer : SerializableDictionaryStoragePropertyDrawer { }