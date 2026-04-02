
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class strike_sender : MonoBehaviour
{


    //public OpponentController opponent;
    public PlayerController player;

    public float d = 0.5f;
    public float size = 0.3f;
    public float speed = 1.0f;

    Vector3 iPos = new Vector3(0f, 2f, -4.5f);
    float tr;

    public Camera camera;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
     

        }

        // if (Input.GetKeyDown(KeyCode.UpArrow))
        // {
        //   shake();
        // }

        if (tr > 0)
        {
            camera.transform.localPosition = iPos + Random.insideUnitSphere * size;

            tr -= Time.deltaTime * speed;
        }
        else
        {
            tr = 0f;
            camera.transform.localPosition = iPos;
        }
    }

    

    public void move(int lr)
    {
        if (lr == 0)
        {
            transform.position = new Vector3(-4.52f, -1.4f, 0.52f);
        }
        else

        {
            transform.position = new Vector3(4.52f, -1.4f, 0.52f);

        }
    }

 
    public void shake()
        {
            d = 0.3f;
            size = 0.5f;
            tr = d;
        }

    public void dshake()
    {
        d = 0.1f;
        size = 0.2f;
        tr = d;
    }



}

