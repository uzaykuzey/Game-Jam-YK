using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathCard : MonoBehaviour
{
    private Vector2 basePosition;
    private BoxCollider2D bc;
    // Start is called before the first frame update
    void Start()
    {
        bc=GetComponent<BoxCollider2D>();
        basePosition = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = basePosition + new Vector2( 0, Mathf.Sin(Time.time*Mathf.PI) * 0.3f);
        transform.rotation = Quaternion.Euler(0, (Time.time / 5f - Mathf.FloorToInt(Time.time / 5f)) * 360, 0);

        if(bc.IsTouchingLayers(Controller.instance.playerLayer) && !Controller.instance.player.hasDeathCard && Controller.instance.player.living)
        {
            Controller.instance.player.hasDeathCard = true;
            Destroy(gameObject);
        }
    }
}
