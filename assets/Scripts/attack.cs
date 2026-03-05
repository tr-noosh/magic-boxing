using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class attack : MonoBehaviour
{
    public Texture[] frames;
    public Renderer left_fist;
    public Renderer right_fist;

    public wasd squares_check;

    public float changeInterval = 0.16F;

    int play = 0;

    void Start()
    {
        //left_fist = GetComponent<Renderer>();
        //right_fist = GetComponent<Renderer>();
    }


    // Update is called once per frame
    void Update()
    {
        int index = Mathf.FloorToInt(Time.time / changeInterval);

        if (squares_check.Punch == 1)
        {
            play = 1;


            if (squares_check.Right == 1)
            {
                right_fist.material.mainTexture = frames[index];


            }
            else
            {
                left_fist.material.mainTexture = frames[index];

            }
        }


        if (play == 1)
        {
            index = Mathf.FloorToInt(Time.time / changeInterval);
            index = index % frames.Length;
           // rend.material.mainTexture = frames[index];

        }
        else
        {
            play = 0;
        }

        left_fist.material.mainTexture = frames[index];
        right_fist.material.mainTexture = frames[index];

    }
}
