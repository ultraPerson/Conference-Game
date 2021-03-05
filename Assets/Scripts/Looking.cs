using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Looking : MonoBehaviour
{

    public Transform playerBody;
    public float xSpeed = 100f;
    public float ySpeed = 100f;
    public bool invxAxis = false;

    private float yRotation = 0.0f;
    private float xRotation = 0.0f;


    // Start is called before the first frame update
    void Start()
    {

        Cursor.lockState = CursorLockMode.Locked;

    }

    // Update is called once per frame
    void Update()
    {

        float mouseY = Input.GetAxis("Mouse Y") * xSpeed * Time.deltaTime;
        float mouseX = Input.GetAxis("Mouse X") * ySpeed * Time.deltaTime;

        if (invxAxis == true)
        {
            xRotation += mouseY;
        }
        else
        {
            xRotation -= mouseY;
        }

        yRotation += mouseX;
        if (xRotation < 80 && xRotation > -80)
        {
            xRotation = Mathf.Clamp(xRotation, -80f, xRotation);
            
        }
        else
        {
            xRotation = Mathf.Clamp(xRotation, -80f, 80f);
        }
        //yRotation += mouseX;

        playerBody.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);

    }
}
