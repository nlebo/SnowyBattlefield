using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Move : MonoBehaviour {

    public float MoveSpeed = 30;
	Vector3 MousePos;
	Vector3 MouseWPos;
	Vector3 BasePos;
	float BaseSize;
	public bool Event = false;
	bool ScrollBtn;
	public static Camera_Move m_Camera_Move;

	// Use this for initialization
	void Start () {
		ScrollBtn = false;
		m_Camera_Move = this;
	}

    // Update is called once per frame
    void Update()
    {

        if (!Event)
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel") * MoveSpeed;
            MouseWPos = UI_MANAGER.m_UI_MANAGER.UICamera.ScreenToViewportPoint(Input.mousePosition);

            if (MouseWPos.y >= 1)
                transform.Translate(new Vector3(0, 1, 0) * MoveSpeed * Time.deltaTime);

            else if (MouseWPos.y <= 0f)
                transform.Translate(new Vector3(0, -1, 0) * MoveSpeed * Time.deltaTime);


            if (MouseWPos.x <= 0)
                transform.Translate(new Vector3(-1, 0, 0) * MoveSpeed * Time.deltaTime);
            else if (MouseWPos.x >= 1)
                transform.Translate(new Vector3(1, 0, 0) * Time.deltaTime * MoveSpeed);

            if (Input.GetMouseButtonDown(2))
            {
                MousePos = UI_MANAGER.m_UI_MANAGER.UICamera.ScreenToWorldPoint(Input.mousePosition);
                BasePos = transform.position;
                ScrollBtn = true;
            }
            else if (Input.GetMouseButtonUp(2)) ScrollBtn = false;

            if (ScrollBtn)
            {
                Vector3 Change = UI_MANAGER.m_UI_MANAGER.UICamera.ScreenToWorldPoint(Input.mousePosition) - MousePos;

                transform.position = BasePos - new Vector3(Change.x, Change.y, 0);
            }

            transform.Translate(new Vector3(Input.GetAxis("Horizontal") * MoveSpeed * Time.deltaTime, Input.GetAxis("Vertical") * MoveSpeed * Time.deltaTime));

            if (Camera.main.orthographicSize > 2 && scroll > 0) Camera.main.orthographicSize -= MoveSpeed * Time.deltaTime;
            if (Camera.main.orthographicSize < 12 && scroll < 0) Camera.main.orthographicSize += MoveSpeed * Time.deltaTime;
        }
	}

	public void ShotAction(Vector3 PPos,Vector3 EPos)
	{
		StartCoroutine(ShotCameraMove(PPos,EPos));
	}

	public void ActionZoomIn(Vector3 Pos)
	{
		//Event = true;
		Pos.z = -10;
		transform.position = Pos;
		StartCoroutine(ZoomIn(4,0.2f));

	}
	public void ActionZoomOut()
	{
		StartCoroutine(ZoomOut(BaseSize,0.2f));
	}

	IEnumerator ZoomIn(float Z,float t)
	{
		float dT = 0;
		BaseSize =  Camera.main.orthographicSize;
		while(dT < t)
		{
			dT += Time.deltaTime;
			Camera.main.orthographicSize = Mathf.Lerp(BaseSize,Z,dT/t);
			yield return null;
		}
		Camera.main.orthographicSize = Z;
		yield return null;
	}

	IEnumerator ZoomOut(float Z,float t)
	{
		float dT = 0;
		BaseSize =  Camera.main.orthographicSize;
		while(dT < t)
		{
			
			dT += Time.deltaTime;
			Camera.main.orthographicSize = Mathf.Lerp(BaseSize,Z,dT/t);
			yield return null;
		}

		Camera.main.orthographicSize = Z;
		//Event = false;
		yield return null;
	}

	public IEnumerator CameraMove(Vector3 A, Vector3 B,float t)
	{
		A.z = transform.position.z;
		B.z = transform.position.z;
		float DT = 0;
		while(DT < t)
		{
			DT += Time.deltaTime;
			transform.position = new Vector3(Mathf.Lerp(A.x,B.x,DT/t),Mathf.Lerp(A.y,B.y,DT/t),transform.position.z);
			yield return null;
		}
		
		transform.position = B;
		yield return null;
	}
	IEnumerator ShotCameraMove(Vector3 PPos,Vector3 EPos)
	{
		Event = true;
		ActionZoomIn(PPos);
		yield return new WaitForSeconds(1.3f);

		StartCoroutine(CameraMove(PPos,EPos,0.7f));
		yield return new WaitForSeconds(1.3f);

		ActionZoomOut();
		Event = false;
		yield return null;
	}
}
