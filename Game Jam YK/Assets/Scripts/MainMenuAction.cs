using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public enum MenuAction
{
    StartGame,
    ExitGame,
    ReturnToMainMenu,
    OpenSettings,
    OpenControls,
    Music,
    Sound,
    ChangeRight,
    ChangeLeft,
    ChangeDown,
    ChangeJump,
    ChangeDash,
    ChangeAttack,
    Back,
    Retry
}

public class MainMenuAction : MonoBehaviour
{
    public MenuAction type;
    [SerializeField] private GameObject settingsStuff;
    public static bool settingsOn;
    public static bool controlsOn;
    public static bool dontDestroyDone = false;

    public static List<BoxCollider2D> settingsColliders;
    public static List<BoxCollider2D> controlColliders;
    public static TextMeshProUGUI[] texts;

    private static float defaultDeltaTime=-1;
    private static float counter;

    private static MenuAction LastAction;

    private void Start()
    {
        if(defaultDeltaTime==-1)
        {
            defaultDeltaTime = Time.deltaTime;
            print(defaultDeltaTime);
        }
        if(settingsStuff==null)
        {
            return;
        }
        if (!dontDestroyDone)
        {
            dontDestroyDone = true;
            settingsColliders = new();
            controlColliders = new();
            texts = new TextMeshProUGUI[7];
            DontDestroyOnLoad(settingsStuff);
        }
        else
        {
            Destroy(settingsStuff);
            return;
        }
        Controller.settingsStuff=settingsStuff;
        foreach (Transform child in settingsStuff.transform)
        {
            if (child.name == "Settings")
            {
                Controller.settingsRenderer = child.GetComponent<SpriteRenderer>();
                foreach (Transform t in child.transform)
                {
                    try
                    {
                        settingsColliders.Add(t.GetComponent<BoxCollider2D>());
                    } 
                    catch { };
                }
            }
            else if (child.name == "Controls")
            {
                Controller.controlsRenderer = child.GetComponent<SpriteRenderer>();
                foreach (Transform t in child.transform)
                {
                    try
                    {
                        controlColliders.Add(t.GetComponent<BoxCollider2D>());
                    }
                    catch { };
                }
            }
            else if(child.name=="Canvas")
            {
                foreach (Transform t in child.transform)
                {
                    if (int.TryParse(t.name, out int value))
                    {
                        print(t.name);
                        texts[value] = t.GetComponent<TextMeshProUGUI>();
                    }
                }
            }
            try
            {
                child.GetComponent<SpriteRenderer>().enabled = false;
            }
            catch { };
        }
        Controller.InitializeControlMap();
        CloseEverything();
    }

    private void Update()
    {
        if(type==MenuAction.StartGame || type==MenuAction.Retry)
        {
            if(Controller.GetKey(Control.Jump))
            {
                OnMouseDown();
            }
        }
        if (Controller.settingsRenderer == null)
        {
            return;
        }
        Controller.settingsRenderer.enabled = settingsOn;
        Controller.controlsRenderer.enabled = controlsOn;
        for(int i= 0;i < texts.Length;i++)
        {
            if (!controlsOn)
            {
                texts[i].enabled = false;
                continue;
            }
            texts[i].enabled = true;
            texts[i].text = (LastAction - MenuAction.ChangeRight == i && LastAction <= MenuAction.ChangeAttack) ? "...": controlsOn ? Controller.GetControlSymbol((Control) i) + "" : " ";
        }

        if (LastAction >= MenuAction.ChangeRight && LastAction <= MenuAction.ChangeAttack)
        {
            if (Input.anyKey)
            {
                foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
                {
                    if(Controller.GetControlSymbol(key)=="None")
                    {
                        continue;
                    }
                    if (Input.GetKey(key))
                    {
                        Controller.SwitchKey((Control) (LastAction - MenuAction.ChangeRight), key);
                        LastAction = MenuAction.OpenControls;
                        break;
                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape) && counter> 1)
        {
            counter = 0;
            if (Controller.InMenu)
            {
                CloseEverything();
            }
            else
            {
                OpenSettings();
            }
        }

        Time.timeScale = Controller.InMenu ? 0f : 1f;
        counter+=defaultDeltaTime / 10f;
    }

    private static void OpenSettings()
    {
        CloseEverything();
        settingsOn = true;
        foreach (BoxCollider2D b in settingsColliders)
        {
            b.enabled = true;
        }
    }

    private static void OpenControls()
    {
        CloseEverything();
        controlsOn = true;
        foreach (BoxCollider2D b in controlColliders)
        {
            b.enabled = true;
        }
    }


    public static void CloseEverything()
    {
        settingsOn = false;
        controlsOn = false;
        LastAction = MenuAction.Back;
        foreach (BoxCollider2D b in settingsColliders)
        {
            b.enabled = false;
        }
        foreach (BoxCollider2D b in controlColliders)
        {
            b.enabled = false;
        }
    }

    private void OnMouseDown()
    {
        LastAction = type;

        if(settingsOn)
        {
            switch (type)
            {
                case MenuAction.Music:
                    Controller.dontPlayMusic = !Controller.dontPlayMusic;
                    break;
                case MenuAction.Sound:
                    Controller.dontPlaySound= !Controller.dontPlaySound;
                    break;
                case MenuAction.ReturnToMainMenu:
                    CloseEverything();
                    SceneManager.LoadScene("Main Menu");
                    break;
                case MenuAction.Back:
                    CloseEverything();
                    break;
                case MenuAction.OpenControls:
                    OpenControls();
                    break;
            }
        }
        else if(controlsOn)
        {
            if(type==MenuAction.Back)
            {
                OpenSettings();
            }
        }
        else
        {
            switch (type)
            {
                case MenuAction.StartGame:
                    SceneManager.LoadScene("Cutscene");
                    break;
                case MenuAction.ExitGame:
                    Application.Quit();
                    break;
                case MenuAction.ReturnToMainMenu:
                    SceneManager.LoadScene("Main Menu");
                    break;
                case MenuAction.OpenSettings:
                    OpenSettings();
                    break;
                case MenuAction.Retry:
                    SceneManager.LoadScene(Controller.deathCondition/2 == 0 ? "Castle": "Boss");
                    break;
            }
        }

    }
}
