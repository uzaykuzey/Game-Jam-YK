using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health;
    public bool facingRight;
    public bool knockBackRes;

    protected Rigidbody2D rb;
    protected SpriteRenderer sr;
    protected BoxCollider2D bc;
    protected float timeOfGotHit;
    protected float timeOfDeath;
    public bool youAreAlreadyDead;
    

    // Start is called before the first frame update
    protected virtual void Start()
    {
        rb=GetComponent<Rigidbody2D>();
        sr=GetComponent<SpriteRenderer>();
        bc=GetComponent<BoxCollider2D>();
        timeOfGotHit = -10;
        timeOfDeath = -10;
        youAreAlreadyDead = false;
    }

    public bool Invulnarable()
    {
        return Time.time - timeOfGotHit < 0.35f;
    }

    public void Kill()
    {
        if (PlayerMovement.STOP && bc.IsTouchingLayers(Controller.instance.playerLayer))
        {
            Controller.instance.player.KilledEnemyWhileDead++;
        }
        timeOfDeath = Time.time;
        bc.enabled = false;
        rb.gravityScale = 0;
        youAreAlreadyDead = true;
    }

    // Update is called once per frame
    protected virtual void FixedUpdate()
    {
        if ((health <= 0 || (PlayerMovement.STOP && bc.IsTouchingLayers(Controller.instance.playerLayer))) && !youAreAlreadyDead)
        {
            Kill();
        }
        if (Time.time - timeOfDeath > 0.75f && youAreAlreadyDead)
        {
            Destroy(gameObject);
        }

        if (Time.time - timeOfDeath < 0.75f)
        {
            rb.velocity = new Vector2(0, 0);
            sr.sprite = Controller.instance.explosion;
            sr.color = Color.white;
            return;
        }

        if(bc.IsTouchingLayers(Controller.instance.swordLayer) && !Invulnarable())
        {
            health--;
            timeOfGotHit = Time.time;
            if(!knockBackRes)
            {
                rb.velocity = new Vector2((Controller.instance.player.LookingRight ? 1 : -1) * 2, 2);
            }
            Controller.instance.player.DownStroke();
        }

        if(Invulnarable())
        {
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, Mathf.FloorToInt(Time.time*10) % 2 == 1 ? 0.4f: 1);
        }
        else
        {
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b);
        }

        if (facingRight)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }
}
