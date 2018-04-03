using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class KwPlayer : NetworkBehaviour {

	public float moveSpeed = 5;
	public float moveX;
	public float moveY;
	public GameObject runner;

	public Image energyBar;
	public float totalTime = 180;
	private float timeLeft;

	public FixedJoystick joystick;


	private Animator anim;


	private bool playerMoving;
	private Vector2 lastMove;
	private NetworkStartPosition[] spawnPoints;

	//[SyncVar(hook="MapToggle")]
	private string playerType="chaser";

	//[SyncVar]
	//public string pname="chaser";


	public override void OnStartLocalPlayer(){
	Invoke ("Reactivate cam",0.5f);
		//CmdToggle ();

	}

	void ReactivateCam(){
		this.gameObject.GetComponentInChildren<Camera> ().enabled = false;
		this.gameObject.GetComponentInChildren<Camera> ().enabled = true;
		}


	void Start () {
		joystick = FindObjectOfType<FixedJoystick>();
		anim = GetComponent<Animator> ();
		runner = GameObject.Find ("Runner");	// reference to other player
		GameObject energyBarObj = GameObject.FindGameObjectWithTag("EnergyBar");
		energyBar = energyBarObj.GetComponent<Image>();

		timeLeft = totalTime;	// initialize time remaining

		if (isLocalPlayer) {
			this.transform.GetChild(0).gameObject.GetComponent<Camera>().enabled=true;	
			spawnPoints = FindObjectsOfType<NetworkStartPosition> ();
		
			if (anim.bodyPosition.x < 1.0) {
				CmdToggle ();
				Debug.Log (playerType);
			} else {
				Debug.Log (playerType);
			}

			MapToggle (playerType);
	


		} else {
			
			this.transform.GetChild(0).gameObject.GetComponent<Camera>().enabled=false;	
		}
	}
		
	void Update () {
		if (!isLocalPlayer) {
			
			return;
		}
	
		//Debug.Log (NetworkManager.matchHost);
		MoveAndRotate ();

	}


	void MoveAndRotate(){

		moveX = joystick.Horizontal;
		moveY = joystick.Vertical;
		//moveX = Input.GetAxisRaw ("Horizontal");	// moveX = 1, 0 or -1
		//moveY = Input.GetAxisRaw ("Vertical");		// moveY = 1, 0 or -1
		playerMoving = false;

		if (moveX != 0 || moveY != 0) 
		{
			transform.Translate (new Vector3(
				moveX * moveSpeed * Time.deltaTime * (1.0f-0.2f*Mathf.Abs(moveY)),	// (1.0f-0.2f) is to calibrate diagonal move speed
				moveY * moveSpeed * Time.deltaTime * (1.0f-0.2f*Mathf.Abs(moveX))));
			playerMoving = true;
			lastMove = new Vector2 (moveX, moveY);	// only updated when moved
		}

<<<<<<< HEAD:New Unity Project/Assets/Scripts/KwPlayer.cs
		//Debug.DrawRay (transform.position, player2.transform.position-transform.position, Color.red);
		Debug.DrawRay (Vector2.zero, new Vector2(3,0), Color.red);
=======
		Debug.DrawRay (transform.position, runner.transform.position-transform.position, Color.red);
>>>>>>> master:New Unity Project/Assets/Scripts/PlayerController.cs

		RaycastHit2D hit = Physics2D.Linecast (transform.position, runner.transform.position);
		if (hit.collider.gameObject == runner) {
			Debug.Log ("in sight!");
		}

		anim.SetFloat ("MoveX", moveX);
		anim.SetFloat ("MoveY", moveY);
		anim.SetBool ("PlayerMoving", playerMoving);
		anim.SetFloat ("LastMoveX", lastMove.x);
		anim.SetFloat ("LastMoveY", lastMove.y);

		timeLeft -= Time.deltaTime;
		float ratio = timeLeft / totalTime;
		energyBar.rectTransform.localScale = new Vector3 (1, ratio, 1);

		if (timeLeft <= 0) {
			Debug.Log ("Game Over!");
		}

	}

	[Command]
	void CmdToggle()
	{
		playerType = "hider";
	}

	void MapToggle(string playerType)
	{
		if(playerType == "hider") {
			this.transform.GetChild (0).gameObject.GetComponent<Camera> ().GetComponent<CameraController> ().render = true;
			this.transform.GetChild (1).gameObject.GetComponent<Light> ().intensity = 0;
	//  on directional light
	//}
		//Hide the sprite or not

		Debug.Log("Toggle invoked");
		}else{

			GameObject.FindGameObjectsWithTag("Player")[0].transform.GetChild (1).gameObject.GetComponent<Light> ().intensity = 0;
			GameObject.FindGameObjectsWithTag("Player")[1].transform.GetChild (1).gameObject.GetComponent<Light> ().intensity = 0;
			this.transform.GetChild (1).gameObject.GetComponent<Light> ().intensity = 1;
			this.transform.GetChild (0).gameObject.GetComponent<Camera> ().GetComponent<CameraController> ().render = false;

		}



}
}
