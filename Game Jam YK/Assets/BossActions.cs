using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class BossActions : MonoBehaviour
{
    [SerializeField] private float cycleLengthX = 2;
    [SerializeField] private float HorizontalPos = 10;
    [SerializeField] Transform transform;
    [SerializeField] public float[] platformY;
    [SerializeField] public float Xleft;
    [SerializeField] public float Xright;
    [SerializeField] public float phase;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void Teleport()
    {
        float targetX = UnityEngine.Random.Range(Xleft, Xright);
        int targetYindex = UnityEngine.Random.Range(0, platformY.Length - 1);
        float targetY = platformY[targetYindex];
        transform.position = new Vector3(targetX, targetY, 0);
    }
}
