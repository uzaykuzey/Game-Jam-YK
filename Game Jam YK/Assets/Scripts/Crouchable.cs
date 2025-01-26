using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Crouchable : MonoBehaviour
{

    private CompositeCollider2D tCollider;

    public float yPos;

    private void Start()
    {
        tCollider=GetComponent<CompositeCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Controller.InMenu)
        {
            return;
        }
        if (Controller.instance.player.transform.position.y > yPos && !(Controller.GetKey(Control.DownInput)))
        {
            tCollider.forceSendLayers = Controller.instance.SendEverythingMask;
        }
        else
        {
            tCollider.forceSendLayers = Controller.instance.SendEverythingExceptPlayerMask;
        }
    }



}
