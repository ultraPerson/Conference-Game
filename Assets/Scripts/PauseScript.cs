using System.Collections;
using System.Collections.Generic;
using System.IO;
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
    private Slider mouseS;
    private HorizontalSelector charList;
    
    
    private int charInd = 0;
    private string currentScreen;
    [SerializeField] private bool isMainMenu = false;
    public bool isPaused 
    {get; private set;}
    private bool talking = false;
    
    void Start(){

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
        visorCanvas = GameObject.Find("/Main Camera/VisorCanvas");
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
        mouseS.value = 25;

        charList.index = charInd;
        
        
        
        

        charList.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(_updateSettings);
        charList.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(_updateSettings);
        menuPannels.transform.GetChild(1).GetChild(0).GetChild(1).GetComponent<Button>().onClick.AddListener(_closePause);
        //pauseUI.transform.GetChild(0).GetChild(3).GetComponent<Button>().onClick.AddListener(_settings);
        //settingsObj.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(_help);
        
        
        IntroScreen();

    }

    public void IntroScreen()
    {

        OpenPause();
        menuPannels.transform.GetChild(0).GetChild(2).GetComponent<Button>().onClick.Invoke();
        
        currentScreen = "Help";
        
        
        
        
        
        Time.timeScale = 0;

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
        Time.timeScale = 0;
        isPaused = true;
        visorCanvas.SetActive(false);
        
       
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
   
