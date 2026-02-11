using UnityEditor;

public class SmartCopy
{
    private static SerializedDataStore _serializedDataStore;

    [MenuItem("CONTEXT/Component/Copy Serialized Values")]
    [MenuItem("CONTEXT/ScriptableObject/Copy Serialized Values")]
    private static void CopySerializedValues(MenuCommand command)
    {
        _serializedDataStore = null;
        var sourceObject = command.context;
        var serializedSourceObject = new SerializedObject(sourceObject);
        var publicProperties = ValidPublicProperties.GetValidPublicProperties(serializedSourceObject);
        _serializedDataStore = new SerializedDataStore(serializedSourceObject, publicProperties);
    }

    [MenuItem("CONTEXT/Component/Paste Matching Serialized Values")]
    [MenuItem("CONTEXT/ScriptableObject/Paste Matching Serialized Values")]
    private static void PasteMatchingSerializedValues(MenuCommand command)
    {
        var target = command.context;
        _serializedDataStore.CopyFromSerializedDataStore(target);
    }

    [MenuItem("CONTEXT/Component/Paste Matching Serialized Values", true)]
    [MenuItem("CONTEXT/ScriptableObject/Paste Matching Serialized Values", true)]
    private static bool ValidatePasteValuesByName(MenuCommand command)
    {
        return _serializedDataStore != null;
    }
}