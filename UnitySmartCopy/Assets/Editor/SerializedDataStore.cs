using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class SerializedDataStore
{
    private Dictionary<string, SerializedPropertyData> store = new();

    struct SerializedPropertyData
    {
        public SerializedPropertyType type;
        public object data;
    }

    public SerializedDataStore(SerializedObject serializedSourceObject, HashSet<string> propertyPathsToStore)
    {
        foreach (var property in propertyPathsToStore)
        {
            var serializedProp = serializedSourceObject.FindProperty(property);
            if (serializedProp == null)
            {
                continue;
            }

            store.Add(property, GetValueFromSerializedProperty(serializedProp));
        }
    }

    public void CopyFromSerializedDataStore(Object target)
    {
        var targetObject = new SerializedObject(target);
        foreach (var kvp in store)
        {
            var targetPropertyPath = kvp.Key;
            var clipboard = kvp.Value;
            var targetProperty = targetObject.FindProperty(targetPropertyPath);
            if (targetProperty == null)
            {
                continue;
            }

            SetSerializedPropertyValue(targetProperty, clipboard);
        }

        targetObject.ApplyModifiedProperties();
    }

    private SerializedPropertyData GetValueFromSerializedProperty(SerializedProperty property)
    {
        var result = new SerializedPropertyData() { type = property.propertyType, };
        switch (property.propertyType)
        {
            case SerializedPropertyType.Generic:
                result.data = GetValueFromGenericSerializedProperty(property);
                break;

            case SerializedPropertyType.ManagedReference:
                result.data = property.managedReferenceValue;
                break;

            case SerializedPropertyType.ObjectReference:
                result.data = property.objectReferenceValue;
                break;
            default:
                result.data = property.boxedValue;
                break;
        }

        return result;
    }

    private Dictionary<string, SerializedPropertyData> GetValueFromGenericSerializedProperty(
        SerializedProperty property)
    {
        var result = new Dictionary<string, SerializedPropertyData>();
        var iterator = property.Copy();
        var rootPath = property.propertyPath;
        iterator.Next(true);
        while (iterator.propertyPath.StartsWith(rootPath))
        {
            var key = iterator.propertyPath.Split('.').Last();
            result.Add(key, GetValueFromSerializedProperty(iterator));

            // move to next sibling
            iterator.Next(false);
        }

        return result;
    }

    private void SetSerializedPropertyValue(SerializedProperty targetProperty,
        SerializedPropertyData serializedProperty)
    {
        if (targetProperty.isArray && targetProperty.propertyType == SerializedPropertyType.Generic)
        {
            SetArraySerializedPropertyValue(targetProperty, serializedProperty);
            return;
        }

        if (serializedProperty.type == SerializedPropertyType.Generic)
        {
            SetGenericSerializedPropertyValue(targetProperty, serializedProperty);
            return;
        }

        if (targetProperty.propertyType != serializedProperty.type)
        {
            return;
        }

        if (serializedProperty.type == SerializedPropertyType.ObjectReference)
        {
            targetProperty.objectReferenceValue = (Object)serializedProperty.data;
        }
        else if (serializedProperty.type == SerializedPropertyType.ManagedReference)
        {
            targetProperty.managedReferenceValue = serializedProperty.data;
        }
        else
        {
            targetProperty.boxedValue = serializedProperty.data;
        }
    }

    private void SetArraySerializedPropertyValue(SerializedProperty targetProperty,
        SerializedPropertyData serializedProperty)
    {
        var genericValues = (Dictionary<string, SerializedPropertyData>)serializedProperty.data;
        var arrayObject = (Dictionary<string, SerializedPropertyData>)genericValues["Array"].data;
        var arraySize = arrayObject["size"].data;
        targetProperty.arraySize = (int)arraySize;
        foreach (var kvp in arrayObject)
        {
            var key = kvp.Key;
            if (!key.StartsWith(("data[")))
            {
                continue;
            }

            var start = key.IndexOf('[');
            var end = key.IndexOf(']');
            var indexString = key.Substring(start + 1, end - start - 1);
            if (!int.TryParse(indexString, out var index))
            {
                continue;
            }

            var elementProp = targetProperty.GetArrayElementAtIndex(index);
            SetSerializedPropertyValue(elementProp, kvp.Value);
        }
    }

    private void SetGenericSerializedPropertyValue(SerializedProperty targetProperty,
        SerializedPropertyData serializedProperty)
    {
        var genericValues = (Dictionary<string, SerializedPropertyData>)serializedProperty.data;
        foreach (var kvp in genericValues)
        {
            var relativeProperty = targetProperty.FindPropertyRelative(kvp.Key);
            SetSerializedPropertyValue(relativeProperty, kvp.Value);
        }
    }
}
