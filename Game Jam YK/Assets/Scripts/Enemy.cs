using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health;
    public bool moving;
    public BoxCollider2D rightCheck;
    public BoxCollider2D frontCheck;
    public bool facingRight;
    public int force;
    public Sprite explosionSprite;


    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private BoxCollider2D bc;
    private float timeOfGotHit;
    private float timeOfDeath;
    private bool youAreAlreadyDead;
    

    // Start is called before the first frame update
    void Start()
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

    // Update is called once per frame
    void FixedUpdate()
    {
        if((health <= 0 || (PlayerMovement.STOP && bc.IsTouchingLayers(Controller.instance.playerLayer))) && !youAreAlreadyDead)
        {
            if(PlayerMovement.STOP && bc.IsTouchingLayers(Controller.instance.playerLayer))
            {
                Controller.instance.player.KilledEnemyWhileDead++;
            }
            timeOfDeath = Time.time;
            bc.enabled = false;
            rb.gravityScale = 0;
            youAreAlreadyDead = true;
        }

        if (Time.time - timeOfDeath > 0.75f && youAreAlreadyDead)
        {
            Destroy(gameObject);
        }

        if (Time.time - timeOfDeath < 0.75f)
        {
            rb.velocity = new Vector2(0, 0);
            sr.sprite = explosionSprite;
            sr.color = Color.white;
            return;
        }

        if(bc.IsTouchingLayers(Controller.instance.swordLayer) && !Invulnarable())
        {
            health--;
            timeOfGotHit = Time.time;
            rb.velocity = new Vector2((Controller.instance.player.LookingRight ? 1 : -1) * 2, 2);
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

        if(moving && GetComponent<BoxCollider2D>().IsTouchingLayers(Controller.instance.groundLayer) && !PlayerMovement.STOP)
        {
            if(frontCheck.IsTouchingLayers(Controller.instance.groundLayer) || !rightCheck.IsTouchingLayers(Controller.instance.groundLayer))
            {
                facingRight = !facingRight;
            }

            if(Time.time - timeOfGotHit > 0.5f)
            {
                rb.velocity = (new Vector2(3 * (facingRight ? 1 : -1), rb.velocity.y));
            }

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
