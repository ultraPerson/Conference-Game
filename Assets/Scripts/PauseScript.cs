using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Characters;
using Scoreboards;
using Cinemachine;


namespace Menus{
public class PauseScript : MonoBehaviour
{
    private GameObject pauseUI;
    private GameObject visorCanvas;
    private GameObject settingsObj;
    private GameObject helpObj;
    private GameObject previous;
    private GameObject player;
    
    private GameObject male;
    private GameObject fem;
    private GameObject mann;

    private UnityAction _closePause;
    private UnityAction _settings;
    private UnityAction _help;
    private UnityAction _back;
    private UnityAction _startGame;

    private CinemachineFreeLook camRig;

    private string pObjName = "Player";
    private string pObjNameAlt = "Player(Clone)";

    private Button back;
    private Slider mouseS;
    private Dropdown charList;
    
    
    
    private string currentScreen;
    public bool isPaused 
    {get; private set;}
    private bool talking = false;
    
    void Start(){

        _closePause += ClosePause;
        _help += Help;
        _settings += Settings;
        _back += BackFunction;
        _startGame += StartGame;

        
        pauseUI = GameObject.Find("/PauseCanvas");
        visorCanvas = GameObject.Find("/Main Camera/VisorCanvas");
        player = GameObject.Find(pObjName);
        //player.SetActive(true);
        
        
        
        settingsObj = pauseUI.transform.GetChild(1).gameObject;
        helpObj = pauseUI.transform.GetChild(2).gameObject;
        mouseS = settingsObj.transform.GetChild(4).GetChild(1).GetComponent<Slider>();
        camRig = transform.GetChild(1).GetComponent<CinemachineFreeLook>();
        charList = settingsObj.transform.GetChild(5).GetChild(1).GetComponent<Dropdown>();

        mouseS.minValue = 1f;
        mouseS.maxValue = 100f;
        mouseS.onValueChanged.AddListener(delegate {SensitivityChange(mouseS.value);});
        mouseS.value = 25;

        charList.onValueChanged.AddListener(delegate {CharacterSelect(charList.value);});
        
        
        


        pauseUI.transform.GetChild(0).GetChild(2).GetComponent<Button>().onClick.AddListener(_closePause);
        pauseUI.transform.GetChild(0).GetChild(3).GetComponent<Button>().onClick.AddListener(_settings);
        settingsObj.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(_help);
        
        IntroScreen();

    }

    public void IntroScreen()
    {

        settingsObj.SetActive(false);
        helpObj.SetActive(true);
        pauseUI.SetActive(true);

        
        currentScreen = "Help";
        
        
        
        back = helpObj.transform.GetChild(1).gameObject.GetComponent<Button>();
        back.GetComponentInChildren<Text>().text = "Continue";
        
        back.onClick.AddListener(_startGame);
        visorCanvas.SetActive(false);
        Time.timeScale = 0;

    }
    

    private void Update()
    {

        if(player == null){

            pObjName = pObjNameAlt;
            
            player = GameObject.Find(pObjName);
            player.SetActive(true);
            
        }

        
        
        

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!talking)
            {
                isPaused = !isPaused;
            }

            if (isPaused)
        {
            OpenPause();
        } else
        {
            ClosePause();
        }
        }

        
        
    }

    public void StartGame()
    {
        currentScreen = "Play";
        helpObj.SetActive(false);
        pauseUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
        isPaused = false;
        visorCanvas.SetActive(true);
        back.onClick.RemoveListener(_startGame);
    }
    public void OpenPause()
    {
        currentScreen = "Pause";
        pauseUI.SetActive(true);
        pauseUI.transform.GetChild(0).gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
        isPaused = true;
        visorCanvas.SetActive(false);
        settingsObj.SetActive(false);
        helpObj.SetActive(false);
       
    }

    public void ClosePause()
    {
        currentScreen = "Play";
        pauseUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
        isPaused = false;
        visorCanvas.SetActive(true);
        
       
    }

    public void Settings()
    {
        currentScreen = "Settings";
       
        settingsObj.SetActive(true);
        
        back = settingsObj.transform.GetChild(2).gameObject.GetComponent<Button>();
        back.onClick.RemoveListener(_back);
        back.onClick.AddListener(_back);
      
        
        previous = pauseUI.transform.GetChild(0).gameObject;
        previous.SetActive(false);
        helpObj.SetActive(false);
        
        



    }

    public void SensitivityChange(float val)
    {
        

        camRig.m_XAxis.m_MaxSpeed = val * 10;
        camRig.m_YAxis.m_MaxSpeed = val / 30;

    }

    public void CharacterSelect(int val)
    {

        switch (val)
        {

            case 0:
            
            player.transform.GetChild(0).gameObject.SetActive(false);
            player.transform.GetChild(3).gameObject.SetActive(false);
            player.transform.GetChild(4).gameObject.SetActive(true);
            male = GameObject.Find($"{pObjName}/Male");
            player.GetComponent<TPMove>().charAnim = male.GetComponent<Animator>();
            break;

            case 1:
            
            player.transform.GetChild(0).gameObject.SetActive(false);
            player.transform.GetChild(3).gameObject.SetActive(true);
            player.transform.GetChild(4).gameObject.SetActive(false);
            fem = GameObject.Find($"{pObjName}/Female");
            player.GetComponent<TPMove>().charAnim = fem.GetComponent<Animator>();
            break;

            case 2:
            
            player.transform.GetChild(0).gameObject.SetActive(true);
            player.transform.GetChild(3).gameObject.SetActive(false);
            player.transform.GetChild(4).gameObject.SetActive(false);
            mann = GameObject.Find($"{pObjName}/Mannequine");
            player.GetComponent<TPMove>().charAnim = mann.GetComponent<Animator>();
            break;

            default:
            Debug.Log("Character selection error");
            break;

        }

    }

    public void Help()
    {

        Debug.Log("Help");
        currentScreen = "Help";
        helpObj.SetActive(true);
        settingsObj.SetActive(false);
        previous = settingsObj;
        back = helpObj.transform.GetChild(1).gameObject.GetComponent<Button>();
        back.onClick.RemoveListener(_back);
        back.onClick.AddListener(_back);



    }

    public void BackFunction()
    {
        //Debug.Log("Back");
        switch(currentScreen)
        {
            case "Help":
            Settings();
            Debug.Log($"Back, {currentScreen}");
            break;

            case "Settings":
            OpenPause();
            Debug.Log($"Back, {currentScreen}");
            break;

            default:
            ClosePause();
            Debug.Log($"Back, {currentScreen}");
            break;

        }
        
    
    }

    public void ConvoStatus(bool convo)
    {

        talking = convo;

    }
}
}
   
