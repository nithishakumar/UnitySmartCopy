using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace SmartCopy
{
    /// <summary>
    /// Stores serialized data of an object in memory and transfers it between Unity objects.
    /// </summary>
    public class SerializedDataStore
    {
        /// Maps property paths to their respective data stored as SerializedPropertyData objects
        private Dictionary<string, SerializedPropertyData> store = new();

        struct SerializedPropertyData
        {
            public SerializedPropertyType type;
            public object data;
        }

        /// <summary>
        /// Creates a new <see cref="SerializedDataStore"/> object by copying values from a specified <see cref="SerializedObject"/>
        /// for the property paths listed in <paramref name="propertyPathsToStore"/>.
        /// </summary>
        /// <param name="serializedSourceObject">The <see cref="SerializedObject"/> to copy data from.</param>
        /// <param name="propertyPathsToStore">The set of property paths whose values should be stored.</param>
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

        /// <summary>
        /// Copies stored serialized data to the specified <see cref="UnityEngine.Object"/>.
        /// Iterates through the internal <see cref="store"/> and sets the value of each
        /// serialized property on the target given the property paths and types match.
        /// </summary>
        /// <param name="target">The <see cref="UnityEngine.Object"/> to apply the stored data to.</param>
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

        /// <summary>
        /// Gets the value of a <see cref="SerializedProperty"/> and stores it in a
        /// <see cref="SerializedPropertyData"/> object.
        /// Handles generic properties, object references, managed references, and standard types.
        /// </summary>
        /// <param name="property">The <see cref="SerializedProperty"/> to get the value from.</param>
        /// <returns>A <see cref="SerializedPropertyData"/> object containing the property's type and value.</returns>
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

        /// <summary>
        /// Recursively extracts data from a generic <see cref="SerializedProperty"/>.
        /// </summary>
        /// <param name="property">The generic <see cref="SerializedProperty"/> to extract data from.</param>
        /// <returns>
        /// A dictionary mapping property names to their data as <see cref="SerializedPropertyData"/> objects.
        /// </returns>
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

        /// <summary>
        /// Sets a <paramref name="targetProperty"/>'s value from the <paramref name="serializedProperty"/>
        /// given their property types match.
        /// </summary>
        private void SetSerializedPropertyValue(SerializedProperty targetProperty,
            SerializedPropertyData serializedProperty)
        {
            if (targetProperty == null)
            {
                return;
            }

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
                CopyObjectReference(targetProperty, (Object)serializedProperty.data);
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

        /// <summary>
        /// Sets a <paramref name="targetProperty"/>'s array value from the <paramref name="serializedProperty"/>
        /// given that serializedProperty stores valid array data.
        /// </summary>
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

        /// <summary>
        /// Sets a <paramref name="targetProperty"/>'s generic value from the <paramref name="serializedProperty"/>
        /// given serializedProperty stores data of a generic serialized proeprty.
        /// </summary>
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

        /// <summary>
        /// Returns the scene path of a GameObject or Component.
        /// </summary>
        /// <param name="objectToCopy">The object to check. Can be a GameObject or Component.</param>
        /// <returns>
        /// The path of the scene the object belongs to, or an empty string if the object is not a game object or component.
        /// </returns>
        private string GetScenePathOfGameObjectOrComponent(Object objectToCopy)
        {
            if (objectToCopy is GameObject go)
            {
                return go.scene.path;
            }
            
            if (objectToCopy is Component component)
            {
                return component.gameObject.scene.path;
            }
            
            return string.Empty;
        }

        /// <summary>
        /// Copies a Unity object reference to a serialized property, ensuring scene-bound references are valid.
        /// Scene-bound references are only copied if both source and target are in the same scene.
        /// Null references are assigned directly.
        /// </summary>
        /// <param name="targetProperty">The <see cref="SerializedProperty"/> to assign to.</param>
        /// <param name="objectReference">The Unity <see cref="Object"/> to copy (GameObject, Component, or asset).</param>
        private void CopyObjectReference(SerializedProperty targetProperty, Object objectReference)
        {
            if (targetProperty == null)
            {
                return;
            }
            
            if (objectReference == null)
            {
                targetProperty.objectReferenceValue = null;
                return;
            }
            
            var isObjToCopyPersistent = EditorUtility.IsPersistent(objectReference);
            var isTargetPersistent = EditorUtility.IsPersistent(targetProperty.serializedObject.targetObject);
            
            if (!isTargetPersistent && !isObjToCopyPersistent)
            {
               var areInSameScene = GetScenePathOfGameObjectOrComponent(targetProperty.serializedObject.targetObject) == GetScenePathOfGameObjectOrComponent(objectReference);
               if (!areInSameScene)
               {
                   targetProperty.objectReferenceValue = null;
                   return;
               }
            }

            else if (!isObjToCopyPersistent && isTargetPersistent) {
                targetProperty.objectReferenceValue = null;
                return;
            }
            
            targetProperty.objectReferenceValue = objectReference;
        }
    }
}
