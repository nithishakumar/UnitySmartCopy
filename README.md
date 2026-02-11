# UnitySmartCopy
UnitySmartCopy is a Unity Editor plugin that lets you copy and paste serialized field values between any Unity objects by intelligently matching property path and type.
It works across any built-in components, MonoBehaviours, and ScriptableObjects — even if they are unrelated classes — as long as their serialized fields match.

#### What is copied?

UnitySmartCopy copies and pastes all modifiable serializable fields, including:

- Public fields
- Private fields marked with [SerializeField]
- Fields marked with [HideInInspector]
- Nested serialized data
  
If the property path and type match, the value is transferred automatically.

### Copying Matching Fields from CharacterProfile.cs to CharacterSetup.cs

<p align="center">
<img src=https://github.com/user-attachments/assets/72a228bb-2ddb-454f-8fcf-a104eb87b903 width="400"/>
</p>

| CharacterProfile.cs | CharacterSetup.cs |
|----------|----------|
| <img width="538" height="363" alt="Screenshot 2026-02-11 at 9 15 24 AM" src="https://github.com/user-attachments/assets/136c96db-4179-4b8d-9e8c-effe9105956f" /> | <img width="538" height="341" alt="Screenshot 2026-02-11 at 9 15 40 AM" src="https://github.com/user-attachments/assets/1c1934c1-6a49-4200-b7d3-44cc1cb7fc35" />  |


### Copying Matching Fields from TestDataAsset.asset to CharacterController.cs
<br>
<p align="center">
<img src=https://github.com/user-attachments/assets/954912c7-c353-4780-88e5-8d91df252519 width="900"/>
</p>
<br>

| TestDataAsset.asset | CharacterController.cs |
|----------|----------|
| <img width="549" height="535" alt="Screenshot 2026-02-11 at 9 21 51 AM" src="https://github.com/user-attachments/assets/01ff1401-9133-4a15-b9ed-dc730d1f8658" /> | <img width="540" height="402" alt="Screenshot 2026-02-11 at 9 22 37 AM" src="https://github.com/user-attachments/assets/8c8a194b-0787-4ee1-a95c-38de2a7200dc" /> |


### Smart Memory Preservation

The plugin preserves serialized data in memory — even if the source or target object is destroyed — allowing you to safely paste values later without losing data.

<p align="center">
<img width="800" height="437" alt="image" src="https://github.com/user-attachments/assets/7b020c2f-8686-45eb-84cb-2d4ef7001e86" />
</p>

| TestDataAsset.asset (CharacterDataAsset) | TestDataAsset1.asset (CharacterDataV2Asset) |
|----------|----------|
| <img width="550" height="543" alt="Screenshot 2026-02-11 at 9 29 51 AM" src="https://github.com/user-attachments/assets/1c1b9ff2-eba9-4e06-adfe-aaac4aca7e88" /> | <img width="546" height="522" alt="Screenshot 2026-02-11 at 9 30 07 AM" src="https://github.com/user-attachments/assets/49951cd5-7491-43bc-a401-fac05bc1da4a" /> |

Note that BaseStats.CritChance is not copied since it doesn't exist in TestDataAsset1.asset.







