using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using Unity.Plastic.Newtonsoft.Json;
using Unity.Plastic.Newtonsoft.Json.Linq;

public class SmartCopy
{
    private static readonly HashSet<string> PUBLIC_PROPERTIES_TO_IGNORE = new() { "m_Script" };

    private static JObject _sourceJsonObject;
    private static HashSet<string> _validPublicProperties = new();

    private static Dictionary<string, SerializedPropertyType> _expectedPropertyTypes = new();
    private static Dictionary<string, int> _objectReferenceValues = new();

    [MenuItem("CONTEXT/Component/Copy Serialized Values")]
    private static void CopySerializedValues(MenuCommand command)
    {
        _objectReferenceValues.Clear();
        _expectedPropertyTypes.Clear();

        var sourceObject = command.context;
        var serializedSourceObject = new SerializedObject(sourceObject);
        _validPublicProperties = GetValidPublicProperties(serializedSourceObject);
        var sourceJson = EditorJsonUtility.ToJson(sourceObject);
        _sourceJsonObject = JObject.Parse(sourceJson);
        foreach (var property in _validPublicProperties)
        {
            var serializedProp = serializedSourceObject.FindProperty(property);
            if (serializedProp == null)
            {
                continue;
            }

            _expectedPropertyTypes.Add(property, serializedProp.propertyType);
            if (serializedProp.propertyType == SerializedPropertyType.ObjectReference)
            {
                _objectReferenceValues.Add(property, serializedProp.objectReferenceInstanceIDValue);
            }
        }
    }

    [MenuItem("CONTEXT/Component/Paste Serialized Values")]
    private static void PasteValuesByPropertyPath(MenuCommand command)
    {
        var target = command.context;
        var targetJson = EditorJsonUtility.ToJson(target);
        var targetJsonObj = JObject.Parse(targetJson);
        var targetJsonObjValue = targetJsonObj.Properties().First().Value;
        var sourceJsonObjectValue = _sourceJsonObject.Properties().First().Value;
        var targetObj = new SerializedObject(target);
        foreach (var property in _validPublicProperties)
        {
            var serializedProp = targetObj.FindProperty(property);
            if (serializedProp == null || serializedProp.propertyType == SerializedPropertyType.ObjectReference ||
                serializedProp.propertyType != _expectedPropertyTypes[property])
            {
                continue;
            }

            targetJsonObjValue[property] = sourceJsonObjectValue[property];
        }

        EditorJsonUtility.FromJsonOverwrite(targetJsonObj.ToString(Formatting.None), target);
        targetObj = new SerializedObject(target);
        foreach (var kvp in _objectReferenceValues)
        {
            var serializedProp = targetObj.FindProperty(kvp.Key);
            if (serializedProp != null && serializedProp.propertyType == SerializedPropertyType.ObjectReference)
            {
                serializedProp.objectReferenceInstanceIDValue = kvp.Value;
            }
        }

        targetObj.ApplyModifiedProperties();
    }

    [MenuItem("CONTEXT/Component/Paste Serialized Values", true)]
    private static bool ValidatePasteValuesByName(MenuCommand command)
    {
        return _sourceJsonObject?.Properties().Any() == true;
    }

    // https://discussions.unity.com/t/serializedproperty-nextvisible-doesnt-work-with-hideininspector/801420/2
    private delegate FieldInfo FieldInfoGetter(SerializedProperty p, out Type t);

    private static HashSet<string> GetValidPublicProperties(SerializedObject serializedObject)
    {
        HashSet<string> temp = new();
#if UNITY_2019_3_OR_NEWER
        var fieldInfoGetterMethod = typeof(Editor).Assembly.GetType("UnityEditor.ScriptAttributeUtility")
            .GetMethod("GetFieldInfoAndStaticTypeFromProperty",
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
#else
    var fieldInfoGetterMethod =
 typeof( Editor ).Assembly.GetType( "UnityEditor.ScriptAttributeUtility" ) 
.GetMethod( "GetFieldInfoFromProperty", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static );
#endif
        var fieldInfoGetter = (FieldInfoGetter)Delegate.CreateDelegate(typeof(FieldInfoGetter), fieldInfoGetterMethod);

        var propertyAll = serializedObject.GetIterator();
        var propertyVisible = serializedObject.GetIterator();
        if (propertyAll.Next(true))
        {
            bool iteratingVisible = propertyVisible.NextVisible(true);
            do
            {
                bool isVisible = iteratingVisible && SerializedProperty.EqualContents(propertyAll, propertyVisible);
                if (isVisible)
                    iteratingVisible =
                        propertyVisible
                            .NextVisible(false); // Change it to true if you want to enter child properties, as well
                else
                {
                    // Internal Unity variables don't seem to have a FieldInfo but when SerializedProperty.type is "Array", we must consider it
                    // visible to avoid false negatives because even though "Array" type doesn't have a FieldInfo, it can be a visible array property
                    isVisible = propertyAll.type == "Array" ||
                                fieldInfoGetter(propertyAll, out Type propFieldType) != null;
                }

                if (isVisible && !PUBLIC_PROPERTIES_TO_IGNORE.Contains(propertyAll.name))
                {
                    temp.Add(propertyAll.name);
                }
            } while (propertyAll.Next(false)); // Change it to true if you want to enter child properties, as well
        }

        return temp;
    }
}