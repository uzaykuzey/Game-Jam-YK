using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Control
{
    RightInput,
    LeftInput,
    DownInput,
    Jump,
    Dash,
    Attack,
    Settings
}

public class Controller : MonoBehaviour
{
    public static Controller instance;
    public PlayerMovement player;
    public LayerMask swordLayer;
    public LayerMask groundLayer;
    public LayerMask playerLayer;
    public LayerMask enemyLayer;
    public Camera mainCamera;
    public Sprite explosion;
    public Sprite golem1;
    public Sprite golem2;
    public Sprite archerReady;
    public Sprite archerShooting;
    public Sprite voodooReady;
    public Sprite voodooShooting;
    public SpriteRenderer[] hearts;
    public Sprite fullHeart;
    public Sprite noHeart;
    public Sprite deathTarot;
    public Sprite deathTarotBehind;
    public Sprite circle;
    public Sprite brokenCircle;
    public SpriteRenderer deathTarotRenderer;
    public SpriteRenderer countdown;
    public GameObject cameraChecker;

    public SpriteRenderer bossOverlayRenderer;
    public Sprite normalOverlay;
    public Sprite attackOverlay;
    public static SpriteRenderer settingsRenderer;
    public static SpriteRenderer controlsRenderer;
    public static GameObject settingsStuff;

    public static int deathCondition;

    public AudioClip music;
    public AudioClip teleport;
    public AudioClip golem;
    public AudioClip snowballs;
    public AudioClip realive;
    public AudioClip sword;
    public AudioClip boom;
    public AudioClip laserBlast;
    public AudioClip shield;
    public AudioClip shieldBreak;
    public AudioClip jump;

    public AudioSource MY_MUSIC_PLAYER;

    private List<AudioSource> tempSources;

    private bool[] spinning;
    
    private static Dictionary<Control, KeyCode> controlMap;
    public static bool dontPlayMusic;
    public static bool dontPlaySound;
    public static bool InMenu
    {
        get { return MainMenuAction.settingsOn || MainMenuAction.controlsOn; }
    }


    public void PlayAudio(AudioClip audioClip, bool bigGolem=false)
    {
        if(dontPlaySound)
        {
            return;
        }
        if (tempSources==null)
        {
            tempSources = new();
        }
        for(int i=0;i<tempSources.Count;i++)
        {
            if (tempSources[i].clip == audioClip)
            {
                if (!tempSources[i].isPlaying)
                {
                    tempSources[i].pitch = 1 + (audioClip.name.ToLower() == "small golem sound" ? 0: Random.Range(-0.2f, 0.2f));
                    if(bigGolem)
                    {
                        tempSources[i].pitch = 0.3f;
                    }
                    tempSources[i].Play();
                }
                return;
            }
        }
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.spatialBlend = 0;
        audioSource.clip = audioClip;
        audioSource.pitch = 1;
        if (bigGolem)
        {
            audioSource.pitch = 0.3f;
        }
        audioSource.Play();
        tempSources.Add(audioSource);
    }

    public LayerMask SendEverythingExceptPlayerMask
    {
        get
        {
            return sendEverythingExceptPlayer.forceSendLayers;
        }
    }
    public LayerMask SendEverythingMask
    {
        get
        {
            return sendEverything.forceSendLayers;
        }
    }
    public Vector3 GetClosestCameraPosition(Vector3 pos)
    {
        Vector3 distance = new Vector3(int.MaxValue, int.MaxValue, 0);
        foreach(Vector3 v in cameraPositions)
        {
            if ((v - pos).magnitude < (distance- pos).magnitude)
            {
                distance = v;
            }
        }
        return new Vector3(distance.x, distance.y, mainCamera.transform.position.z);
    }

    private List<Vector3> cameraPositions;
    [SerializeField] private BoxCollider2D sendEverything;
    [SerializeField] private BoxCollider2D sendEverythingExceptPlayer;
    [SerializeField] private GameObject cameraPositionsObject;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        if(controlMap==null)
        {
            InitializeControlMap();
        }
        spinning = new bool[4];

        if (SceneManager.GetActiveScene().name!="Castle")
        {
            return;
        }
        cameraPositions = GetAllChildren(cameraPositionsObject);
        tempSources = new();
    }

    public static void InitializeControlMap()
    {
        controlMap = new();
        controlMap.Add(Control.RightInput, KeyCode.RightArrow);
        controlMap.Add(Control.LeftInput, KeyCode.LeftArrow);
        controlMap.Add(Control.DownInput, KeyCode.DownArrow);
        controlMap.Add(Control.Jump, KeyCode.C);
        controlMap.Add(Control.Attack, KeyCode.Z);
        controlMap.Add(Control.Dash, KeyCode.X);
        controlMap.Add(Control.Settings, KeyCode.Escape);
    }

    List<Vector3> GetAllChildren(GameObject parent)
    {
        List<Vector3> children = new List<Vector3>();
        foreach (Transform child in parent.transform)
        {
            children.Add(child.gameObject.transform.position);
            children.AddRange(GetAllChildren(child.gameObject));
        }
        return children;
    }

    private void Update()
    {
        MY_MUSIC_PLAYER.enabled = !dontPlayMusic;
    }

    private void FixedUpdate()
    {
        float scale = mainCamera.orthographicSize / 8.102688f;
        if(settingsStuff!=null)
        {
            settingsStuff.transform.localScale = new Vector3(scale, scale, scale);
            settingsStuff.transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, settingsStuff.transform.position.z);
        }
        if (SceneManager.GetActiveScene().name != "Boss" && SceneManager.GetActiveScene().name != "Castle")
        {
            return;
        }
        transform.position=mainCamera.transform.position;
        for (int i = 0; i < hearts.Length; i++)
        {
            if(i<player.health && hearts[i].sprite.name == noHeart.name && !spinning[i])
            {
                spinning[i] = true;
                int index = i;
                hearts[i].transform.DORotate(new Vector3(0, 90, 0), 0.15f).onComplete += () =>
                {
                    hearts[index].sprite = fullHeart;
                    hearts[index].transform.DORotate(new Vector3(0, 0, 0), 0.15f).onComplete += () =>
                    {
                        spinning[index] = false;
                    };
                };
            }
            else if(i>=player.health && hearts[i].sprite.name == fullHeart.name && !spinning[i])
            {
                spinning[i] = true;
                int index = i;
                hearts[i].transform.DORotate(new Vector3(0, 90, 0), 0.15f).onComplete += ()=>
                {
                    hearts[index].sprite = noHeart;
                    hearts[index].transform.DORotate(new Vector3(0, 180, 0), 0.15f).onComplete += () =>
                    {
                        spinning[index] = false;
                    };
                };
            }
        }

        if (player.hasDeathCard && deathTarotRenderer.sprite.name == deathTarotBehind.name && !spinning[3])
        {
            spinning[3] = true;
            deathTarotRenderer.transform.DORotate(new Vector3(0, 90, 0), 0.15f).onComplete += () =>
            {
                deathTarotRenderer.sprite = deathTarot;
                deathTarotRenderer.transform.DORotate(new Vector3(0, 0, 0), 0.15f).onComplete += () =>
                {
                    spinning[3] = false;
                };
            };
        }
        else if (!player.hasDeathCard && deathTarotRenderer.sprite.name == deathTarot.name && !spinning[3])
        {
            spinning[3] = true;
            deathTarotRenderer.transform.DORotate(new Vector3(0, 90, 0), 0.15f).onComplete += () =>
            {
                deathTarotRenderer.sprite = deathTarotBehind;
                deathTarotRenderer.transform.DORotate(new Vector3(0, 180, 0), 0.15f).onComplete += () =>
                {
                    spinning[3] = false;
                };
            };
        }

        Vector3 viewPos = mainCamera.WorldToViewportPoint(cameraChecker.transform.position);

        bool isVisible = (viewPos.x > 0 && viewPos.x < 1) &&
                         (viewPos.y > 0 && viewPos.y < 1);

        if(!isVisible)
        {
            mainCamera.transform.position += new Vector3(0, 0, -0.1f);
        }

        if (SceneManager.GetActiveScene().name == "Boss")
        {
            return;
        }

        if (player.transform.position.x <= 100)
        {
            Vector3 v = GetClosestCameraPosition(player.transform.position);
            if (mainCamera.transform.position != v)
            {
                mainCamera.transform.DOMove(v, 1).SetEase(Ease.Linear);
            }
        }
        else if(mainCamera.transform.position.x < 221.327 || player.transform.position.x < 221.327)
        {
            mainCamera.transform.DOMove(new Vector3(player.transform.position.x, mainCamera.transform.position.y, mainCamera.transform.position.z), 0.2f);
        }

        if(player.transform.position.x> 238.05)
        {
            SceneManager.LoadScene("Boss");
        }
    }

    public bool SameRoom(Vector3 pos1, Vector3 pos2)
    {
        Vector3 v1 = GetClosestCameraPosition(pos1);
        Vector3 v2 = GetClosestCameraPosition(pos2);
        return v1 == v2 || (v1.x > 90 && v2.x > 90);
    }

    public static bool GetKey(Control control)
    {
        return controlMap.TryGetValue(control, out KeyCode value) && Input.GetKey(value);
    }

    public static bool GetKeyDown(Control control)
    {
        return controlMap.TryGetValue(control, out KeyCode value) && Input.GetKeyDown(value);
    }

    public static string GetControlSymbol(KeyCode value)
    {
        switch (value)
        {
            case KeyCode.RightArrow:
                return "→";
            case KeyCode.LeftArrow:
                return "←";
            case KeyCode.DownArrow:
                return "↓";
            case KeyCode.Space:
                return "Space";
            case KeyCode.LeftShift:
                return "LShift";
            case KeyCode.UpArrow:
                return "↑";
            case KeyCode.LeftControl:
                return "LCtrl";
            case KeyCode.Escape:
                return "ESC";
            default:
                if (value >= KeyCode.A && value <= KeyCode.Z)
                {
                    return ((char)('A' + (value - KeyCode.A))) + "";
                }
                if (value >= KeyCode.Alpha0 && value <= KeyCode.Alpha9)
                {
                    return ((char)('0' + (value - KeyCode.Alpha0))) + "";
                }
                return "None";
        }
    }

    public static string GetControlSymbol(Control control)
    {
        if(!controlMap.TryGetValue(control, out KeyCode value) || value==KeyCode.None)
        {
            return "None";
        }
        return GetControlSymbol(value);
    }

    public static void SwitchKey(Control control, KeyCode code)
    {
        if(code == KeyCode.Escape || (controlMap.TryGetValue(control, out KeyCode value1) && value1 == code))
        {
            return;
        }
        if(controlMap.ContainsValue(code))
        {
            foreach(Control c in controlMap.Keys)
            {
                if(controlMap.TryGetValue(c, out KeyCode value2) && value2==code)
                {
                    controlMap.Remove(c);
                    controlMap.Add(c, KeyCode.None);
                    break;
                }
            }
        }
        controlMap.Remove(control);
        controlMap.Add(control, code);
    }

}
