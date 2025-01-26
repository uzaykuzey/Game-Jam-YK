using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health;
    public bool facingRight;
    public bool knockBackRes;
    public bool hasShield;
    public SpriteRenderer shieldRenderer;

    protected Rigidbody2D rb;
    protected SpriteRenderer sr;
    protected BoxCollider2D bc;
    protected float timeOfGotHit;
    protected float timeOfDeath;
    public bool youAreAlreadyDead;
    private float timeOfShowingShield;

    public static Color shieldColor = new(0.6379494f, 0.9056604f, 0.8986688f, 0.5294118f);
    

    // Start is called before the first frame update
    protected virtual void Start()
    {
        rb=GetComponent<Rigidbody2D>();
        sr=GetComponent<SpriteRenderer>();
        bc=GetComponent<BoxCollider2D>();
        timeOfGotHit = -10;
        timeOfDeath = -10;
        youAreAlreadyDead = false;
        foreach (Transform child in transform)
        {
            if (child.gameObject.name.ToLower().Equals("shield"))
            {
                shieldRenderer=child.GetComponent<SpriteRenderer>();
            }
        }
        if(shieldRenderer!=null)
        {
            shieldRenderer.enabled = false;
            shieldRenderer.color = shieldColor;
            shieldRenderer.transform.position = new Vector3(shieldRenderer.transform.position.x, shieldRenderer.transform.position.y, -0.51f);
        }

        timeOfShowingShield = -10;
    }

    public bool Invulnarable()
    {
        return Time.time - timeOfGotHit < 0.35f;
    }

    public void Kill()
    {
        timeOfDeath = Time.time;
        bc.enabled = false;
        rb.gravityScale = 0;
        youAreAlreadyDead = true;
        Controller.instance.PlayAudio(Controller.instance.boom);
    }

    // Update is called once per frame
    protected virtual void FixedUpdate()
    {
        if(!youAreAlreadyDead)
        {
            if (health <= 0)
            {
                Kill();
            }
            if((PlayerMovement.STOP && bc.IsTouchingLayers(Controller.instance.playerLayer)))
            {
                Controller.instance.player.KilledEnemyWhileDead++;
                if (hasShield)
                {
                    timeOfShowingShield = Time.time;
                    hasShield = false;
                    Controller.instance.PlayAudio(Controller.instance.shieldBreak);
                }
                else if(Time.time - timeOfShowingShield > 0.5f && gameObject.name!="Boss")
                {
                    Kill();
                }
            }
        }
        if(!PlayerMovement.STOP && bc.IsTouchingLayers(Controller.instance.playerLayer) && Controller.instance.player.living && gameObject.name.ToLower().Contains("golem"))
        {
            Controller.instance.PlayAudio(Controller.instance.golem, gameObject.name.ToLower().Contains("big"));
        }
        if (shieldRenderer != null)
        {
            shieldRenderer.enabled = Time.time - timeOfShowingShield < 0.5f;
            shieldRenderer.sprite = hasShield ? Controller.instance.circle : Controller.instance.brokenCircle;
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
            if(hasShield)
            {
                timeOfShowingShield = Time.time;
                Controller.instance.PlayAudio(Controller.instance.shield);
            }
            if (!hasShield)
            {
                health--;
                timeOfGotHit = Time.time;
                if (!knockBackRes)
                {
                    rb.velocity = new Vector2((Controller.instance.player.LookingRight ? 1 : -1) * 2, 2);
                }
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
