using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class SmartCopy
    {
        private static SerializedObject clipboardSO;
        
        [MenuItem("CONTEXT/Component/Copy Component Values By Name")]
        private static void CopyComponentValuesByName(MenuCommand command)
        {
            // The component that was right-clicked
            Component comp = (Component)command.context;
            
            // Clone the object and store serialized data in editor memory
            clipboardSO = new SerializedObject(Object.Instantiate(comp));
        }
        
        [MenuItem("CONTEXT/Component/Paste Component Values By Name")]
        private static void PasteValuesByName(MenuCommand command)
        {
            var clickedComp = (Component)command.context;
            var clickedSO = new SerializedObject(clickedComp);
            var enterChildren = true;
            var clickedProp = clickedSO.GetIterator();

            while (clickedProp.NextVisible(enterChildren))
            {
                enterChildren = false;
                
                // Try to find a matching property in the CLIPBOARD by name
                var clipboardProp = clipboardSO.FindProperty(clickedProp.name);
                
                // TODO: note down properties that were copied and not copied and give a summary to the user
                
                // Check if it exists AND the types match (e.g., don't paste a string into a float)
                if (clipboardProp != null && clipboardProp.propertyType == clickedProp.propertyType)
                {
                    clickedSO.CopyFromSerializedProperty(clipboardProp);
                }
                
            }
            
            clickedSO.ApplyModifiedProperties();
            Debug.Log($"Smart Pasted values into {clickedComp.GetType().Name} on {clickedComp.gameObject.name}");
        }
        
        [MenuItem("CONTEXT/Component/Paste Component Values By Name", true)]
        private static bool ValidatePasteValuesByName(MenuCommand command)
        {
            return clipboardSO != null;
        }
    }
}
