using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor_Manager : MonoBehaviour
{
    public static Cursor_Manager m_Cursor_Manager;
    public Camera Cam;
    SpriteRenderer THIS;
    // Start is called before the first frame update

    void Start()
    {
        Cursor.visible = false;
        THIS = GetComponent<SpriteRenderer>();
        m_Cursor_Manager = this;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Cam.ScreenToWorldPoint(Input.mousePosition);
        transform.position += new Vector3(0, 0, 110);

        if (Cursor.visible) Cursor.visible = false;
    }

    public void SetCursor(Sprite C)
    {
        THIS.sprite = C;
    }
}
