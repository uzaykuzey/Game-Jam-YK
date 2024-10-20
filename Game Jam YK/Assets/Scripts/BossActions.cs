using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class BossActions : MonoBehaviour
{
    //[SerializeField] private float cycleLengthX = 2;
    //[SerializeField] private float HorizontalPos = 10;
    [SerializeField] public float[] platformY;
    [SerializeField] public float Xleft;
    [SerializeField] public float Xright;
    [SerializeField] public int phase;
    [SerializeField] public BossBullet bullet;
    //[SerializeField] public Laser[] lasers;
    [SerializeField] public BoxCollider2D[] laserColliders;
    [SerializeField] public Animator[] animators;

    // Start is called before the first frame update
    void Start()
    {
        phase = 1;
        bullet.GetComponent<CircleCollider2D>().enabled = false;
        bullet.GetComponent<SpriteRenderer>().enabled = false;
    }

    public void Teleport()
    {
        Controller.instance.PlayAudio(Controller.instance.teleport);
        float targetX;
        float targetY;
        do
        {
            targetX = UnityEngine.Random.Range(Xleft, Xright);
            targetY = platformY[UnityEngine.Random.Range(0, platformY.Length)];
        }
        while ((new Vector3(targetX, targetY, Controller.instance.player.transform.position.z) - Controller.instance.player.transform.position).magnitude < 5 || (new Vector3(targetX, targetY, transform.position.z) - transform.position).magnitude < 10) ;
        transform.position = new Vector3(targetX, targetY, 0);
    }

    public void SpawnBullet()
    {
        Controller.instance.PlayAudio(Controller.instance.snowballs);
        int repeat = 3 + (phase == 1 ? 0 : 1);
        for (int i=0;i< repeat; i++)
        {
            BossBullet b = Instantiate(bullet, transform.position, Quaternion.identity);
            b.transform.position = transform.position;
            b.GetComponent<CircleCollider2D>().enabled = true;
            b.GetComponent<SpriteRenderer>().enabled = true;
            b.FlyToPlayer(phase);
        }
    }
    public void ShootLaser()
    {
        //Laser closestLaser;
        BoxCollider2D closestCollider;
        if(Controller.instance.player.transform.position.y< -2.47)
        {
            closestCollider = laserColliders[0];
        }
        else if(Controller.instance.player.transform.position.y < 1.55)
        {
            closestCollider = laserColliders[1];
        }
        else
        {
            closestCollider = laserColliders[2];
        }
        closestCollider.GetComponent<Animator>().Play("LaserShoot");
        if(phase==2)
        {
            laserColliders[3].transform.position = new Vector3(Controller.instance.player.transform.position.x, laserColliders[3].transform.position.y, laserColliders[3].transform.position.z);
            laserColliders[3].GetComponent<Animator>().Play("LaserShoot");
        }
        Controller.instance.PlayAudio(Controller.instance.laserBlast);
    }
}
