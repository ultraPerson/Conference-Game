using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlentyMoreWhereThatCameFrom : MonoBehaviour
{
    
    [SerializeField] private GameObject playerPrefab;
    

    // Update is called once per frame
    void Update()
    {
        if(GameObject.Find("Player") == null && GameObject.Find("Player(Clone)") == null)
        {
            Instantiate(playerPrefab);
        }
        
    }
}
