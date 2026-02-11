using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;

public static class ValidPublicProperties
{
  private static readonly HashSet<string> PUBLIC_PROPERTIES_TO_IGNORE = new() { "m_Script" };

  // https://discussions.unity.com/t/serializedproperty-nextvisible-doesnt-work-with-hideininspector/801420/2
  private delegate FieldInfo FieldInfoGetter(SerializedProperty p, out Type t);

  public static HashSet<string> GetValidPublicProperties(SerializedObject serializedObject)
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
            propertyVisible.NextVisible(false); // Change it to true if you want to enter child properties, as well
        else
        {
          // Internal Unity variables don't seem to have a FieldInfo but when SerializedProperty.type is "Array", we must consider it
          // visible to avoid false negatives because even though "Array" type doesn't have a FieldInfo, it can be a visible array property
          isVisible = propertyAll.type == "Array" || fieldInfoGetter(propertyAll, out Type propFieldType) != null;
        }

        if (isVisible && !PUBLIC_PROPERTIES_TO_IGNORE.Contains(propertyAll.propertyPath))
        {
          temp.Add(propertyAll.propertyPath);
        }
      } while (propertyAll.Next(false)); // Change it to true if you want to enter child properties, as well
    }

    return temp;
  }
}
