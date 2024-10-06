using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : Enemy
{
    public GameObject bullet;
    public bool fired;

    protected override void Start()
    {
        base.Start();
        fired = false;
    }


    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (Time.time - timeOfDeath < 0.75f)
        {
            return;
        }
        facingRight = Controller.instance.player.transform.position.x > transform.position.x;

        if(Mathf.FloorToInt(Time.time * 5f) % 10 == 0)
        {
            if(!fired)
            {
                fired = true;
                GameObject go = Instantiate(bullet);
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

    public override void Destroy()
    {
        Destroy(bullet);
        base.Destroy();
    }
}
