using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public float moveSpeed = 5;
	public float moveX;
	public float moveY;
	public GameObject player2;

	private Animator anim;
	//private Rigidbody2D rigidBody;

	private bool playerMoving;
	private Vector2 lastMove;


	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		//rigidBody = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		
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

		Debug.DrawRay (transform.position, player2.transform.position-transform.position, Color.red);

		RaycastHit2D hit = Physics2D.Linecast (transform.position, player2.transform.position);
		if (hit.collider.gameObject == player2) {
			Debug.Log ("in sight!");
		}
			
		anim.SetFloat ("MoveX", moveX);
		anim.SetFloat ("MoveY", moveY);
		anim.SetBool ("PlayerMoving", playerMoving);
		anim.SetFloat ("LastMoveX", lastMove.x);
		anim.SetFloat ("LastMoveY", lastMove.y);
	}
}
