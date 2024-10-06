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

    protected override void Start()
    {
        base.Start();
        activatedShieldYet = false;
        startTime = Time.time;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if(health<=5 && !activatedShieldYet)
        {
            activatedShieldYet=true;
            hasShield = true;
        }

        if(health<=0)
        {
            SceneManager.LoadScene("Ending");
        }

        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if(obj.name.ToLower().Contains("card"))
            {
                return;
            }
        }

        if (Mathf.FloorToInt(Time.time - startTime)%20==5 && !Controller.instance.player.hasDeathCard)
        {
            if(spawnOnce)
            {
                spawnOnce = false;
                Vector3 pos = new Vector3(Random.Range(-13.28f, 13.28f), Random.Range(-5.74f, 5.74f), -3);
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
