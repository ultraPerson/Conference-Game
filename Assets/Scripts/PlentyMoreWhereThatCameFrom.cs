using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Menus;

public class PlentyMoreWhereThatCameFrom : MonoBehaviour
{
    
    [SerializeField] private GameObject playerPrefab;
    

    // Update is called once per frame
    void Update()
    {
        if(GameObject.Find("Player") == null && GameObject.Find("Player(Clone)") == null)
        {
            
            GameObject.Find("AudioZoner").transform.GetChild(0).GetComponent<AudioControl>().player = Instantiate(playerPrefab, new Vector3(4.9f, 73.7f, -62.13f), Quaternion.identity);
        }
        
    }
}
