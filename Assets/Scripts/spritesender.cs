using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spritesender : MonoBehaviour
{
    public bool Actionable = false;
    public ParticleSystem powsL, powsR;

    public OpponentController opponent;
    public PlayerController player;

    // seems to manage opponent hit particle effect, move to opponent

    void Update()
    {
        //player.actionable = Actionable;
    }

    public void reset_Damage() { player.ani.ResetTrigger("stun"); }

    void powL(){
        if(opponent.blocking == BlockType.NONE) powsL.Play();
    }

    void powR() {
        if (opponent.blocking == BlockType.NONE) powsR.Play();
    }

}
