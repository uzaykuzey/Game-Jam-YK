using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossScript : Enemy
{
    private bool activatedShieldYet;
    private float startTime;
    private bool spawnOnce = false;
    [SerializeField] private GameObject deathCard;
    
    private float shieldRegenTime;
    private int neededTimeToPass;

    protected override void Start()
    {
        base.Start();
        activatedShieldYet = false;
        startTime = Time.time;
        shieldRegenTime = int.MaxValue;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if(health<=8 && !activatedShieldYet)
        {
            activatedShieldYet=true;
            hasShield = true;
            shieldRegenTime = Time.time;
            neededTimeToPass = Random.Range(15,20);
            GetComponent<BossActions>().phase = 2;
        }

        if(health<=0)
        {
            SceneManager.LoadScene("Ending");
        }

        if(sr.sprite.name == "Last_boss_standing")
        {
            Controller.instance.bossOverlayRenderer.sprite = Controller.instance.normalOverlay;
        }
        else if(sr.sprite.name == "Last_boss_attack")
        {
            Controller.instance.bossOverlayRenderer.sprite = Controller.instance.attackOverlay;
        }
        

        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if(obj.name.ToLower().Contains("card"))
            {
                return;
            }
        }

        if (Time.time-shieldRegenTime > neededTimeToPass && !Controller.instance.player.hasDeathCard && hasShield)
        {
            shieldRegenTime = int.MaxValue;
            if (spawnOnce)
            {
                spawnOnce = false;
                Vector3 pos;
                do
                {
                    pos = new Vector3(Random.Range(-13.28f, 13.28f), Random.Range(-5.74f, 5.74f), -3);
                }
                while ((Controller.instance.player.transform.position - pos).magnitude < 5);
                GameObject o = Instantiate(deathCard);
                o.transform.position = pos;
            }
            
        }
        else
        {
            spawnOnce=true;
        }
    }
}
