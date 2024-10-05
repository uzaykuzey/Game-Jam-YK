using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public static Controller instance;
    public PlayerMovement player;
    public LayerMask swordLayer;
    public LayerMask groundLayer;
    public LayerMask playerLayer;
    public LayerMask enemyLayer;
    public Camera camera;
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
    public Vector3 GetClosestCameraPosition
    {
        get 
        {
            Vector3 distance = new Vector3(int.MaxValue, int.MaxValue, 0);
            foreach(Vector3 v in cameraPositions)
            {
                if ((v - player.transform.position).magnitude < (distance-player.transform.position).magnitude)
                {
                    distance = v;
                }
            }
            return new Vector3(distance.x, distance.y, camera.transform.position.z);
        }
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
        Vector3 v = GetClosestCameraPosition;
        if (camera.transform.position != v)
        {
            camera.transform.DOMove(v, 1).SetEase(Ease.Linear);
        }
    }
}
