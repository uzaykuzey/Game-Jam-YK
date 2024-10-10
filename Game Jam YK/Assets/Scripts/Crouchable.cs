using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Crouchable : MonoBehaviour
{

    private static float timeOfCrouch;

    private CompositeCollider2D tCollider;

    public float yPos;

    private void Start()
    {
        tCollider=GetComponent<CompositeCollider2D>();
        timeOfCrouch = -10;
    }

    // Update is called once per frame
    void Update()
    {
        if (Controller.InMenu)
        {
            return;
        }
        if (Input.GetKey(KeyCode.DownArrow) && Time.time - timeOfCrouch >= 0.5)
        {
            timeOfCrouch = Time.time;
        }
        if(Controller.instance.player.transform.position.y > yPos && !(Time.time-timeOfCrouch < 0.5 && Time.time - timeOfCrouch>0.2 && Input.GetKey(KeyCode.DownArrow)))
        {
            tCollider.forceSendLayers = Controller.instance.SendEverythingMask;
        }
        else
        {
            tCollider.forceSendLayers = Controller.instance.SendEverythingExceptPlayerMask;
        }
    }



}
