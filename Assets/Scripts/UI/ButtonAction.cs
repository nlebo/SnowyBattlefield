using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonAction : MonoBehaviour
{
    public Camera UICamera;
    Input_Manager _Input;
    public RectTransform _Transform;
    RectTransform _ChildTrans;
    Vector3 MousePos;
    Vector3 Change;
    Vector3 BasePos;
    bool Down;
    // Start is called before the first frame update
    virtual protected void Start()
    {
        _Transform = GetComponent<RectTransform>();

        if(transform.childCount > 0)
        _ChildTrans = transform.GetChild(0).GetComponent<RectTransform>();
        _Input = Input_Manager.m_InputManager;

        if(UI_MANAGER.m_UI_MANAGER != null)
        UICamera = UI_MANAGER.m_UI_MANAGER.UICamera;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void PointDown()
    {
        _Transform.position -= new Vector3(0.01f, 0.01f,0);
    }
    public void PointUp()
    {
        _Transform.position += new Vector3(0.01f, 0.01f, 0);
    }

    public void SetBarDown()
    {
        MousePos = UICamera.ScreenToWorldPoint(Input.mousePosition);
        BasePos = _ChildTrans.position;
    }

    public void SetBarDrag()
    {
        Change = UICamera.ScreenToWorldPoint(Input.mousePosition) - MousePos;

        _ChildTrans.position = BasePos + new Vector3(Change.x, 0, 0);
        
    }

    public void SetBarUp()
    {
        MousePos = Vector3.zero;
        _ChildTrans.position = BasePos;
    }

    public virtual void InvenDown()
    {
        MousePos = UICamera.ScreenToWorldPoint(Input.mousePosition);
        BasePos = _Transform.position;
    }

    public void InvenDrag()
    {
        Change = UICamera.ScreenToWorldPoint(Input.mousePosition) - MousePos;

        _Transform.position = BasePos + new Vector3(Change.x , Change.y, 0);

    }

    public virtual void InvenUp()
    {
        MousePos = Vector3.zero;
    }

    public void SetImage(int a)
    {
        Unit_Manager _unit = Board_Manager.m_Board_Manager.GetUnit(a);
        Camera.main.transform.position = new Vector3(_unit.transform.position.x, _unit.transform.position.y, -10);

    }

    

}
