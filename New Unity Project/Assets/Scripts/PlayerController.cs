using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class PlayerController : NetworkBehaviour {

	public float moveSpeed = 5;
	public float moveX;
	public float moveY;

	private Animator anim;
	private Rigidbody2D rigidBody;

	private bool playerMoving;
	private Vector2 lastMove;






	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		rigidBody = GetComponent<Rigidbody2D>();
		if (isLocalPlayer) {
			//rigidBody.gameObject.GetComponent<Renderer> ().material.color = Color.black;
			this.transform.GetChild (0).gameObject.GetComponent<Camera> ().enabled = true;

		}
		else {
			
			this.transform.GetChild (0).gameObject.GetComponent<Camera> ().enabled = false;
			rigidBody.gameObject.GetComponent<Renderer> ().material.color = Color.red;

		}
	}
	
	// Update is called once per frame
	void Update () {
		if (!isLocalPlayer)
		{
			
			return;
		}

		moveX = Input.GetAxisRaw ("Horizontal");	// moveX = 1, 0 or -1
		moveY = Input.GetAxisRaw ("Vertical");		// moveY = 1, 0 or -1
		playerMoving = false;

		//rigidBody.velocity = new Vector2(
		//	moveX * moveSpeed * (1.0f-0.2f*Mathf.Abs(moveY)),
		//	moveY * moveSpeed * (1.0f-0.2f*Mathf.Abs(moveX)));

		if (moveX != 0 || moveY != 0) 
		{
			transform.Translate (new Vector3(
				moveX * moveSpeed * Time.deltaTime * (1.0f-0.2f*Mathf.Abs(moveY)),	// (1.0f-0.2f) is to calibrate diagonal move speed
				moveY * moveSpeed * Time.deltaTime * (1.0f-0.2f*Mathf.Abs(moveX))));
			playerMoving = true;
			lastMove = new Vector2 (moveX, moveY);	// only updated when moved
		}

		anim.SetFloat ("MoveX", moveX);
		anim.SetFloat ("MoveY", moveY);
		anim.SetBool ("PlayerMoving", playerMoving);
		anim.SetFloat ("LastMoveX", lastMove.x);
		anim.SetFloat ("LastMoveY", lastMove.y);


	}




}
