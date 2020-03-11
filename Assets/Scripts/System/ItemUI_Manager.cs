using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUI_Manager : ButtonAction
{
    // Start is called before the first frame update
    public int x =0 ,y=0;
    public int sx=1,sy=1;
    public Item_Manager _This;

    public override void InvenDown()
    {
        base.InvenDown();
        Input_Manager.m_InputManager.DragItem = this;
        
    }
}
