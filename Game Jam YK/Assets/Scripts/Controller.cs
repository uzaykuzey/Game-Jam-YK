using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

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
    public SpriteRenderer[] hearts;
    public Sprite fullHeart;
    public Sprite noHeart;
    public Sprite deathTarot;
    public Sprite deathTarotBW;
    public SpriteRenderer deathTarotRenderer;

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
        cameraPositions = GetAllChildren(cameraPositionsObject);
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
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].sprite = i < player.health ? fullHeart : noHeart;
        }
        deathTarotRenderer.sprite = player.hasDeathCard ? deathTarot : deathTarotBW;
        Vector3 v = GetClosestCameraPosition(player.transform.position);
        if (mainCamera.transform.position != v)
        {
            mainCamera.transform.DOMove(v, 1).SetEase(Ease.Linear);
        }
    }
}
