using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : Enemy
{
    public GameObject bullet;
    public bool fired;
    public bool voodoo;

    protected override void Start()
    {
        base.Start();
        fired = false;
    }


    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (youAreAlreadyDead)
        {
            return;
        }
        if(!Controller.instance.SameRoom(Controller.instance.player.transform.position, transform.position))
        {
            return;
        }
        facingRight = Controller.instance.player.transform.position.x > transform.position.x;

        if(Mathf.FloorToInt(Time.time * (voodoo ? 10f: 5f)) % 10 == 0)
        {
            if(!fired)
            {
                fired = true;
                GameObject go = Instantiate(bullet);
                go.GetComponent<Bullet>().enabled = true;
                go.GetComponent<Bullet>().real = false;
                go.GetComponent<SpriteRenderer>().enabled = true;
                go.GetComponent<BoxCollider2D>().enabled = true;
                go.GetComponent<Bullet>().facingRight = facingRight;
                go.transform.position = transform.position;
            }
            sr.sprite = Controller.instance.archerShooting;
        }
        else
        {
            fired = false;
            sr.sprite = Controller.instance.archerReady;
        }

        
    }
}
