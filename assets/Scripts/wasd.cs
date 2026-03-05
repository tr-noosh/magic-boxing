using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class wasd : MonoBehaviour
{
    public Graphic W, A, S, D, Space, Shift;

    public int Left, Right, Up, Down, Punch = 0;


    // Start is called before the first frame update
    void Start()
    {
     
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.S))
        {
            S.color = Color.green;

        }
        else
        {
            S.color = Color.white;

        }

        if (Input.GetKey(KeyCode.W))
        {
            W.color = Color.green;

        }
        else
        {
            W.color = Color.white;
        }
            if (Input.GetKey(KeyCode.A))
            {
                A.color = Color.green;
            Left = 1;
            }
            else
            {
                A.color = Color.white;
            Left = 0;

             }
 
            if (Input.GetKey(KeyCode.D))
            {
                D.color = Color.green;
           Right = 1;
        }
            else
            {
                D.color = Color.white;
            Right = 0;
        }



        if (Input.GetKey(KeyCode.LeftShift))
        {
            Shift.color = Color.green;

        }
        else
        {
            Shift.color = Color.white;

        }

        if (Input.GetKey(KeyCode.Space))
        {
            Space.color = Color.green;

        }
        else
        {
            Space.color = Color.white;

        }

    }

  }