using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class KwPlayer : NetworkBehaviour {
	SpriteRenderer m_SpriteRenderer;
	public float moveSpeed = 5;
	public float moveX;
	public float moveY;
    public GameObject player1;
	public GameObject player2;
	public FixedJoystick joystick;

    private Text win;
    private Text lost;
    private Text chaserText;
    private Text runnerText;

    public float c = 0;


    private GameObject bar;


	private Animator anim;

    private float originalHeight;


	public bool playerMoving;
	private Vector2 lastMove;
	private NetworkStartPosition[] spawnPoints;

	public string playerType="chaser";

    public float roundTime = 45f;

    public float leftTime;

    public bool found;



    public override void OnStartLocalPlayer(){
	Invoke ("Reactivate cam",0.5f);
		//CmdToggle ();

	}

	void ReactivateCam(){
		this.gameObject.GetComponentInChildren<Camera> ().enabled = false;
		this.gameObject.GetComponentInChildren<Camera> ().enabled = true;
		}



	// Use this for initialization
	void Start () {
		joystick = FindObjectOfType<FixedJoystick>();
		anim = GetComponent<Animator> ();


        win = GameObject.Find("Won").GetComponent<Text>();
        lost = GameObject.Find("Lost").GetComponent<Text>();
        runnerText = GameObject.Find("Runner").GetComponent<Text>();
        chaserText = GameObject.Find("Chaser").GetComponent<Text>();

        win.color = Color.clear;
        lost.color = Color.clear;
        runnerText.color = Color.clear;
        chaserText.color = Color.clear;

        leftTime = roundTime;



        bar = GameObject.Find("Bar");

        originalHeight = bar.GetComponent<RectTransform>().sizeDelta.y;
        

        if (isLocalPlayer) {
			this.transform.GetChild(0).gameObject.GetComponent<Camera>().enabled=true;	
			spawnPoints = FindObjectsOfType<NetworkStartPosition> ();
		
			if (anim.bodyPosition.x < 1.0) {
				CmdToggle ();
				Debug.Log (playerType);
			} else {
				Debug.Log (playerType);
			}


			PointLightToggle (playerType);
            DirectionalLightToggle(playerType);

		} else {
			
			this.transform.GetChild(0).gameObject.GetComponent<Camera>().enabled=false;	
		}
	}

	// Update is called once per frame
	void Update () {
		if (!isLocalPlayer) {
			
			return;
		}

        moveX = joystick.Horizontal;
        moveY = joystick.Vertical;

        if (GameObject.FindGameObjectsWithTag("Player").Length != 2)
        {
            leftTime = roundTime;
        }
        else
        {

			if (playerType == "chaser") {
				GameObject[] playersA = GameObject.FindGameObjectsWithTag("Player");
				GameObject playera = playersA[0];
				GameObject playerb = playersA[1];
				if (playera.GetComponent<KwPlayer> ().playerType == "hider") {
					playerb.GetComponent<SpriteRenderer> ().material.color = Color.black;
				} else {
					playera.GetComponent<SpriteRenderer> ().material.color = Color.black;
				}
			}


            leftTime -= Time.deltaTime;
        }
       
        if (roundTime - leftTime < 5)
        {
            instructionTextDisplay(runnerText,chaserText,playerType);
        }
        else
        {
            Debug.Log("Arrive");
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            Debug.Log("pass");
            
            player1 = players[0];
            player2 = players[1];

            runnerText.color = Color.clear;
            chaserText.color = Color.clear;

            if (!found)
            {
                found = (detectCollision(player1, player2));
            }
            
            if (found |player1.transform.position.z==0.1f|player2.transform.position.z==0.1f)
            {
                if (c == 0)
                {
                    this.transform.position += new Vector3(0, 0, 0.1f);
                }

                moveX = 0.00001f;
                moveY = 0.00001f;
                c+=1f;
                Debug.Log(c);
                
             
            }
 
            if ((player1.transform.position.z==0.1f | player2.transform.position.z==0.1f)&c==10f)
            {
               
                if (playerType == "hider")
                {
                    displayResult(win, lost, false);
                    //gameOver(5);
                }
                else
                {
                    displayResult(win, lost, true);
                    //gameOver(5);
                }
            }

           
        }
        
        if (leftTime <= 0)
        {
            if(playerType == "hider")

            {
                displayResult(win, lost, true);
                //Debug.Log()
                //gameOver(5);
            }
            else
            {
                displayResult(win, lost, false);
                //gameOver(5);
            }
        }


		MoveAndRotate ();

        float leftTimeRatio = leftTime / roundTime;
        shortenTimeBar(bar,originalHeight,leftTimeRatio);

	}


	void MoveAndRotate(){

	

		playerMoving = false;

        

		if (moveX != 0 || moveY != 0) 
		{
            updateTransform(moveX,moveY,transform);
			playerMoving = true;
			lastMove = new Vector2 (moveX, moveY);	// only updated when moved
		}
        
		Debug.DrawRay (Vector2.zero, new Vector2(3,0), Color.red);


	
		anim.SetFloat ("MoveX", moveX);
		anim.SetFloat ("MoveY", moveY);
		anim.SetBool ("PlayerMoving", playerMoving);
		anim.SetFloat ("LastMoveX", lastMove.x);
		anim.SetFloat ("LastMoveY", lastMove.y);

	}

    [Command]
    void CmdFound()
    {
        found = true;
    }

    [Command]
	void CmdToggle()
	{
		playerType = "hider";
	}

	public void PointLightToggle(string playerType)
	{
		if(playerType == "hider") {
			
			this.transform.GetChild (1).gameObject.GetComponent<Light> ().intensity = 0;

		Debug.Log("Toggle invoked");
		}else{

			GameObject.FindGameObjectsWithTag("Player")[0].transform.GetChild (1).gameObject.GetComponent<Light> ().intensity = 0;
			GameObject.FindGameObjectsWithTag("Player")[1].transform.GetChild (1).gameObject.GetComponent<Light> ().intensity = 0;
			this.transform.GetChild (1).gameObject.GetComponent<Light> ().intensity = 1;
			

		}

    }

    public void DirectionalLightToggle(string playerType)
    {
        if (playerType == "hider")
        {
            this.transform.GetChild(0).gameObject.GetComponent<Camera>().GetComponent<CameraController>().render = true;
        }
        else
        {
            this.transform.GetChild(0).gameObject.GetComponent<Camera>().GetComponent<CameraController>().render = false;
        }
    }

    public void updateTransform(float moveX, float moveY, Transform transform)
    {

        Vector3 movementVector = new Vector3(
                moveX * moveSpeed * Time.deltaTime * (1.0f - 0.2f * Mathf.Abs(moveY)),  // (1.0f-0.2f) is to calibrate diagonal move speed
                moveY * moveSpeed * Time.deltaTime * (1.0f - 0.2f * Mathf.Abs(moveX)));
        transform.Translate(movementVector);
    }


    public void shortenTimeBar(GameObject timeBar, float orignialHeight,float timeLeftRatio)
    {
        var rectTransform = timeBar.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, orignialHeight * timeLeftRatio);
    }

    public void instructionTextDisplay(Text runnerText, Text chaserText, string playerType)
    {
        if(playerType == "hider")
        {
            runnerText.color = Color.white;
        }
        else
        {
            chaserText.color = Color.white;
        }
    }

 
    public void displayResult(Text win, Text lost, bool winGame)
    {
        if (winGame)
        {
            win.color = Color.white;
        }
        else
        {
            lost.color = Color.white;
        }
    }



    public void gameOver(float sleepSeconds)
    {
       
        SceneManager.LoadScene("Menu");
    }


    //return true if in sight
    public bool detectCollision(GameObject player1, GameObject player2)
    {
        RaycastHit2D hit = Physics2D.Raycast(player1.transform.position, player2.transform.position-player1.transform.position);
        if (hit.collider.gameObject == player2)
        {
            if (hit.distance < 1f)
            {
                return true;
               

            }
        }
        return false;
    }

}
