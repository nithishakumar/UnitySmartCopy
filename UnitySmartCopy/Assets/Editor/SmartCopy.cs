using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class SmartCopy
    {
        private static SerializedObject sourceSnapshot;
        
        [MenuItem("CONTEXT/Component/Copy Component Values By Name")]
        private static void CopyComponentValuesByName(MenuCommand command)
        {
            // The component that was right-clicked
            Component comp = (Component)command.context;

            // Example action: log the component type and GameObject
            Debug.Log($"Clicked menu on {comp.GetType().Name} attached to {comp.gameObject.name}");
            
            sourceSnapshot = new SerializedObject(comp);
        }
        
        [MenuItem("CONTEXT/Component/Paste Component Values By Name")]
        private static void PasteValuesByName(MenuCommand command)
        {
            // The component that was right-clicked
            Component comp = (Component)command.context;

            // Example action: log the component type and GameObject
            Debug.Log($"Clicked menu on {comp.GetType().Name} attached to {comp.gameObject.name}");
        }
        
        [MenuItem("CONTEXT/Component/Paste Component Values By Name", true)]
        private static bool ValidatePasteValuesByName(MenuCommand command)
        {
            return sourceSnapshot != null;
        }
    }
}
