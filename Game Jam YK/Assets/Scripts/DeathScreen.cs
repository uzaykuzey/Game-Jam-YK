using UnityEngine;

public class DeathScreen : MonoBehaviour
{
    [SerializeField] private Sprite noDeathCard;
    [SerializeField] private Sprite noKills;
    [SerializeField] private SpriteRenderer spriteRenderer;

    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        spriteRenderer.sprite=(Controller.deathCondition%2 == 0 ? noKills : noDeathCard);
    }
}
