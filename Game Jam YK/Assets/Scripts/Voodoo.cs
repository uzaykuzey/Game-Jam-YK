using DG.Tweening;
using UnityEngine;

public class Voodoo : Enemy
{
    public GameObject projectile;
    public Enemy linked;
    public bool killedLinked;

    private bool fired;

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (youAreAlreadyDead)
        {
            if(!killedLinked)
            {
                killedLinked = true;
                try
                {
                    transform.DOMove(linked.transform.position, 0.5f).onComplete += () =>
                    {
                        linked.Kill();
                    };
                }
                catch { }
            }
            return;
        }
        if(!Controller.instance.SameRoom(Controller.instance.player.transform.position, transform.position))
        {
            return;
        }

        if (Mathf.FloorToInt(Time.time * 5f) % 10 == 0)
        {
            if (!fired)
            {
                fired = true;
                GameObject go = Instantiate(projectile);
                go.GetComponent<Bullet>().real = true;
                go.GetComponent<SpriteRenderer>().enabled = true;
                go.GetComponent<CircleCollider2D>().enabled = true;
                go.GetComponent<Rigidbody2D>().velocity = new Vector2(5+UnityEngine.Random.Range(0, 5), 5+UnityEngine.Random.Range(0, 5));
                go.transform.position = transform.position;
            }
            sr.sprite = Controller.instance.voodooReady;
        }
        else
        {
            fired = false;
            sr.sprite = Controller.instance.voodooShooting;
        }
    }
}
