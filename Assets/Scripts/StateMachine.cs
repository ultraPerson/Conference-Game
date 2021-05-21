using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Cinemachine;
using Menus;
using Michsky.UI.ModernUIPack;
using Characters;

namespace States
{

public class StateMachine : MonoBehaviour
{

    string[] states = {"Menu", "Game"};
    string currentState;
    //[SerializeField] private GameObject player;
    private GameObject pauseMenu;
    [SerializeField]private CinemachineFreeLook camControl;
    [SerializeField]private CinemachineDollyCart dolly1;
    private Transform camMenu;
    private Transform camGame;

    private bool readyFreddy = false;

    private UnityAction _begin;
    // Start is called before the first frame update
    void Start()
    {
        
        currentState = "Menu";
        
        //transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
        //player = GameObject.Find("Player(Clone)");
        
        GetComponent<PauseScript>().OpenPause();
        //dolly1 = GameObject.Find("DollyCart1").GetComponent<CinemachineDollyCart>();
        

    }
    
    
    void Awake()
    {
        //GetComponent<PauseScript>().OutsideSettingsChange("Sensitivity", 0);

        camMenu = GameObject.Find("MenuStart").transform;
        camGame = GameObject.Find("GameStart").transform;

        camControl.m_XAxis.m_InputAxisName = "";
        camControl.m_YAxis.m_InputAxisName = "";

        
        dolly1.m_Speed = 0;

        pauseMenu = GameObject.Find("PauseCanvas");
        pauseMenu.transform.GetChild(1).GetChild(1).GetChild(0).GetChild(4).gameObject.SetActive(true);
        pauseMenu.transform.GetChild(1).GetChild(1).GetChild(0).GetChild(4).GetComponent<Button>().onClick.AddListener(delegate {GameStart();});
        pauseMenu.transform.GetChild(1).GetChild(1).GetChild(0).GetChild(1).gameObject.SetActive(false);
        _begin += GameStart;
        

    }

    void GameStart()
    {
        currentState = "Game";
        pauseMenu.transform.GetChild(1).GetChild(1).GetChild(0).GetChild(4).gameObject.SetActive(false);
        pauseMenu.transform.GetChild(1).GetChild(1).GetChild(0).GetChild(1).gameObject.SetActive(true);
        pauseMenu.SetActive(false);
        dolly1.m_Speed = 20;
        GetComponent<PauseScript>().StartGame();
        GetComponent<TPMove>().FromTheBeach(camMenu, camGame);
        //transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
        

    }

    void Update()
    {
        // if(dolly1.m_Position > 1)
        // {
        //     //
            
        // }
        if(dolly1.m_Position >= 81 && readyFreddy == false)
        {
            camControl.m_Follow = this.gameObject.transform;
            camControl.m_LookAt = GetComponent<Transform>().GetChild(2);
            camControl.m_XAxis.m_InputAxisName = "Mouse X";
            camControl.m_YAxis.m_InputAxisName = "Mouse Y";
            if(GetComponent<PauseScript>().mouseS.value == 1)
            {
                GetComponent<PauseScript>().OutsideSettingsChange("Sensitivity", 25);
            }
            GetComponent<TPMove>().TogglePC();
            readyFreddy = true;
        }
    }
}
}

    
