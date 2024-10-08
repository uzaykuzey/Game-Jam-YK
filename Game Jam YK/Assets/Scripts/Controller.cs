using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public void PlayAudio(AudioClip audioClip, bool bigGolem=false)
    {
        if(tempSources==null)
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
        spinning = new bool[4];

        if (SceneManager.GetActiveScene().name=="Boss")
        {
            return;
        }
        cameraPositions = GetAllChildren(cameraPositionsObject);
        tempSources = new();
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

    private void FixedUpdate()
    {
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
}
