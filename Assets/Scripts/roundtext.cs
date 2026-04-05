using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class roundtext : MonoBehaviour
{
    public TextMeshProUGUI round;
    public GameObject robject;
    public Animator textani;


   public int remaining;
   public int timer;



    void Start()
    {
        robject.SetActive(true);
        round.text = "Round 1";
     
    }


    void Update()
    {
        if (remaining <= 0) {

            textani.ResetTrigger("round");


            return;
        }



        if (timer > 200)
        {
            round.text = remaining.ToString();
            textani.SetTrigger("round");
            robject.SetActive(true);

            remaining--;
            timer = 0;

        }
        else
        {
            timer++;
        }
    }

    public void countdown(int downfrom)
    {
        remaining = downfrom;
        timer = 0;
    }


    public void rounddown(int num)
    {
    
        round.text = "Round" + num.ToString();
        textani.SetTrigger("fade");
        robject.SetActive(true);



    }
}