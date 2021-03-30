using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;
using TMPro;

namespace Conference.Characters
{
    namespace Conference.Scoreboards
    {
        public class ChatBehaviour : NetworkBehaviour
        {
      
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


                
                
                
                chatText = this.transform.GetChild(0).GetChild(0).gameObject.GetComponent<TMP_Text>();
                inputField = this.transform.GetChild(0).GetChild(1).gameObject.GetComponent<TMP_InputField>(); //GameObject.Find("/ChatUI/InputChat").GetComponent<TMP_InputField>();
                chatRT = chatText.GetComponent<RectTransform>();
                inputRT = inputField.GetComponent<RectTransform>();
                
                //tPVisor = player.GetComponent<TPVisor>();
                
            }

            void Awake()
            {


                this.gameObject.SetActive(false);
                
                
                
            }

            public override void OnStartAuthority()
            {

                
                this.gameObject.SetActive(true);

                OnMessage += HandleNewMessage;
            }

            public void OpenChatUI(bool onOff)
            {
                this.gameObject.SetActive(onOff);
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
                RpcHandleMessage($"[{connectionToClient.connectionId}]: {message}");

            }

            [ClientRpc]

            private void RpcHandleMessage(string message)
            {

                OnMessage?.Invoke($"/n{message}");

            }

            void Update()
            {

                

              
            }

        }
    }
}
