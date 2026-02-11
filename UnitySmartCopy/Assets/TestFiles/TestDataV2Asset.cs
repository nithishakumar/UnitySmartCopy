using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "TestDataAsset", menuName = "Test/Test Data Asset")]
public class TestDataV2Asset : ScriptableObject
{
    [Header("Primitive Fields")]
    public int health;
    public float movementSpeed;

    [SerializeField] private string internalId;

    [Header("Struct")]
    public Stats baseStats;

    [Header("Collections")]
    public int[] experienceLevels;
    public List<string> tags;

    [Header("References")]
    public GameObject characterPrefab;

    [System.Serializable]
    public struct Stats
    {
        public int strength;
        public int agility;
        public float criticalHitRate;
    }
}