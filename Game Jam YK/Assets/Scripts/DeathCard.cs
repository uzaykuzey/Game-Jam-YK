using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathCard : MonoBehaviour
{
    private Vector2 basePosition;
    private BoxCollider2D bc;
    private SpriteRenderer sr;
    // Start is called before the first frame update
    void Start()
    {
        bc=GetComponent<BoxCollider2D>();
        sr=GetComponent<SpriteRenderer>();
        basePosition = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = basePosition + new Vector2( 0, Mathf.Sin(Time.time*Mathf.PI) * 0.3f);
        float rot = (Time.time / 5f - Mathf.FloorToInt(Time.time / 5f)) * 360;
        transform.rotation = Quaternion.Euler(0, rot, 0);
        if(rot<=90 || rot>=270)
        {
            sr.sprite = Controller.instance.deathTarot;
        }
        else
        {
            sr.sprite = Controller.instance.deathTarotBehind;
        }

        if(bc.IsTouchingLayers(Controller.instance.playerLayer) && !Controller.instance.player.hasDeathCard && Controller.instance.player.living)
        {
            Controller.instance.player.hasDeathCard = true;
            Destroy(gameObject);
        }
    }
}
