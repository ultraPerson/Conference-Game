using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBehaviour : MonoBehaviour
{

    private GameObject player;
    private Vector3 forceDir;
    //private bool kick = false;

    // Start is called before the first frame update
    void Start()
    {

        player = GameObject.Find("Player");
        
    }

    // Update is called once per frame
    void Update()
    {

        if(player == null)
        {
            player = GameObject.Find("Player(Clone)");
        }
        
    }

    

    public void Kick(Vector3 dir)
    {

        this.GetComponent<Rigidbody>().AddForce(dir, ForceMode.Impulse);
        
    }
}
