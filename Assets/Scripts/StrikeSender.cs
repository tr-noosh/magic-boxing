
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class StrikeSender : MonoBehaviour
{
	public PlayerController player;
	public OpponentController opponent;

	public ParticleSystem powsL, powsR;

	
	// seems to manage opponent hit particle effect, move to opponent

	public float shakeLength = 0.25f; // default shake duration
	public float shakeSize = 0.5f;

	Vector3 initPos = new Vector3(0f, 2f, -4.5f);
	float shakeTime = 0.0f;

	public Camera cam;

	void Awake()
	{
		cam = Camera.main;
		initPos = cam.transform.position;
	}


	int i = 0;
	void FixedUpdate()
	{
		i = (i + 1) % 2;
		if (i != 0) return;
		if (shakeTime > 0.0f) {
			cam.transform.position = initPos + UnityEngine.Random.insideUnitSphere * shakeSize * Mathf.Pow(shakeTime/shakeLength,3);
			shakeTime -= Time.fixedDeltaTime;
		}
		else {
			cam.transform.position = initPos;
		}
	}

	public void move(int lr) {
		if (lr == 0) {
			transform.position = new Vector3(-4.52f, -1.4f, 0.52f);
		}
		else {
			transform.position = new Vector3(4.52f, -1.4f, 0.52f);
		}
	}

	public void shake() {
		shakeTime = shakeLength;
	}

	//public void reset_Damage() { player.ani.ResetTrigger("stun"); }
	public void pow(bool rightPunch){
		if (rightPunch) {
			powsR.Play();
		}
		else {
			powsL.Play();
		}
	}
}
