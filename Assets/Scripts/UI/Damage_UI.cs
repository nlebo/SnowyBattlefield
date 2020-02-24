using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Damage_UI : MonoBehaviour {

	public float LiveTime = 2;
	public float MoveSpeed = 5;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		transform.Translate(new Vector3(0, MoveSpeed * Time.deltaTime, 0));

		LiveTime -= Time.deltaTime;

		if (LiveTime <= 0) Destroy(gameObject);
	}

	public void SetText(string _text)
	{
		GetComponent<TextMesh>().text = _text;
	}
}
