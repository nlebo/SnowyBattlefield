using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Move : MonoBehaviour {

    public float MoveSpeed = 30;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		float scroll = Input.GetAxis("Mouse ScrollWheel") * MoveSpeed;
        transform.Translate(new Vector3(Input.GetAxis("Horizontal") * MoveSpeed * Time.deltaTime, Input.GetAxis("Vertical") * MoveSpeed * Time.deltaTime));

		if (Camera.main.orthographicSize > 2 && scroll > 0) Camera.main.orthographicSize -= MoveSpeed * Time.deltaTime;
		if (Camera.main.orthographicSize < 12 && scroll < 0) Camera.main.orthographicSize += MoveSpeed * Time.deltaTime;
    }
}
