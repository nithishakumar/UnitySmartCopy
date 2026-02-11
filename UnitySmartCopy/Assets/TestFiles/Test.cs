using System.Collections.Generic;
using UnityEngine;

public class Test1 : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed;

    [SerializeField] 
    private Vector3 spawnPosition;

    [SerializeField]
    private List<Vector3> patrolPoints;

    [SerializeField]
    private string playerName;

    [SerializeField]
    private Color primaryColor;

    [SerializeField]
    private GameObject characterPrefab;

    [SerializeField] 
    private string uniqueID;

    [SerializeField]
    private Color secondaryColor;

    [SerializeField]
    private int healthPoints;
}


