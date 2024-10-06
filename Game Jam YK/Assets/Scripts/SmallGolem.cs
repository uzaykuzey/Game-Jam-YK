using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallGolem : Enemy
{
    public BoxCollider2D bottomCheck;
    public BoxCollider2D frontCheck;
    public int force;

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if(Time.time - timeOfDeath < 0.75f)
        {
            return;
        }

        if (GetComponent<BoxCollider2D>().IsTouchingLayers(Controller.instance.groundLayer) && !PlayerMovement.STOP)
        {
            if (frontCheck.IsTouchingLayers(Controller.instance.groundLayer) || frontCheck.IsTouchingLayers(Controller.instance.enemyLayer) || !bottomCheck.IsTouchingLayers(Controller.instance.groundLayer))
            {
                facingRight = !facingRight;
            }

            if (Time.time - timeOfGotHit > 0.5f)
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

        sr.sprite=Mathf.FloorToInt(Time.time * 3)%2 == 0 ? Controller.instance.golem1: Controller.instance.golem2;
    }
}
