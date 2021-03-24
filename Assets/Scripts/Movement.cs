using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    public CharacterController charController;
    public float moveSpeed = 12f;
    public float gravity = -9.81f;
    //public Animator animator;

    private float playerX;
    private float playerZ;
    private Vector3 velocity;
    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {

        Vector3 moveLR = transform.right;
        Vector3 moveFB = transform.forward;

        playerX = Input.GetAxis("Horizontal");
        playerZ = Input.GetAxis("Vertical");
        Vector3 playerMove = moveLR * playerX + moveFB * playerZ;

        charController.Move(playerMove * moveSpeed * Time.deltaTime);

        
        charController.Move(velocity * Time.deltaTime);

        if (charController.isGrounded)
        {
            velocity.y = 0;
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }
        //float averageSpeed = (velocity.x * velocity.z) / 2;
        //animator.SetFloat("Speed", averageSpeed);

    }
}
