using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
public class CameraController : NetworkBehaviour {

	public GameObject followTarget;
	private Vector3 targetPos;
	private float moveSpeed = 6.0f;
	private float offset = -10.0f;

	// Use this for initialization
	void Start () {
		targetPos.z = offset;
	}

	// Update is called once per frame
	void LateUpdate () {
		targetPos.x = followTarget.transform.position.x;
		targetPos.y = followTarget.transform.position.y;
		transform.position = Vector3.Lerp (transform.position,targetPos,moveSpeed*Time.deltaTime);
	}
}
