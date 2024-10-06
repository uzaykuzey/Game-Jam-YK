using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public bool facingRight;
    public bool real;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (rb.IsTouchingLayers(Controller.instance.groundLayer))
        {
            Destroy(gameObject);
        }

        if (real)
        {
            return;
        }
        rb.velocity = new Vector2(5 * (facingRight ? 1: -1), 0);


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
