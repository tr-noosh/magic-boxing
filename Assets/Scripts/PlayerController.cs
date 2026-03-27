using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;


















//using System.Diagnostics;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(SpriteRenderer))]
public class PlayerController : MonoBehaviour
{
	public OpponentController opponent;

	public UnityEngine.UI.Slider healthBar;

	public GameObject ui;
	public GameObject mui;


	private SpriteRenderer spr;
	public GameObject gloves;

	public Animator ani;
	public Animator oppani;

	public TextMeshProUGUI scoreText;

	public int gameState = 0;
	public int menuState = 1;
	public int difficulty = 2;



	[Header("Player Position")]
	public bool center = true;
	public bool low = true;
	public bool left = false;
	public bool right = false;

	[Header("Player State")]
	public bool actionable = true; // can begin an action or interrupt currently performing action
	public bool invincible = false; // temporarily immune to further damage
	public int knockouts = 0;
	public int roundKOs = 0;
	public bool knockedOut = false;
	public float koTimer = 0.0f;
	public float getupProgress = 0.0f;

	[Header("Stats")]
	public float maxHealth = 100.0f;
	public float health = 100.0f;
	public float damageStat = 5.0f;

	[Header("mappings n whatnot")]

	public KeyCode left_punch = KeyCode.D;
	public KeyCode right_punch = KeyCode.K;

	public KeyCode left_dodge = KeyCode.C;
	public KeyCode right_dodge = KeyCode.M;
	public KeyCode low_dodge = KeyCode.Space;

	//   public KeyCode left_dodge = KeyCode.RightArrow;
	//public KeyCode right_dodge = KeyCode.LeftArrow;
	//public KeyCode low_dodge = KeyCode.DownArrow;


	public bool jabHold = false;

	public KeyCode left_jab = KeyCode.S;
	public KeyCode right_jab = KeyCode.L;



	public UnityEngine.UI.Button play;
	public UnityEngine.UI.Button settings, cset;

	public UnityEngine.UI.Button start, easy, mid, hard;

	public GameObject difficultyMenu, startMenu, settingsMenu, loseMenu;

	public bool heldL, heldR, heldD;

	public int ttimer = 0;
    public GameObject loseText;

    public float damage_t = 12f;

	void Awake()
	{

	}


	void Start()
	{
        //loseText.SetActive(false);
        play.onClick.AddListener(PlayClick);
		start.onClick.AddListener(StartClick);

		cset.onClick.AddListener(CClick);
        //
        //hard.onClick.AddListener(hardClick);

        settings.onClick.AddListener(SClick);


		
        spr = GetComponent<SpriteRenderer>();
		// ani = GetComponent<Animator>();

		menuUpdate();
	}

	void miss() { }
	void blocked() { }


	public void damaged(string zone, float damage) {

		//damage = damage_t;
        damage_t = damage;
        healthBar.value = health / maxHealth;

		ani.SetTrigger("stun");

		ttimer = 30;

			if (health <= 0)
			{
			gameState = 0;
            menuState = 5;
            menuUpdate();

		}

	}



	public void hit(string punch)
	{
		bool highPunch = false;
		bool rightPunch = false;

		switch (punch)
		{
			case "LEFTJAB":
				highPunch = true;
				break;
			case "RIGHTJAB":
				rightPunch = true;
				highPunch = true;
				break;
			case "LEFTHOOK":
				break;
			case "RIGHTHOOK":
				rightPunch = true;
				break;
                //Debug.Log("sd");
        }

		/*if (highPunch) { // JAB
			if (!opponent.high) { miss(); }

			else if (opponent.blocking == BlockType.HIGH || opponent.blocking == BlockType.ALL) { opponent.block(highPunch, rightPunch); blocked(); }
			
			else if (opponent.center) { opponent.damage(highPunch, rightPunch, damageStat); }
			else if ((rightPunch && opponent.right) || (!rightPunch && opponent.left)) {
				opponent.damage(highPunch, rightPunch, damageStat);
			}
			else { miss(); }
		}
		else { // HOOK
			if (!opponent.low) { miss(); }
			else if (opponent.blocking == BlockType.LOW || opponent.blocking == BlockType.ALL) { opponent.block(highPunch, rightPunch); blocked(); }
			else if (opponent.center) { opponent.damage(highPunch, rightPunch, damageStat); }

			else if ((rightPunch && opponent.right) || (!rightPunch && opponent.left)) {
				opponent.damage(highPunch, rightPunch, damageStat);
			}
			else { miss(); }
		}*/
		if (opponent.iv == true)
		{


			Debug.Log("s");
		}
		else
		{
			if (opponent.blocking == BlockType.HIGH || opponent.blocking == BlockType.ALL)
			{

				opponent.block(highPunch, rightPunch);

				Debug.Log("a");

				blocked();

			}
			else
			{
				opponent.damage(highPunch, rightPunch, 2f);

				Debug.Log("d");

			}
		}




	}


	public void keyUpdate(keymap key)
	{
		left_dodge = key.left_dodge;
        right_dodge = key.right_dodge;

      left_jab = key.left_jab;
        right_jab = key.right_jab;

		left_punch = key.left_punch;
		right_punch = key.right_punch;

		low_dodge = key.low_dodge;

		Debug.Log("assigned keys");

    }

	private void startPunch(bool right)
	{
		bool jab = Input.GetKey(left_jab);
		ani.SetTrigger(
			(right ? "right" : "left") + (jab ? "jab" : "hook")
		);

		//hit((right ? "right" : "left") +(jab ? "jab" : "hook"));

		//oppani.SetTrigger(
		// (right ? "right" : "left") + ("_block")
		// );

	}

	void Update()
	{

		if (Input.GetKeyDown(KeyCode.Alpha1))
		{


			if (invincible == true)
			{
				invincible = false;


			}
			else
			{
				invincible = true;
			}
		}



		if (ttimer > 0)
		{
			ttimer--;
		}
		if (ttimer == 28)
		{
            health -= damage_t * difficulty;
        }
        if (ttimer == 0)
        {

			ani.ResetTrigger("stun");
          //  health -= damage_t;
        }


		right = heldR;
		left = heldL;
		low = heldD;

        center = !(heldL || heldR);

        if (gameState == 1)
		{
            ui.SetActive(true);
            mui.SetActive(false);
            gloves.SetActive(true);

			updateScoreText();

			if (!actionable) {

                ani.ResetTrigger("leftHook");
                ani.ResetTrigger("rightHook");
                ani.ResetTrigger("leftJab");
                ani.ResetTrigger("rightJab");

                return;
            } 

            heldL = Input.GetKey(left_dodge);
            Vector3 desired = heldL ? new Vector3(-4f, 0f, -2f) : new Vector3(0f, 0f, -2f);
            transform.position = Vector3.Lerp(transform.position, desired, Time.deltaTime * 6f);
            //gloves.transform.position = Vector3.Lerp(gloves.transform.position, desired, Time.deltaTime * 3f);

            heldR = Input.GetKey(right_dodge);
            Vector3 desiredr = heldR ? new Vector3(4f, 0f, -2f) : new Vector3(0f, 0f, -2f);
            transform.position = Vector3.Lerp(transform.position, desiredr, Time.deltaTime * 6f);


            heldD = Input.GetKey(low_dodge);
            Vector3 desiredl = heldD ? new Vector3(0f, -2f, -2f) : new Vector3(0f, 0f, -2f);
			transform.position = Vector3.Lerp(transform.position, desiredl, Time.deltaTime * 6f);

		

            if (Input.GetKey(left_punch))
			{
				if (jabHold == true)
				{
					startPunch(true);

				}
				else
				{

					ani.SetTrigger(("left") + ("Hook"));

				}
			}
			else if (Input.GetKey(right_punch))
			{

				if (jabHold == true)
				{
					startPunch(false);

				}
				else
				{
					ani.SetTrigger(("right") + ("Hook"));

				}
			}
			if(jabHold == false)
			{
                if (Input.GetKey(left_jab))
                {
                    ani.SetTrigger("leftJab");
                  

                }
                else if (Input.GetKey(right_jab))
                {
                    ani.SetTrigger("rightJab");
                   
                }


            }


        }
		else
		{
            ui.SetActive(false);
            mui.SetActive(true);
            gloves.SetActive(false);

			menu();


		}


	
	}

	void menu()
	{

   
        ////play.clicked += Click("play");

        // settings.clicked += Click("settings");

    }

	/* void Click(string button)
     {

         switch (button)
         {
             case "play":
                 Debug.Log("play");
                 break;
             case settings:
                 Debug.Log("settoings!");
                 break;
         }

         Debug.Log("Clicked!");

     } */

	void PlayClick()
	{
		Debug.Log("play button clicked");

		if (menuState == 1)
		{
			menuState = 2;
		}

		menuUpdate();

	}

	void menuUpdate()
	{
        startMenu.SetActive(false);
        difficultyMenu.SetActive(false);
        settingsMenu.SetActive(false);
        loseMenu.SetActive(false);

        switch (menuState)
        {
            case 1:
                startMenu.SetActive(true);
                break;
            case 2:
                difficultyMenu.SetActive(true);
                break;
            case 3:
                settingsMenu.SetActive(true);
                break;
            case 5:
                loseMenu.SetActive(true);
                break;


        }
    }

	void SClick()
	{
		Debug.Log("settings button");

		if (menuState == 1)
		{
			menuState = 3;
		}
		menuUpdate();
	}


    void CClick()
    { Debug.Log("choose key button");   if (menuState == 3){
  menuState = 1;
        }
        menuUpdate();
    }

    void StartClick()
    {
        Debug.Log("start game button");

        if (menuState == 2)
        {
            gameState = 1;
        }

    menuUpdate();
		}



    void easyClick() {Debug.Log("easy"); difficulty = 1; }
    void midClick() { Debug.Log("medium"); difficulty = 2; }
    void hardClick() { Debug.Log("hard"); difficulty = 3; }





    void updateScoreText() {
		string playerTxt = ( roundKOs == 2 ? "<color=\"red\">2</color>" : roundKOs.ToString());
		string opponentTxt = ( opponent.roundKOs == 2 ? "<color=\"red\">2</color>" : opponent.roundKOs.ToString());
		scoreText.text = playerTxt + "-" + opponentTxt;
	}

	Color activeColor = new(.33f, .80f, .16f, 1f); Color inactiveColor = new(.61f, .61f, .61f, 1f); Color hurtColor = new(.93f, .25f, .25f, 1f);
	Vector3 flat = new(.2f, .2f, 0.01f);

	private void OnDrawGizmos() {
		if (!spr) {spr = GetComponent<SpriteRenderer>();}
		Gizmos.matrix = Matrix4x4.TRS(spr.bounds.center, Camera.current.transform.rotation, Vector3.one);
		Vector3 above = new Vector3(0, spr.bounds.extents.y + .25f, 0);

		// Actionability indicator
        if (!actionable)
        {
            Gizmos.color = new(0, 0, 0, .7f);
            Gizmos.DrawCube(above - transform.up * .125f, flat * 4f);
        }

        // Center
        Gizmos.color = center ? activeColor : inactiveColor;
		Gizmos.DrawCube(above, flat);
		if (opponent.hitCenter) {
			Gizmos.color = hurtColor;
			Gizmos.DrawCube(above - transform.forward*.01f, flat*0.6f);
		}

		// Low
		Gizmos.color = low ? activeColor : inactiveColor;
		Gizmos.DrawCube(above - transform.up*.25f, flat);
		if (opponent.hitLow) {
			Gizmos.color = hurtColor;
			Gizmos.DrawCube(above - transform.up*.25f - transform.forward*.01f, flat*0.6f);
		}

		// Left
		Gizmos.color = left ? activeColor : inactiveColor;
		Gizmos.DrawCube(above - transform.right*.25f, flat);
		if (opponent.hitLeft) {
			Gizmos.color = hurtColor;
			Gizmos.DrawCube(above - transform.right*.25f - transform.forward*.01f, flat*0.6f);
		}

		// Right
		Gizmos.color = right ? activeColor : inactiveColor;
		Gizmos.DrawCube(above + transform.right*.25f, flat);
		if (opponent.hitRight) {
			Gizmos.color = hurtColor;
			Gizmos.DrawCube(above + transform.right*.25f - transform.forward*.01f, flat*0.6f);
		}

		// Invincibility indicator
        if (invincible)
		{
            Gizmos.color = Color.yellow;
            Gizmos.DrawCube(above - transform.up * .125f, flat * 3f);
        }
	}
}
