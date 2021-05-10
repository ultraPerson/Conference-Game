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
            Instantiate(playerPrefab, new Vector3(4.9f, 73.7f, -62.1f), Quaternion.identity);
        }
        
    }
}
