using UnityEngine;

public class Golem : Enemy
{
    public BoxCollider2D bottomCheck;
    public BoxCollider2D frontCheck;

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if(youAreAlreadyDead)
        {
            return;
        }

        while(transform.position.y < -28)
        {
            transform.position += new Vector3(0, 1, 0);
        }

        if (!Controller.instance.SameRoom(Controller.instance.player.transform.position, transform.position))
        {
            return;
        }

        if (GetComponent<BoxCollider2D>().IsTouchingLayers(Controller.instance.groundLayer) && !PlayerMovement.STOP)
        {
            if (frontCheck.IsTouchingLayers(Controller.instance.groundLayer) || frontCheck.IsTouchingLayers(Controller.instance.enemyLayer) || !bottomCheck.IsTouchingLayers(Controller.instance.groundLayer) || !Controller.instance.SameRoom(transform.position, frontCheck.transform.position))
            {
                facingRight = !facingRight;
            }

            if (Time.time - timeOfGotHit > 0.5f || knockBackRes)
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
