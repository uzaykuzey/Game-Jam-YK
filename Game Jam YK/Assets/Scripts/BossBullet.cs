using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : MonoBehaviour
{
    public void FlyToPlayer(int phase)
    {
        transform.DOMove((Controller.instance.player.transform.position + new Vector3(UnityEngine.Random.Range(-5f, 5f), UnityEngine.Random.Range(-5f, 5f), 0) - transform.position).normalized * 50, 7.5f - (phase==1 ? 0: 1.5f)).onComplete += () =>
        {
            Destroy(gameObject);
        };
    }
}
