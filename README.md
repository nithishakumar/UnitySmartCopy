# UnitySmartCopy
UnitySmartCopy is a Unity Editor plugin that lets you copy and paste serialized field values between any Unity objects by intelligently matching property path and type.
It works across any built-in components, MonoBehaviours, and ScriptableObjects — even if they are unrelated classes — as long as their serialized fields match.


### Copying Matching Fields from CharacterProfile.cs to CharacterSetup.cs

<p align="center">
<img src=https://github.com/user-attachments/assets/72a228bb-2ddb-454f-8fcf-a104eb87b903 width="400"/>
</p>

#### What is copied?

UnitySmartCopy copies and pastes all modifiable serializable fields, including:

- Public fields
- Private fields marked with [SerializeField]
- Fields marked with [HideInInspector]
- Nested serialized data

If the property path and type match, the value is transferred automatically.

| CharacterProfile.cs | CharacterSetup.cs |
|----------|----------|
| <img width="538" height="363" alt="Screenshot 2026-02-11 at 9 15 24 AM" src="https://github.com/user-attachments/assets/136c96db-4179-4b8d-9e8c-effe9105956f" /> | <img width="538" height="341" alt="Screenshot 2026-02-11 at 9 15 40 AM" src="https://github.com/user-attachments/assets/1c1934c1-6a49-4200-b7d3-44cc1cb7fc35" />  |


### Copying Matching Fields from TestDataAsset.asset to CharacterController.cs

<img src=https://github.com/user-attachments/assets/954912c7-c353-4780-88e5-8d91df252519 width="900"/>


### Smart Memory Preservation

The plugin preserves serialized data in memory — even if the source or target object is destroyed — allowing you to safely paste values later without losing data.

<img width="900" height="437" alt="image" src="https://github.com/user-attachments/assets/34695783-6a86-4d3f-b24c-53e9a5c24844" /> 








