using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    /// <summary>
    /// Defunct Script, separated into Looking and Movement scripts. Keep only for refference
    /// </summary>
    CharacterController charController;
    //Canvas visor;
    // Rigidbody p_body;
    public float moveSpeed = 12f;
    public float gravity = 10f;
    public float ySpeed = 100f;
    public float xSpeed = 100f;
    public bool invxAxis = false;
    //public string vScan = "Scanning...";
    public GameObject player;
    public Transform playerBody;
    //public CharacterController charController;

    private float yRotation = 0.0f;
    private float xRotation = 0.0f;
    private float velocity = 0f;
    private float playerX;
    private float playerZ;
    private Camera cam;
    private Vector3 direction = new Vector3(0, 0, 0);
    private Vector3 playerPos;
    private Ray lookAt;
    //private Vector3 moveDir = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        
        cam = Camera.main;
        charController = GetComponent<CharacterController>();

       

        //p_body = playerBody.GetComponent<Rigidbody>();
        
        
    }

    


    // Update is called once per frame
    void Update()
    {
        //Define left/right and forward backward each frame
        Vector3 moveLR = transform.right;
        Vector3 moveFB = transform.forward;
        playerPos = player.transform.position;
        
        
        
        playerX = Input.GetAxis("Horizontal");
        playerZ = Input.GetAxis("Vertical");
        Vector3 playerMove = moveLR * playerX + moveFB * playerZ;

        charController.Move(playerMove * moveSpeed * Time.deltaTime);



       
        //If player is grounded, and gravity **********

        if (charController.isGrounded)
        {
            velocity = 0;
        } else
        {
            
            velocity -= gravity * Time.deltaTime;
            velocity = Mathf.Clamp(velocity, -19, 19);
            charController.Move(new Vector3(0, velocity, 0) * Time.deltaTime);
        }


        //Camera control***************

        float mouseY = Input.GetAxis("Mouse Y") * xSpeed;
        float mouseX = Input.GetAxis("Mouse X") * ySpeed;

        if (invxAxis == true)
        {
            xRotation += mouseY;
        }
        else
        {
            xRotation -= mouseY;
        }

        yRotation += mouseX;

        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        //yRotation += mouseX;

        playerBody.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
        //playerBody.Rotate(Vector3.up * mouseX);
        


        
    }
}
