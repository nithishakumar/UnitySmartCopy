using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "TestDataAsset", menuName = "Test/Test Data Asset")]
public class TestDataAsset : ScriptableObject
{
    [Header("Primitive Fields")]
    public int health;
    public float speed;

    [SerializeField] private string internalId;

    [Header("Struct")]
    public Stats baseStats;

    [Header("Collections")]
    public int[] levels;
    public List<string> tags;

    [Header("References")]
    public GameObject prefabReference;
    public Material materialReference;

    [System.Serializable]
    public struct Stats
    {
        public int strength;
        public int agility;
        public float critChance;
    }
}