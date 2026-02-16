# SmartCopy - Copy Serialized Values Between Different Object Types
SmartCopy is a Unity Editor plugin that lets you copy and paste serialized field values between any Unity objects by intelligently matching property path and type.
It works across any built-in components, MonoBehaviours, and ScriptableObjects — even if they are unrelated classes — as long as their serialized fields match.

#### What is copied?

SmartCopy copies and pastes all modifiable serializable fields, including:

- Public fields
- Private fields marked with [SerializeField]
- Fields marked with [HideInInspector]
- Nested serialized data
  
If the property path and type match, the value is transferred automatically. Properties that don't match are left unaltered.

The plugin preserves serialized data of an object in memory — even if the source or target object is destroyed — allowing you to safely paste values later without losing data. 
This would be useful when copying values from objects in play mode.

View demo at: https://github.com/nithishakumar/UnitySmartCopy






