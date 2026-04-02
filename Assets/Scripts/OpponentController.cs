using System;

using TMPro;
using UnityEngine;


using UnityEngine.UI;
using Random = UnityEngine.Random;

public enum BlockType : int
{
	NONE = 0,
	LOW = 1,
	HIGH = 2,
	ALL = 3,
}

[RequireComponent(typeof(SpriteRenderer))]
public class OpponentController : MonoBehaviour
{
	public PlayerController player;

	public Slider healthBar;
    public strike_sender strikes;

    private SpriteRenderer spr;

	public GameObject enemy;

    private Animator ani;

    public Animator strikeani;
    public Animator fireani;
    public Animator cloudani;

    public EnemyMove[] moveList;

	[Header("Hitting Zones")]
	public bool hitCenter = false;
	public bool hitLow = false;
	public bool hitLeft = false;
	public bool hitRight = false;

	[Header("Opponent Position")]
	public bool center = true;
	public bool left = false;
	public bool right = false;

	public bool low = true;
	public bool high = true;


    public bool invincible = true;
    public BlockType blocking = BlockType.NONE;
	public bool blocked = false;

	public int hitsRemaining = 4;
	public float stunTime = 10.0f;
	public float currentMoveDamage = 0.0f;

	public float maxHealth = 100.0f;
	public float health = 100.0f;
	public int knockouts = 0;
	public int roundKOs = 0;
	public int phase = 0;

	public bool stunned = false;
	public bool finalHit = false;

	public bool knockedOut = false;
	public float koTimer = 0.0f;
	public float getupTime = 30.0f;
	public TextMeshProUGUI koText;
	private Animator koAni;

	public bool success,iv = false;

	public int btimer = 300;
    public int ftimer = 0;

    public ParticleSystem glass;

    void Awake()
	{
		spr = GetComponent<SpriteRenderer>();
		ani = GetComponent<Animator>();
		koAni = koText.gameObject.GetComponent<Animator>();

        ani.enabled = false;
    }

	private void knockout() {

		iv = true;
		//knockedOut = true; // set by animation
		roundKOs++;
		knockouts++;
		phase++;
		
		ani.SetTrigger("KO");
		getupTime = 1;
	}

	public void damage(int dmg, bool rightPunch)
	{   // opponent taking damage. interrupt attacks and play animations
		if (blocking == BlockType.HIGH || blocking == BlockType.ALL)
		{

			Debug.Log("block");

			block(true, rightPunch);

		}
		else
		{



			if (btimer == 0)
			{

				//Debug.Log("damage");

				if (knockouts > 0)
				{
                    btimer = 40;

                    blocking = BlockType.NONE;


					health -= dmg;

					if (health <= 0.0f)
					{
						knockout(); return;
					}


					/*	if (hitsRemaining > 0 && stunTime > 0.0f)
						{
							stunned = true;
							Debug.Log("gorp");
						}

						if (hitsRemaining == 1) { finalHit = true; }
						if (hitsRemaining <= 0) {
					*/

					ani.SetBool("stunned", stunned);
					ani.SetBool("final", finalHit);


					if (hitsRemaining <= 0)
					{

                        btimer = 1000;
                        blocking = BlockType.ALL;

						ani.SetTrigger("left_flame");
						ani.Play("flame_left");
                       

                    }
					else
					{
						hitsRemaining--;


						ani.SetTrigger("ouch" + (rightPunch ? "Right" : "Left") + ("Low"));

					}


				}
				//ani.SetTrigger("ouch" + (rightPunch ? "Right" : "Left") + (highPunch ? "High" : "Low"));
			}
		  }

		}



    

	public void glass_break() { 
       {
            glass.Play();
       }
    }


    public void block(bool highPunch, bool rightPunch) {

        ani.SetTrigger(
         (rightPunch ? "right" : "left") + ("_block")
         );

    }

    private void checkHitting() {


        if (!player.invincible)
		{
            if (player.center && hitCenter)
			{
				success = true;
				player.damaged("center", currentMoveDamage);
			}
			else if (player.low && hitLow)
			{
				success = true;
				player.damaged("low", currentMoveDamage);
			}
			else if (player.left && hitLeft)
			{
				success = true;
				player.damaged("left", currentMoveDamage);
			}
			else if (player.right && hitRight)
			{
				success = true;
				player.damaged("right", currentMoveDamage);

			}
			//ani.SetBool("success", success);
		}
	}

	public void chooseMove() {

		iv = false;
		success = false;
		ani.SetBool("success", success);
		finalHit = false;
		ani.SetBool("final", finalHit);

		EnemyMove move = RandomMove.SelectMove(moveList, phase);

		hitsRemaining = move.maxHits;
		stunTime = move.maxTime;
		currentMoveDamage = move.damageOnHit;

		if (move.triggerName != null)
		{


			if (move.playRandomFromList > 0)
			{
				int anim_r = Random.Range(0, move.triggerName.Length);

				//strikes.move(anim_r);

				ani.SetTrigger(move.triggerName[anim_r]);
				strikeani.SetTrigger(move.triggerName[anim_r]);
                cloudani.SetTrigger(move.triggerName[anim_r]);
                fireani.SetTrigger(move.triggerName[anim_r]);

            }
			else
			{
               // strikes.move(0);

                ani.SetTrigger(move.triggerName[0]); 
                strikeani.SetTrigger(move.triggerName[0]);
                cloudani.SetTrigger(move.triggerName[0]);
                fireani.SetTrigger(move.triggerName[0]);
            }

		}

    }
	
	private int lastNumber = 0;

	void Update() {

      
        if(btimer > 0)
		{

			--btimer;
		}



        if (Input.GetKey(KeyCode.Alpha2))
        {
            ani.SetTrigger("right_block");
        }
     



        if (player.gameState == 1)
		{
			spr.enabled = true;
			enemy.SetActive(true);
			healthBar.enabled = false;

            invincible = true;
            ani.enabled = true;
			ani.SetTrigger("start");

			

			ftimer++;


			healthBar.value = health / maxHealth;

			if (ftimer >= 500)
			{

                ftimer = 500;
                invincible = false;
                checkHitting();
               

                if (stunned)
				{
					if (stunTime > 0.0f) { stunTime -= Time.deltaTime; }
					else
					{
						stunned = false;
						ani.SetBool("stunned", stunned);
					}
				}
				if (knockedOut)
				{
					koTimer += Time.deltaTime;
					int countNum = (int)Math.Floor(koTimer);
					if (roundKOs == 3)
					{
						koText.text = "TKO";
						koAni.SetTrigger("TKO");
						// TKO!!! end game
						// try deactivating the script itself so no funny logic happens
						enabled = false;
						return;
					}
					else if (koTimer >= getupTime)
					{
						knockedOut = false;
						ani.SetTrigger("RISE");
						health = 70.0f; // something
						koTimer = 0.0f;
						lastNumber = 0;
					}
					else if (koTimer >= 11.0f)
					{
						koText.text = "KO!";
						koAni.SetTrigger("KO");
						// its over, knockout!!
					}
					else if (countNum < 11 && countNum > lastNumber && countNum < getupTime)
					{
						lastNumber = countNum;
						koText.text = countNum.ToString();
						koAni.SetTrigger("count");
					}
				}


            }
		
		
			
		
		}
		else
		{

			spr.enabled = false;
			healthBar.enabled = false;
			enemy.SetActive(false);

		}
		
	}


	Color activeColor = new(.33f, .80f, .16f, 1f); Color inactiveColor = new(.61f, .61f, .61f, 1f); Color blockColor = new(.8f, .8f, .3f, 1f);
	Vector3 flat = new(.2f, .2f, 0.01f); Vector3 flatWide = new(.8f, .12f, 0.01f);
	private void OnDrawGizmos() {
		if (!spr) {spr = GetComponent<SpriteRenderer>();}
		Gizmos.matrix = Matrix4x4.TRS(spr.bounds.center, Camera.current.transform.rotation, Vector3.one);
		Vector3 above = new Vector3(0, spr.bounds.extents.y + .25f, 0);

		// Center
		Gizmos.color = center && high ? activeColor : inactiveColor;
		Gizmos.DrawCube(above, flat);
		Gizmos.color = center && low ? activeColor : inactiveColor;
		Gizmos.DrawCube(above - transform.up*.25f, flat);

		// Left
		Gizmos.color = left && high ? activeColor : inactiveColor;
		Gizmos.DrawCube(above - transform.right*.25f, flat);
		Gizmos.color = left && low ? activeColor : inactiveColor;
		Gizmos.DrawCube(above - transform.up*.25f - transform.right*.25f, flat);

		// Right
		Gizmos.color = right && high ? activeColor : inactiveColor;
		Gizmos.DrawCube(above + transform.right*.25f, flat);
		Gizmos.color = right && low ? activeColor : inactiveColor;
		Gizmos.DrawCube(above - transform.up*.25f + transform.right*.25f, flat);

		// Blocking
		Gizmos.color = blockColor;
		if (blocking == BlockType.LOW || blocking == BlockType.ALL) {
			Gizmos.DrawCube(above - transform.up*.25f - transform.forward*.01f, flatWide);
		}
		if (blocking == BlockType.HIGH || blocking == BlockType.ALL) {
			Gizmos.DrawCube(above - transform.forward*.01f, flatWide);
		}
	}
}
