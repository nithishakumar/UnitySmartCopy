# SmartCopy - Copy Serialized Values Between Different Unity Object Types
SmartCopy is a Unity Editor plugin that lets you copy and paste serialized field values between any Unity objects by intelligently matching property path and type.
It works across any built-in components, MonoBehaviours, and ScriptableObjects — even if they are unrelated classes — as long as their serialized fields match.

#### What is copied?

SmartCopy copies and pastes all modifiable serializable fields, including:

- Public fields
- Private fields marked with [SerializeField]
- Fields marked with [HideInInspector]
- Nested serialized data
  
If the property path and type match, the value is transferred automatically. Properties that don't match are left unaltered.

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

The plugin preserves serialized data in memory — even if the source or target object is destroyed — allowing you to safely paste values later without losing data. This would be useful when copying values from objects in play mode.

<p align="center">
<img width="800" height="437" alt="image" src="https://github.com/user-attachments/assets/d3a8a841-045d-4cdf-a313-a054fabf11db" />
</p>

| TestDataAsset.asset (CharacterDataAsset) | TestDataAsset1.asset (CharacterDataV2Asset) |
|----------|----------|
| <img width="550" height="543" alt="Screenshot 2026-02-11 at 9 29 51 AM" src="https://github.com/user-attachments/assets/1c1b9ff2-eba9-4e06-adfe-aaac4aca7e88" /> | <img width="546" height="522" alt="Screenshot 2026-02-11 at 9 30 07 AM" src="https://github.com/user-attachments/assets/49951cd5-7491-43bc-a401-fac05bc1da4a" /> |

Note that BaseStats.CritChance is not copied since it doesn't exist in TestDataAsset1.asset.


## Minor Difference between SmartCopy and Unity's Default Copy/Paste Behavior 

### When copying a component from Prefab Stage to another component of the same type, the following difference occurs:

If the component is attached to the root prefab object and contains a self-reference (e.g., references a function on itself), Unity’s default Copy/Paste Component remaps the root reference to the target object. SmartCopy does not perform this remapping.

### When the Prefab Stage is Closed Before Copying:

Unity's default behavior maps OnClick's target to the root object (Cube) in the scene, however, SmartCopy doesn't set it to a valid object because the object reference is lost after the prefab stage has closed. Note how both don't set the Character Prefab to the Cube object since it is not present in the current scene.

![ezgif-3452865fcaed27bb](https://github.com/user-attachments/assets/ec6e8714-1a29-49cf-ba24-52dbd17b24a3)

![ezgif-32d0dd6e98a24856](https://github.com/user-attachments/assets/5db555c1-de69-4826-975e-cfce8c875bad)


### When the Prefab Stage is Open Before Copying:

Unity's default behavior maps OnClick's target to the root object (Cube) in the scene, however, SmartCopy copies the root prefab object (Capsule 1) from the Prefab asset itself. Note how both set the Character Prefab to the Cube object since it's still a valid persisent reference because the prefab stage is not closed.

![ezgif-3047f4ea52198aa4](https://github.com/user-attachments/assets/bb798c68-d452-422f-8d2f-4f3ef1b77b08)

![ezgif-308344b5fd8a33df](https://github.com/user-attachments/assets/d708ed71-0888-42e3-9539-7c32243228af)

### However, if the component is attached to a non-root prefab object, both Unity’s default behavior and SmartCopy behave identically.

## Installation

### Install from Asset Store

Visit the link below to directly download the package.

```
    https://assetstore.unity.com/packages/slug/361374
```

### Install using GitHub URL

Open the Package Manager window by selecting Window > Package Manager, then click on [+] > Add package from git URL and enter the following URL:
```
    https://github.com/nithishakumar/UnitySmartCopy?path=SmartCopy
```

### Install by Name

Installing from the Asset Store

Open the Package Manager window by selecting Window > Package Manager, then click on [+] > Add package by name and enter "SmartCopy".









