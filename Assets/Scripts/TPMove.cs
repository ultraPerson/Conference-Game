using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
            private TPVisor tPVisor;


            public float turnSmooth = 0.1f;
            float turnSmoothVel;

            void Start()
            {
                tPVisor = GetComponent<TPVisor>();
                cam = GameObject.Find("/Main Camera").GetComponent<Transform>();
            }

            // Update is called once per frame
            void Update()
            {


                float accelerate = 0;
                Mathf.Clamp(accelerate, 0f, 1f);
                float h = Input.GetAxisRaw("Horizontal");
                float z = Input.GetAxisRaw("Vertical");
                //float fallSpeed = 0;
                Vector3 direction = new Vector3(h, 0f, z).normalized;

                charAnim.SetFloat("motionSpeed", direction.magnitude);

                //Debug.Log(charAnim.GetFloat("motionSpeed"));

                if (!tPVisor.chatOpen)
                {

                    if (direction.magnitude >= 0.1f)
                    {
                        while (accelerate < 100)
                        {
                            accelerate += 0.1f;
                        }

                        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVel, turnSmooth);
                        transform.rotation = Quaternion.Euler(0f, angle, 0f);
                        moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;



                        //controller.Move(new Vector3(0f, -1, 0f) * gravity * Time.deltaTime);
                        controller.Move((moveDir.normalized * (speed * (accelerate / 100)) * Time.deltaTime) + (gravity * Time.deltaTime));
                    }

                    if (!controller.isGrounded)
                    {
                        gravity = new Vector3(0f, -9.8f, 0f);
                    }
                    else gravity = Vector3.zero;
                }



            }
        }
        
    }
