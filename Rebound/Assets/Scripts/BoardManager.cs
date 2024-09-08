using System;
using System.Security.Cryptography;
using UnityEngine;

public class BoardManager : Singleton<BoardManager>
{
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] private GameObject linePrefab;

    private void Awake() {
        dotPrefab = Resources.Load<GameObject>("Prefabs/Dot");
        linePrefab = Resources.Load<GameObject>("Prefabs/Line");
        
    }


}
