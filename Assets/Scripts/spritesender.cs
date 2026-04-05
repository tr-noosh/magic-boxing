using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spritesender : MonoBehaviour
{
    public bool Actionable = false;
    public ParticleSystem powsL, powsR;

    public OpponentController opponent;
    public PlayerController player;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        player.actionable = Actionable;


    }

    void hit(string name) { player.hit(name); }



    void powL(){
        if(opponent.blocking == BlockType.NONE)
        {
            powsL.Play();
        }
    }

    public void reset_Damage() { player.ani.ResetTrigger("stun"); }

    void powR()
    {
        if (opponent.blocking == BlockType.NONE)
        {
            powsR.Play();
        }
    }

}
