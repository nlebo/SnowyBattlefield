using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridsInfo : MonoBehaviour
{
    public int x,y;
    public Inventory_GridManager Inven;

    private void Start() {
        Inven = transform.parent.GetComponent<Inventory_GridManager>();
    }
}
