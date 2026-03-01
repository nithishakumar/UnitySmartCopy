using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterController : MonoBehaviour
{
    public struct Stats
    {
        public int strength;
        public int agility;
        public float critChance;
    }

    public UnityEvent onClick;

    [Header("Primitive Fields")] public int health;
    public float speed;

    [SerializeField] private string internalId;

    [Header("Struct")] public Stats baseStats;

    [Header("Collections")] public int[] levels;
    public List<string> tags;

    [Header("References")] public GameObject characterPrefab;
    public Material materialReference;
    
    [SerializeField]
    private CharacterSetup _characterSetup;
}