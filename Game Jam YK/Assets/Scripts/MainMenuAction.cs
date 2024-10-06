using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuAction : MonoBehaviour
{
    public int type;

    private void OnMouseDown()
    {
        if(type==0)
        {
            SceneManager.LoadScene("Cutscene");
        }
        else if(type==1)
        {
            Application.Quit();
        }
        else
        {
            SceneManager.LoadScene("Main Menu");
        }
    }
}
