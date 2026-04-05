
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class key_choose : MonoBehaviour
{


    public PlayerController player;

    public Texture[] sprites;
    public keymap[] keys;

    public int cycle = 1;

    public RawImage key_u, handL, handR;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        key_u.texture = sprites[cycle];

        player.keyUpdate(keys[cycle]);


        switch (cycle)
        {
            case 0:
                handL.rectTransform.anchoredPosition = new Vector2(91.9f, 52.4f);
                handR.rectTransform.anchoredPosition = new Vector2(181.5f, 52.4f);
                break;

            case 1:
                handL.rectTransform.anchoredPosition = new Vector2(91.9f, 52.4f);
                handR.rectTransform.anchoredPosition = new Vector2(231.8f, 39.2f);
                break;

            case 2:
                handL.rectTransform.anchoredPosition = new Vector2(99.2f, 56.4f);
                handR.rectTransform.anchoredPosition = new Vector2(231.8f, 39.2f);
                break;
        }

        if (player.menuState == 3)
        {


            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                cycle = (cycle + 1) % sprites.Length;
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                cycle = (cycle - 1 + sprites.Length) % sprites.Length;
            }
       


        }
    }
}
