using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Characters;

public class GoalBehaviour : MonoBehaviour
{
    bool scored = false;
    [SerializeField] GameObject player;
    ParticleSystem confetti;

    void Start()
    {
        confetti = GetComponent<ParticleSystem>();
    }

    void OnTriggerEnter(Collider collider)
    {
        if(collider.name == "Ball")
        {
            if(scored == false)
            {
                scored = true;
                player.GetComponent<TPVisor>().points++;
                confetti.Play();
                Debug.Log("Goal");
            }
        }
    }
    

    
}
