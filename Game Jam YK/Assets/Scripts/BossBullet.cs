using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : MonoBehaviour
{
    public Vector3 playerPos;

    public static bool real = true;

    private void Start()
    {
        if(real)
        {
            real = false;
            return;
        }
        transform.DOMove((Controller.instance.player.transform.position + new Vector3(UnityEngine.Random.Range(-2f, 2f), UnityEngine.Random.Range(-2f, 2f), 0) - transform.position).normalized * 50, 4).onComplete += ()=> 
        { 
            Destroy(gameObject);        
        };
    }

}
