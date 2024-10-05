using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public static bool STOP;
    

    public int force;
    public int jumpForce;
    public int maxSpeed;
    public BoxCollider2D groundChecker;
    public GameObject hitObjectFront; 
    public Sprite idleSprite;
    public Sprite runningSprite1;
    public Sprite runningSprite2;
    public Sprite idleAttack1;
    public Sprite idleAttack2;
    public Sprite runningAttack1;
    public Sprite runningAttack2;
    public Sprite deadSprite;
    public GameObject corpse; 
    public GameObject connection;
    public SpriteRenderer countdown;
    public Sprite[] numberSprites;
    public SpriteRenderer[] hearts;
    public Sprite fullHeart;
    public Sprite noHeart;
    public int health;
    public string currentScene;
    public GameObject center;

    private Rigidbody2D rb;
    private BoxCollider2D bc;
    private SpriteRenderer sr;
    private float baseGravity;
    private float timeOfC;
    private float timeOfDash;
    private float timeOfAttack;
    private bool downAttack;
    private bool dashRight;
    private bool canDash;
    private float timeOfGotHit;
    private bool living;
    private Vector2 corpsePos;
    private Vector2 corpseCenter;
    private float timeOfDeath;

    public int KilledEnemyWhileDead { get; set; }

    public bool LookingRight { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        bc=GetComponent<BoxCollider2D>();
        baseGravity = rb.gravityScale;
        timeOfC = -1;
        timeOfDash = -10;
        timeOfAttack = -10;
        timeOfGotHit = -10;
        LookingRight = true;
        canDash = true;
        living = true;
    }

    public bool Invulnarable()
    {
        return Time.time - timeOfGotHit < 0.65f || !living;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.C) || Input.GetKey(KeyCode.Space))
        {
            timeOfC = Time.time;
        }
        rb.gravityScale = Mathf.Sign(rb.velocity.y) == -1 ? baseGravity * 2 : Input.GetKey(KeyCode.C) ? baseGravity * 0.75f : baseGravity;

        if (!Dashing() && Input.GetKey(KeyCode.X) && canDash && Time.time - timeOfDash > 0.5f)
        {
            timeOfDash = Time.time;
            dashRight = LookingRight;
            canDash = false;
        }

        if (!Attacking() && Input.GetKey(KeyCode.Z) && Time.time - timeOfAttack > 0.5f && living)
        {
            timeOfAttack=Time.time;
            downAttack = Input.GetKey(KeyCode.DownArrow);
            if(!downAttack)
            {
                rb.velocity += new Vector2((LookingRight ? -1 : 1) * 5, 0);
            }

        }
    }

    public bool Dashing()
    {
        return Time.time - timeOfDash < 0.099f;
    }

    public bool Attacking()
    {
        return Time.time - timeOfAttack < 0.2f;
    }

    private void FixedUpdate()
    {
        Sprite currentSprite=idleSprite;
        bool walking = false ;
        if(health<=0)
        {
            if(living)
            {
                living = false;
                corpsePos = transform.position;
                corpseCenter = center.transform.position;
                timeOfDeath=Time.time;
                if (LookingRight)
                {
                    corpse.transform.rotation = Quaternion.Euler(0, 0, 0);
                }
                else
                {
                    corpse.transform.rotation = Quaternion.Euler(0, 180, 0);
                }
            }
            corpse.transform.position= corpsePos;
            corpse.GetComponent<SpriteRenderer>().sprite = idleSprite;
        }
        

        corpse.GetComponent<SpriteRenderer>().enabled = !living;
        countdown.enabled = !living;
        if(!living && Time.time-timeOfDeath>=5 && !STOP)
        {
            KilledEnemyWhileDead = 0;
            STOP = true;
            var moving = transform.DOMove(corpsePos, (new Vector2(center.transform.position.x, center.transform.position.y) - corpseCenter).magnitude * 0.05f).SetEase(Ease.InOutBack);
            moving.onComplete+= () =>
            {
                STOP = false;
                if (KilledEnemyWhileDead>0)
                {
                    health = KilledEnemyWhileDead;
                    living = true;
                    timeOfGotHit= Time.time;
                }
                else
                {
                    SceneManager.LoadScene(currentScene);
                }
            };
        }
        if(!living)
        {
            try
            {
                countdown.sprite = numberSprites[4 - Mathf.FloorToInt(Time.time - timeOfDeath)];
            }
            catch(IndexOutOfRangeException _)
            {
                countdown.enabled = false;
            }
            
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            rb.AddForce(new Vector2(force, 0));
            LookingRight = true;
            walking = true;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            rb.AddForce(new Vector2(-force, 0));
            LookingRight = false;
            walking = true;
        }
        else
        {
            if (Mathf.Abs(rb.velocity.x) < 0.5f)
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
            else
            {
                rb.AddForce(new Vector2(-Mathf.Sign(rb.velocity.x) * force, 0));
            }
        }

        if(walking)
        {
            currentSprite = Mathf.FloorToInt(Time.time * 5) % 2 == 0 ? this.runningSprite1 : this.runningSprite2;
        }

        if (groundChecker.IsTouchingLayers(Controller.instance.groundLayer) && !Crouchable.Crouching())
        {
            if (Time.time - timeOfC < 0.25f)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }
            canDash = true;
        }



        if (Mathf.Abs(rb.velocity.x) > maxSpeed)
        {
            rb.velocity = new Vector2 ( Mathf.Sign(rb.velocity.x) * maxSpeed, rb.velocity.y);
        }

        if (bc.IsTouchingLayers(Controller.instance.enemyLayer) && !Invulnarable())
        {
            health--;
            timeOfGotHit = Time.time;
            rb.velocity = new Vector2((LookingRight ? -1 : 1) * maxSpeed*2, jumpForce*0.9f);
            timeOfDash = Time.time - 0.1f;
        }

        if(bc.IsTouchingLayers(Controller.instance.groundLayer))
        {
            timeOfC = -10;
        }

        if (Dashing())
        {
            rb.velocity = new Vector2(20 * (dashRight ? 1: -1), 0);
            LookingRight = dashRight;
        }
        
        if(!Attacking())
        {
            if (LookingRight)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }
        hitObjectFront.GetComponent<BoxCollider2D>().enabled = Attacking() && !downAttack;
        hitObjectFront.GetComponent<SpriteRenderer>().enabled = Attacking() && !downAttack;

        if(Attacking())
        {
            if(walking)
            {
                currentSprite = (Mathf.FloorToInt((Time.time - timeOfAttack) * 10) % 2 == 0) ? runningAttack1 : runningAttack2;
            }
            else
            {
                currentSprite = (Mathf.FloorToInt((Time.time - timeOfAttack) * 10) % 2 == 0) ? idleAttack1 : idleAttack2;
            }
        }

        if (Invulnarable() && living)
        {
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, Mathf.FloorToInt(Time.time * 5) % 2 == 1 ? 0.5f : 1);
        }
        else
        {
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b);
        }

        sr.sprite = living ? currentSprite : deadSprite;

        connection.GetComponent<SpriteRenderer>().enabled = !living;
        if(!living)
        {
            Vector2 pos = center.transform.position;
            float angle = Mathf.Atan2(pos.y - corpseCenter.y, pos.x - corpseCenter.x);
            connection.transform.localScale = new Vector3(connection.transform.localScale.x, (pos- corpseCenter).magnitude, connection.transform.localScale.z);
            connection.transform.SetPositionAndRotation((pos + corpseCenter) / 2f, Quaternion.Euler(0, 0, angle*180f/Mathf.PI + 90f));
        }

        for(int i=0; i<hearts.Length; i++)
        {
            hearts[i].sprite = i < health ? fullHeart : noHeart;
        }
    }

    public void DownStroke()
    {
        if(downAttack)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce/2f);
            canDash = true;
        }

    }
}
