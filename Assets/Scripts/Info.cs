using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Info : MonoBehaviour
{
    public int index = 0;
    GameManager gameManager;

    void Start()
    {
        if (index == 0)
        {
            Debug.LogError("Index not initialized");
        }
        gameManager = GameManager.Instance;
    }

    void Update()
    {
        
    }

}
