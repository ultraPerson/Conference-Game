using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;
using TMPro;
using Scoreboards;
using Characters;

namespace Menus
{
    
        public class ChatBehaviour : NetworkBehaviour
        {
            
            [SerializeField] private GameObject chatUI;
            [SerializeField] private TMP_Text chatText;
            [SerializeField] private TMP_InputField inputField;

            List<GameObject> rootObjects = new List<GameObject>();
            
            

            
            private GameObject player;
            private RectTransform chatRT;
            private RectTransform inputRT;
            private bool chatActive = false;
            //private TPVisor tPVisor;

            private static event Action<string> OnMessage;

            void Start()
            {

                player = GameObject.Find("Player");
                //player.SetActive(true);

                //chatUI = this.transform.GetChild(5).gameObject;


                chatUI.SetActive(false);
                
                
                chatText = chatUI.transform.GetChild(0).GetChild(0).gameObject.GetComponent<TMP_Text>();
                inputField = chatUI.transform.GetChild(0).GetChild(1).gameObject.GetComponent<TMP_InputField>(); //GameObject.Find("/ChatUI/InputChat").GetComponent<TMP_InputField>();
                chatRT = chatText.GetComponent<RectTransform>();
                inputRT = inputField.GetComponent<RectTransform>();

                inputField.onEndEdit.AddListener(delegate {Send(inputField.text);});
                
                //tPVisor = player.GetComponent<TPVisor>();
                
            }

            void Awake()
            {

                if(chatUI == null)
                {
                    chatUI = GameObject.Find("/ChatUI(Clone)");
                }


                
                
                
                
            }

            public override void OnStartAuthority()
            {

                
                chatUI.SetActive(true);

                OnMessage += HandleNewMessage;
            }

            public void OpenChatUI(bool onOff)
            {
                chatUI.SetActive(onOff);
                chatActive = onOff;
                Debug.Log("Chat active: " + onOff);
            }

            [ClientCallback]

            private void OnDestroy()
            {
                if (!hasAuthority) { return; }

                OnMessage -= HandleNewMessage;
            }

            private void HandleNewMessage(string message)
            {
                chatText.text += message;
            }

            [Client]

            public void Send(string message)
            {
                if (!Input.GetKeyDown(KeyCode.Return)) { return; }
                if (string.IsNullOrWhiteSpace(message)) { return; }

                CmdSendMessage(message);
                inputField.text = string.Empty;
                Debug.Log("Send message cmd received");

            }

            [Command]

            private void CmdSendMessage(string message)
            {
                //this will be username
                RpcHandleMessage($"/n[{connectionToClient.connectionId}]: {message}");

            }

            [ClientRpc]

            private void RpcHandleMessage(string message)
            {

                OnMessage?.Invoke($"{message}");

            }

            void Update()
            {

                

              
            }

        }
    }

