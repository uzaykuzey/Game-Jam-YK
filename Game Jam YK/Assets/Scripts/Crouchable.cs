using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Crouchable : MonoBehaviour
{

    private static float timeOfCrouch;

    private CompositeCollider2D tCollider;

    public int yPos;

    private void Start()
    {
        tCollider=GetComponent<CompositeCollider2D>();
        timeOfCrouch = -10;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.C) && Input.GetKey(KeyCode.DownArrow))
        {
        
            timeOfCrouch = Time.time;
        }
        if(!Crouching() && Controller.instance.player.transform.position.y > yPos)
        {
            tCollider.forceSendLayers = Controller.instance.SendEverythingMask;
        }
        else
        {
            tCollider.forceSendLayers = Controller.instance.SendEverythingExceptPlayerMask;
        }
    }

    public static bool Crouching()
    {
        return Time.time - timeOfCrouch < 0.5f;
    }

}
