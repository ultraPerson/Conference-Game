using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using Scoreboards;
using Menus;
using Michsky.UI.ModernUIPack;


namespace Characters
{
   
        public class NPCScript : MonoBehaviour
        {

            //public GameObject playerChar;
            //public GameObject dialogueBox;
            public bool hasQuiz;
            public string nPCName;
            public GameObject dialogueBox;
            [SerializeField] private GameObject shadow;
            public Text dialogueBoxText;
            public TMP_Text quizText;
            public Text interfaceHelp;
            private Camera mainCam;//private
            public Camera thisCam;
            public bool met = false;
            public Canvas dialogueCanvas;
            private Canvas mainCanvas;//private
            public NotificationManager quizCanvas;
            private GameObject pauseCtrl;//private
            private GameObject tPCam;//private
            //public Sprite[] faces;
            //public GameObject face;
            public bool animPlay
            {
                get { return conversing; }
                private set { conversing = value; }
            }
            //public GameObject vision;

            //private bool seesYou = false;
            private int qLevel = 0;
            private int numberOfQ;
            private bool quizing = false;
            private bool conversing = false;
            private bool answer;
            [SerializeField] private bool[] correctAnswers = { true, false };
            [SerializeField] private string[] extraResponse = {"The answer you gave was incorrect!", "My father was a winged pringle."};
            private bool feedbackMode = false;
            [SerializeField] private string[] quizQuestions = { "This statement is false?", "What is the difference between a duck?" };
            private int dialoguePos = 0;
            //private Animation anim;
            private TPVisor vScript;
   
           

            [SerializeField]private string[] dialogue = {
        "Hello! My name is Enpici Defaultson.",
        "I don't have much to say, really.",
        "Let's see how much dialogue I can fit in this box here. Bla bla bla bla bla. Capitalism makes political enemies of us all. Ablolish the state.",
    "Have you figured out that by clicking on an NPC- oh, wait...",
    };

            void Start(){

                thisCam = transform.GetChild(0).GetComponent<Camera>();
                mainCam = Camera.main;
                mainCanvas = Camera.main.transform.GetChild(0).GetComponent<Canvas>();
                
                tPCam = mainCam.gameObject;
                numberOfQ = quizQuestions.Length;
                dialogueCanvas.enabled = false;

                if(extraResponse.Length < numberOfQ)
                {
                    Array.Resize(ref extraResponse, numberOfQ);
                }


            }



            // Start is called before the first frame update
            void Awake()
            {
            
                

                thisCam.enabled = false;
                //Debug.Log(dialogue.Length);
                //quizCanvas.SetActive(false);
                //anim = face.GetComponent<Animation>();
                met = false;
                //anim.playAutomatically = false;
                //dialogueBox.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width * 0.8f, Screen.height / 3);

            }

            public void Interact()
            {
                

                if (quizing || !conversing)
                {
                    conversing = true;
                    pauseCtrl.GetComponent<PauseScript>().ConvoStatus(conversing);

                    mainCam.enabled = false;
                    //change audio listener
                    transform.GetChild(0).GetComponent<AudioListener>().enabled = true;
                    tPCam.SetActive(false);
                    dialogueCanvas.enabled = true;
                    mainCanvas.enabled = false;
                    thisCam.enabled = true;
                    // Cursor.lockState = CursorLockMode.None;
                    // Cursor.visible = true;
                    // Debug.Log(Cursor.lockState);
                }
                if (hasQuiz)
                {
                    //show quiz option
                    //quizCanvas.SetActive(true);
                    quizCanvas.OpenNotification();

                }

                Converse();
/*
                if (!met)
                {
                    Converse();
                }*/



            }

            public void Converse()
            {
                // Speak(dialoguePos, met);
                // anim.Play("TalkingFace");

               /* if (!met)
                {
                    
                    dialogueBoxText.text = dialogue[0];
                    met = true;
                }
                else //try
                    {*/
                    met = true;
                        dialogueBoxText.text = dialogue[ChooseDialogue(dialoguePos)];
                        interfaceHelp.text = "Click to continue...";
                    //}

                //face.GetComponent<SpriteRenderer>().sprite = faces[new System.Random().Next(faces.Length - 1)];
                  





            }
           


            void QuizTime()
            {
                feedbackMode = false;
                quizing = true;

                if (qLevel < quizQuestions.Length)
                {
                    dialogueBoxText.text = quizQuestions[ChooseQuestion(qLevel)];
                    interfaceHelp.text = "'T' for true, 'F' for false";

                    //quizCanvas.transform.position = dialogueBox.transform.position;
                    //quizCanvas.GetComponent<RectTransform>().sizeDelta = dialogueBox.GetComponent<RectTransform>().sizeDelta;
                    //dialogueBox.SetActive(false);
                }
                else

                if (qLevel == quizQuestions.Length)
                {

                    EndQuiz();
                    qLevel = quizQuestions.Length;

                }



            }

            int ChooseQuestion(int pos)
            {

                if (pos > quizQuestions.Length)
                {
                    return qLevel = quizQuestions.Length;
                }
                else return qLevel;

            }

            int ChooseDialogue(int pos)
            {
                if (pos >= dialogue.Length)
                {
                    dialoguePos = dialogue.Length -1;
                    return dialoguePos;
                }
                else 
                {
                    
                    dialoguePos ++;
                    Debug.Log("dialoguePos set to " + pos);
                    return pos;


                }
            }

            void EndQuiz()
            {
                hasQuiz = false;
                quizing = false;
                quizCanvas.CloseNotification();
                //dialogueBox.SetActive(true);
                Interact();
            }

            void AnswerCheck(bool ans)
            {
                TPVisor vScript = pauseCtrl.GetComponent<TPVisor>();
                feedbackMode = true;


                if (ans == correctAnswers[qLevel])
                {

                    Debug.Log(qLevel);
                    
                    vScript.points++;
                    dialogueBoxText.text = $"Correct ansewer: {ans.ToString()}. Excellent!\n";
                    
                    interfaceHelp.text = "Enter/Return";
                    
                    qLevel++;
                }
                else
                {
                    bool correct = !ans;
                    //feedbackMode = true;
                    dialogueBoxText.text = $"Correct ansewer: {correct.ToString()}. Aww, too bad...\n";
                    interfaceHelp.text = "Enter/Return";
                    qLevel++;
                }

                if(!string.IsNullOrEmpty(extraResponse[qLevel -1]))
                    {
                        dialogueBoxText.text += extraResponse[qLevel - 1];
                        Debug.Log(extraResponse[qLevel - 1]);
                    }
            }

            public void LeaveConvo()
            {
                conversing = false;
                pauseCtrl.GetComponent<PauseScript>().ConvoStatus(conversing);
                mainCam.enabled = true;
                
                transform.GetChild(0).GetComponent<AudioListener>().enabled = false;
                tPCam.SetActive(true);
                mainCanvas.enabled = true;
                thisCam.enabled = false;
                //Cursor.lockState = CursorLockMode.Locked;
                // Debug.Log(Cursor.lockState);
                //quizing = false;
                quizCanvas.CloseNotification();
                //face.GetComponent<SpriteRenderer>().sprite = faces[0];
            }



            void Update()
            {

                if(pauseCtrl == null){
                    if(GameObject.Find("Player"))
                    {
                        pauseCtrl = GameObject.Find("Player");
                    } else pauseCtrl = GameObject.Find("Player(Clone)");
                }
                //Mathf.Clamp(dialoguePos, 0, dialogue.Length);
                //NPC sightline
                /*Vector3 targetDir = pauseCtrl.transform.position - transform.position;
                Debug.DrawLine(targetDir, transform.forward);
                float playerAngle = Vector3.Angle(targetDir, transform.forward);

                if (playerAngle < 15)
                {
                    seesYou = true;
                    Debug.Log(nPCName + " sees you");
                }
                else
                {
                    seesYou = false;
                    Debug.Log(nPCName + " lost sight of you");
                }*/

                //NPC conversing and quiz rules
                if (conversing)
                {
                    //fit dialogue to screen
                    dialogueBox.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width * 0.8f, Screen.height / 5);
                    shadow.GetComponent<RectTransform>().sizeDelta = new Vector2(dialogueBox.GetComponent<RectTransform>().sizeDelta.x + 125f, dialogueBox.GetComponent<RectTransform>().sizeDelta.y + 100f);
                    dialogueBoxText.GetComponent<RectTransform>().sizeDelta = dialogueBox.GetComponent<RectTransform>().sizeDelta;
                   // nextText.GetComponent<RectTransform>().sizeDelta = new Vector2(dialogueBox.GetComponent<RectTransform>().sizeDelta.x / 5, dialogueBox.GetComponent<RectTransform>().sizeDelta.y / 6);
                   // nextText.GetComponent<RectTransform>().position = new Vector2((dialogueBox.GetComponent<RectTransform>().position.x / 2) * -1, dialogueBox.GetComponent<RectTransform>().position.y);
                    /*
                    //size next button and text according to screen
                    next.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width / 6, Screen.height / 11);
                    next.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = next.GetComponent<RectTransform>().sizeDelta;
                    //size leave button and text according to screen
                    leave.GetComponent<RectTransform>().sizeDelta = next.GetComponent<RectTransform>().sizeDelta;
                    leave.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = next.GetComponent<RectTransform>().sizeDelta;
                    */

                    if (Input.GetKeyDown(KeyCode.Backspace))
                    {
                        LeaveConvo();
                    }

                    if (Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        Converse();
                        Debug.Log("Click");
                    }


                    if (hasQuiz)
                    {
                        if (!quizing || feedbackMode)
                        {

                            if (Input.GetKeyDown(KeyCode.Return))
                            {
                                QuizTime();
                                Debug.Log("ran QuizTime");
                            }
                        }
                        else

                        if (quizing && conversing)
                        {
                            if (!feedbackMode)
                            {

                                if (Input.GetKeyDown(KeyCode.T))
                                {
                                    answer = true;
                                    AnswerCheck(answer);

                                }
                                else if (Input.GetKeyDown(KeyCode.F))
                                {
                                    answer = false;
                                    AnswerCheck(answer);
                                }
                            }

                        }
                    }

                }



                dialogueCanvas.enabled = thisCam.enabled;


            }

        }
    }


