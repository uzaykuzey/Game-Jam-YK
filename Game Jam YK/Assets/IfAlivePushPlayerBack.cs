using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IfAlivePushPlayerBack : MonoBehaviour
{

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Controller.instance.player.transform.position.x > transform.position.x && Controller.instance.player.transform.position.y > -24.87f && Controller.instance.player.transform.position.y < -20.66f)
        {
            Controller.instance.player.transform.position = new Vector3(Controller.instance.player.transform.position.x - 1, Controller.instance.player.transform.position.y, Controller.instance.player.transform.position.z);
        }
    }
}
