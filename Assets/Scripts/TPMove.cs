using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Scoreboards;
using Menus;

namespace Characters
{
    
        
        public class TPMove : MonoBehaviour
        {
            public CharacterController controller;
            public Animator charAnim;
            private Transform cam;

            public float speed = 6f;
            public Vector3 gravity;
            private Vector3 moveDir;
            private Vector3 forcePosA;
            private Vector3 forcePosB;
            private TPVisor tPVisor;
            public bool playerControl
            {get; private set;}

            public float turnSmooth = 0.1f;
            float turnSmoothVel;

            void Start()
            {
                tPVisor = GetComponent<TPVisor>();
                cam = GameObject.Find("Main Camera").GetComponent<Transform>();
                playerControl = false;
            }

            // Update is called once per frame
            void FixedUpdate()
            {

                float h = Input.GetAxisRaw("Horizontal");
                float z = Input.GetAxisRaw("Vertical");

                if(!tPVisor.isPaused){

                float accelerate = 100f;
                //Mathf.Clamp(accelerate, 0f, 1f);
                
                //float fallSpeed = 0;
                Vector3 direction = new Vector3(h, 0f, z).normalized;

                charAnim.SetFloat("motionSpeed", direction.magnitude);
                


            
                
                if (!controller.isGrounded)
                    {
                        gravity = new Vector3(0f, -.1f, 0f);
                        controller.Move(gravity);
                    }
                    

                //Debug.Log(charAnim.GetFloat("motionSpeed"));

                

                    if (direction.magnitude >= 0.1f)
                    {
                        

                        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVel, turnSmooth);
                        transform.rotation = Quaternion.Euler(0f, angle, 0f);
                        moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;



                        //controller.Move(new Vector3(0f, -1, 0f) * gravity * Time.deltaTime);
                        controller.Move((moveDir.normalized * (speed * (accelerate / 100)) * Time.deltaTime) + (gravity * Time.deltaTime));
                    }

                    
                }


             
            // else
            // {
            //     while(controller.transform.position != forcePosB)
            //     {
            //     controller.Move(Vector3.Lerp(forcePosA, forcePosB, Time.time));
            //     }
            //     if(controller.transform.position == forcePosB)
            //     {
            //         playerControl = true;
            //     }
            // }

            
            }

            // public void FromTheBeach(Transform a, Transform b)
            // {
            //     forcePosA = a.position;
            //     forcePosB = b.position;
            //     playerControl = false;
            // }
            public void TogglePC()
            {
                playerControl = !playerControl;
                //charAnim.SetBool("playerControl", playerControl);
            }
        }
        
    }
