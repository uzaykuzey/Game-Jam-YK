using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class BossActions : MonoBehaviour
{
    [SerializeField] private float cycleLengthX = 2;
    [SerializeField] private float HorizontalPos = 10;
    [SerializeField] public float[] platformY;
    [SerializeField] public float Xleft;
    [SerializeField] public float Xright;
    [SerializeField] public float phase;
    [SerializeField] public BossBullet bullet;
    [SerializeField] public Laser[] lasers;
    [SerializeField] public BoxCollider2D[] laserColliders;
    [SerializeField] public Animator[] animators;
    // Start is called before the first frame update
    void Start()
    {
        bullet.GetComponent<CircleCollider2D>().enabled = false;
        bullet.GetComponent<SpriteRenderer>().enabled = false;
    }

    public void Teleport()
    {
        Controller.instance.PlayAudio(Controller.instance.teleport);
        float targetX = UnityEngine.Random.Range(Xleft, Xright);
        float targetY = platformY[UnityEngine.Random.Range(0, platformY.Length)];
        transform.position = new Vector3(targetX, targetY, 0);
    }

    public void SpawnBullet()
    {
        Controller.instance.PlayAudio(Controller.instance.snowballs);
        for (int i=0;i<3;i++)
        {
            BossBullet b = Instantiate(bullet, transform.position, Quaternion.identity);
            b.transform.position = transform.position;
            b.GetComponent<CircleCollider2D>().enabled = true;
            b.GetComponent<SpriteRenderer>().enabled = true;
        }
    }
    public void ShootLaser()
    {
        Laser closestLaser;
        BoxCollider2D closestCollider;
        if(Controller.instance.player.transform.position.y< -2.47)
        {
            closestLaser = lasers[0];
            closestCollider = laserColliders[0];
        }
        else if(Controller.instance.player.transform.position.y < 1.55)
        {
            closestLaser = lasers[1];
            closestCollider = laserColliders[1];
        }
        else
        {
            closestLaser = lasers[2];
            closestCollider = laserColliders[2];
            
        }

        IEnumerator StallExecution()
        {

            yield return new WaitForSeconds(0.3f);

            closestCollider.enabled = true;

            yield return new WaitForSeconds(0.3f);

            closestCollider.enabled = false;
        }
    }
}
