using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SmartCopy 
    {
        struct SerializedPropertyData
        {
            public SerializedPropertyType propertyType;
            //public GlobalObjectId globalObjectId;
            public object boxedValue;
        }
        private static Dictionary<string, SerializedPropertyData> clipboard = new();
        
        [MenuItem("CONTEXT/Component/Copy Component Values By Name")]
        private static void CopyComponentValuesByName(MenuCommand command)
        {
            // The component that was right-clicked
            var comp = (Component)command.context;
            
            clipboard.Clear();
            
            var iterator = new SerializedObject(comp).GetIterator();
            while (iterator.NextVisible(true))
            {
                if (iterator.name == "m_Script") continue;
                var entry = new SerializedPropertyData
                {
                    propertyType = iterator.propertyType
                };
                /*if (iterator.propertyType == SerializedPropertyType.ObjectReference)
                {
                    entry.globalObjectId = GlobalObjectId.GetGlobalObjectIdSlow(iterator.objectReferenceValue);
                }
                else
                {*/
                    entry.boxedValue = iterator.boxedValue;
                //}
                clipboard.Add(iterator.propertyPath, entry);
            }
        }
        
        [MenuItem("CONTEXT/Component/Paste Component Values By Name")]
        private static void PasteValuesByName(MenuCommand command)
        {
            var clickedComp = (Component)command.context;
            var targetSO = new SerializedObject(clickedComp);
            foreach (var kvp in clipboard)
            {
                var path = kvp.Key;
                var data = kvp.Value;
                var targetProp = targetSO.FindProperty(path);
                if (targetProp == null || targetProp.propertyType != data.propertyType)
                {
                    continue;
                }
                /*if (targetProp.propertyType == SerializedPropertyType.ObjectReference)
                {
                    targetProp.objectReferenceValue = GlobalObjectId.GlobalObjectIdentifierToObjectSlow(data.globalObjectId);
                }
                else
                {*/
                    targetProp.boxedValue = data.boxedValue;
                //}
            }
            targetSO.ApplyModifiedProperties();
        }
        
        [MenuItem("CONTEXT/Component/Paste Component Values By Name", true)]
        private static bool ValidatePasteValuesByName(MenuCommand command)
        {
            return clipboard != null && clipboard.Count > 0;
        }
    }
