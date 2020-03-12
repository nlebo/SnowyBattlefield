using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Move : MonoBehaviour {

    public float MoveSpeed = 30;
	Vector3 MousePos;
	Vector3 BasePos;
	bool ScrollBtn;
	public static Camera_Move m_Camera_Move;

	// Use this for initialization
	void Start () {
		ScrollBtn = false;
		m_Camera_Move = this;
	}
	
	// Update is called once per frame
	void Update () {
		float scroll = Input.GetAxis("Mouse ScrollWheel") * MoveSpeed;
		

		if(Input.GetMouseButtonDown(2)){
			MousePos = UI_MANAGER.m_UI_MANAGER.UICamera.ScreenToWorldPoint(Input.mousePosition);
			BasePos = transform.position;
			ScrollBtn = true;
		}
		else if(Input.GetMouseButtonUp(2)) ScrollBtn = false;

		if(ScrollBtn){
			 Vector3 Change = UI_MANAGER.m_UI_MANAGER.UICamera.ScreenToWorldPoint(Input.mousePosition) - MousePos;

        	transform.position = BasePos - new Vector3(Change.x , Change.y, 0);
		}

		transform.Translate(new Vector3(Input.GetAxis("Horizontal") * MoveSpeed * Time.deltaTime, Input.GetAxis("Vertical") * MoveSpeed * Time.deltaTime));

		if (Camera.main.orthographicSize > 2 && scroll > 0) Camera.main.orthographicSize -= MoveSpeed * Time.deltaTime;
		if (Camera.main.orthographicSize < 12 && scroll < 0) Camera.main.orthographicSize += MoveSpeed * Time.deltaTime;
    }
}
