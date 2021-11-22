using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum UIStates : byte
{
    Menu,
    InGame,
    Disabled
}


public class CursorInterractionController : MonoBehaviour
{
    public static CursorInterractionController s_instance;
    public UIStates _uiStates;

    [SerializeField] private Button _playBtn;
    [SerializeField] private Button _exitBtn;

    [SerializeField] private Image _playImg;
    [SerializeField] private Image _exitImg;

    [SerializeField] private TextMeshProUGUI _playTmp;
    [SerializeField] private TextMeshProUGUI _exitTmp;
    [SerializeField] private TextMeshProUGUI _scoreTmp;

    [SerializeField] private Button _settingsBtn;

    [SerializeField] private Image[] _hudImages = new Image[2];

    private IEnumerator<WaitForEndOfFrame> StartState()
    {
        _uiStates = UIStates.Menu;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        CameraAttention.s_instance.SetMenuTarget();
        foreach (Image i in _hudImages)
        {
            i.enabled = false;
        }
        yield return new WaitForEndOfFrame();
        Time.timeScale = 0;
    }

    private void DisableInteraction()
    {
        _uiStates = UIStates.InGame;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        CameraAttention.s_instance.SetVikingTarget();
        Time.timeScale = 1;
    }
    
    private void EnableInteraction()
    {
        _uiStates = UIStates.Menu;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        CameraAttention.s_instance.SetMenuTarget();
        Time.timeScale = 0;
    }

    public void HideMenu()
    {
        if(_playTmp.text == "Restart")
            Restart();

        DisableInteraction();

        _playBtn.interactable = false;
        _exitBtn.interactable = false;

        _playImg.enabled = false;
        _exitImg.enabled = false;

        _playTmp.enabled = false;
        _exitTmp.enabled = false;

        _settingsBtn.gameObject.SetActive(false);

        _scoreTmp.enabled = true;

        foreach(Image i in _hudImages)
        {
            i.enabled = true;
        }
    }

    [ContextMenu("ShowMenu")]
    public void ShowMenu()
    {
        EnableInteraction();
        CameraAttention.s_instance.SetMenuTarget();

        _playBtn.interactable = true;
        _exitBtn.interactable = true;

        _playImg.enabled = true;
        _exitImg.enabled = true;

        _playTmp.enabled = true;
        _exitTmp.enabled = true;

        _settingsBtn.gameObject.SetActive(true);
        _scoreTmp.enabled = false;

        foreach (Image i in _hudImages)
        {
            i.enabled = false;
        }
    }
    public void ShowMenuNoFreezing()
    {
        _uiStates = UIStates.Menu;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        _playBtn.interactable = true;
        _exitBtn.interactable = true;

        _playImg.enabled = true;
        _exitImg.enabled = true;

        _playTmp.enabled = true;
        _playTmp.text = "Restart";

        _exitTmp.enabled = true;
        _scoreTmp.enabled = true;
        _settingsBtn.enabled = true;


        foreach (Image i in _hudImages)
        {
            i.enabled = false;
        }
    }
    public void IncrementKilledMutants() => _scoreTmp.text = String.Format("Killed: {0}", Spawner.Respawned);
    public void Exit()
    {
        Application.Quit();
    }
    public void Restart()
    {
        Cleaner.s_instance.CleanSpheares();
        Cleaner.s_instance.CleanMutants();
        Viking.s_instance.DamageVolume = 0;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public event Action onMenuEnter;
    public event Action onMenuExit;

    public void OnMenuEnter()
    {
        if ( onMenuEnter != null)
        {
            onMenuEnter();
        }
    }
    public void OnMenuExit()
    {
        if( onMenuExit != null)
        {
            onMenuExit();
        }
    }

    void OnEnable()
    {
        CameraAttention.s_instance.SetMenuTarget();
    }

    void Awake()
    {
        if (s_instance == null )  
        {
            s_instance = this;
        }
        else if (s_instance != null)
        {
            Destroy(s_instance.gameObject);
        }
    }
    void Start()
    {
        StartCoroutine(StartState());

        onMenuEnter += ShowMenu;
        onMenuExit += HideMenu;

        Spawner.s_instance.onEnemyDeath += IncrementKilledMutants;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) == true)
        {
            switch(_uiStates)
            {
                case UIStates.Menu:
                    HideMenu();
                    break;
                case UIStates.InGame:
                    ShowMenu();
                    break;
            }
        }
    }
}
