using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NextScript : MonoBehaviour
{
    [SerializeField] Text text;
    [SerializeField] SpriteRenderer sr;
    [SerializeField] Sprite first;
    [SerializeField] Sprite second;
    private int current;

    void Start()
    {
        current = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(current==0)
        {
            text.text = "I Have Been Cursed By\r\nAnother Fortune Teller.\r\nThe Cards Will Tell What I Should Do.";
            sr.sprite = first;
        }
        else if(current==1)
        {
            text.text = "Death and the Ace of Swords... \r\nI Know Exactly What I Have To Do.";
            sr.sprite = second;
        }
        else
        {
            SceneManager.LoadScene("Castle");
        }

        if(Controller.GetKey(Control.RightInput))
        {
            OnMouseDown();
        }
    }

    private void OnMouseDown()
    {
        if(Controller.InMenu)
        {
            return;
        }
        current++;
    }
}
