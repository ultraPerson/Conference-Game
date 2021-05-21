using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Characters;
using Scoreboards;
using Cinemachine;
using Michsky.UI.ModernUIPack;


namespace Menus{
public class PauseScript : MonoBehaviour
{
    [SerializeField]private GameObject pauseUI;
    private GameObject visorCanvas;
    [SerializeField]private GameObject settingsObj;
    [SerializeField]private GameObject helpObj;
    [SerializeField]private GameObject menuPannels;
    [SerializeField] private GameObject chatUI;
    //private GameObject previous;
    private GameObject player;
    
    private GameObject male;
    private GameObject fem;
    private GameObject mann;

    private UnityAction _closePause;
    private UnityAction _settings;
    private UnityAction _help;
    private UnityAction _back;
    private UnityAction _startGame;
    private UnityAction _updateSettings;

    private CinemachineFreeLook camRig;

    

    private string pObjName = "Player";
    private string pObjNameAlt = "Player(Clone)";
    private string settingsPath;

    //private Button back;
    public Slider mouseS
    {get; private set;}
    private HorizontalSelector charList;
    
    
    private int charInd = 0;
    private string currentScreen;
    [SerializeField] private bool isMainMenu = false;
    public bool isPaused 
    {get; private set;}
    private bool talking = false;
    private bool chatActive = false;
    
    void Start(){
        isPaused = false;

        //SettingsSaver currentSettings = GetSettings();
        //charInd = currentSettings.character;
        
        

        settingsPath  = $"{Application.persistentDataPath}/settings.Json";
        _closePause += ClosePause;
        //_help += Help;
        //_settings += Settings;
        _back += BackFunction;
        _startGame += StartGame;
        //_updateSettings += SaveSettings;
        
        pauseUI = GameObject.Find("/PauseCanvas");
        if(visorCanvas == null)
        {
        visorCanvas = GameObject.Find("/Main Camera").transform.GetChild(0).gameObject;
        }
        Debug.Log(visorCanvas.transform.name);
        player = GameObject.Find(pObjName);
        //player.SetActive(true);
        
        
        
        
        //root menu objects
            camRig = transform.GetChild(1).GetComponent<CinemachineFreeLook>();
        
        menuPannels = pauseUI.transform.GetChild(1).gameObject;

        //Settings objects
        settingsObj = menuPannels.transform.GetChild(1).GetChild(1).gameObject;
        mouseS = settingsObj.transform.GetChild(4).GetChild(1).GetComponent<Slider>();
        charList = settingsObj.transform.GetChild(5).GetChild(1).GetComponent<HorizontalSelector>();

        //Help objects
        helpObj = menuPannels.transform.GetChild(1).GetChild(2).gameObject;

        //settings variables
        mouseS.minValue = 1f;
        mouseS.maxValue = 100f;
        mouseS.onValueChanged.AddListener(delegate {SensitivityChange(mouseS.value);});
        mouseS.value = 0;
        SensitivityChange(mouseS.value);

        //charList.index = charInd;
        
        
        
        

        charList.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(_updateSettings);
        charList.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(_updateSettings);
        menuPannels.transform.GetChild(1).GetChild(0).GetChild(1).GetComponent<Button>().onClick.AddListener(_closePause);
        //pauseUI.transform.GetChild(0).GetChild(3).GetComponent<Button>().onClick.AddListener(_settings);
        //settingsObj.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(_help);
        
        if(isMainMenu)
        {
        IntroScreen();
        } else ClosePause();

    }

    public void IntroScreen()
    {
        if(visorCanvas == null)
        {
        visorCanvas = GameObject.Find("/Main Camera").transform.GetChild(0).gameObject;
        }
        currentScreen = "GameStart";
        OpenPause();
        //menuPannels.transform.GetChild(0).GetChild(2).GetComponent<Button>().onClick.Invoke();
        
        
        isPaused = true;
        
        
        
        
        Time.timeScale = 1;

    }
    

    private void Update()
    {

        if(!isMainMenu)
        {

            if(player == null){

                pObjName = pObjNameAlt;
                
                player = GameObject.Find(pObjName);
                player.SetActive(true);
            
            
            }
        }
        

        if(charInd != charList.index)
        {
            charInd = charList.index;
            CharacterSelect(charInd);
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
        ClosePause();
        
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
        isPaused = false;
        visorCanvas.SetActive(true);
        
    }
    public void OpenPause()
    {
        currentScreen = "Pause";
        pauseUI.SetActive(true);
        menuPannels.transform.GetChild(0).GetChild(0).GetComponent<Button>().onClick.Invoke();
        Cursor.lockState = CursorLockMode.None;
        if(!isMainMenu)
        {
            Time.timeScale = 0;
            menuPannels.GetComponent<WindowManager>().OpenPanel("Settings");
        } else Time.timeScale = 1;
        isPaused = true;
        if(visorCanvas == null)
        {
        visorCanvas = GameObject.Find("/Main Camera").transform.GetChild(0).gameObject;
        }
        player.transform.GetChild(3).gameObject.SetActive(true);
        visorCanvas.SetActive(false);
        
       
    }

    public void ClosePause()
    {
        currentScreen = "Play";
        pauseUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
        isPaused = false;
        visorCanvas = GameObject.Find("/Main Camera").transform.GetChild(0).gameObject;
        player.transform.GetChild(3).gameObject.SetActive(false);
        visorCanvas.SetActive(true);
        
       
    }

    public void OpenChatUI(bool onOff)
            {
                chatUI.SetActive(onOff);
                chatActive = onOff;
                Debug.Log("Chat active: " + onOff);
            }

    

    public void SensitivityChange(float val)
    {
        

        camRig.m_XAxis.m_MaxSpeed = val * 10;
        camRig.m_YAxis.m_MaxSpeed = val / 30;

    }

    public void CharacterSelect(int val)
    {
        Debug.Log($"Character list value: {val}");
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

    

    public void BackFunction()
    {
        //Debug.Log("Back");
        switch(currentScreen)
        {
            case "Help":
            //Settings();
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

    public void OutsideSettingsChange(string type, float val)
    {
        switch(type)
        {
            case "Sensitivity":
            mouseS.value = val;
            SensitivityChange(val);
            Debug.Log($"Outside call changed mouse sensitivity to: {val}");
            break;

            case "Character":
            int ind = (int)val;
            CharacterSelect(ind);
            charList.index = ind;
            charInd = ind;
            Debug.Log($"Outside call changed character index to {(int)val}");
            break;

            default:
            Debug.Log("Incorrect attempt to change settings from outside");
            break;

        }
    }
/*
    private SettingsSaver GetSettings()
    {
        if (!File.Exists(settingsPath))
                {
                    File.Create(settingsPath).Dispose();

                    return new SettingsSaver();
                }

                using (StreamReader stream = new StreamReader(settingsPath))
                {
                    string json = stream.ReadToEnd();

                    return JsonUtility.FromJson<SettingsSaver>(json);
                }
    }

    public void SaveSettings(SettingsSaver settingsSaver)
    {

        using (StreamWriter stream = new StreamWriter(settingsPath))
                {
                    string json = JsonUtility.ToJson(settingsSaver, true);
                    stream.Write(json);
                }

    }*/

    public void ConvoStatus(bool convo)
    {

        talking = convo;

    }
}
}
   
