using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class PasteSerializedValues
    {
        [MenuItem("CONTEXT/Component/Test Menu Item")]
        private static void PasteSerializedValueFunction(MenuCommand command)
        {
            // The component that was right-clicked
            Component comp = (Component)command.context;

            // Example action: log the component type and GameObject
            Debug.Log($"Clicked menu on {comp.GetType().Name} attached to {comp.gameObject.name}");
        }
    }
}
