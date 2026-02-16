using System.Collections.Generic;
using UnityEngine;

public class CharacterSetup : MonoBehaviour
{
    [SerializeField]
    private GameObject characterPrefab;

    [SerializeField]
    private string playerName;
    
    [SerializeField]
    private Color primaryColor;
    
    [SerializeField]
    private int healthPoints;
    
    [SerializeField]
    private float movementSpeed;

    [SerializeField] 
    private Vector3 spawnPosition;
    
    [SerializeField]
    private List<Vector3> patrolPoints;
    
}
