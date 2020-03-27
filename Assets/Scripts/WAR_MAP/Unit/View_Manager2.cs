using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class View_Manager2 : MonoBehaviour
{
    [SerializeField]private LayerMask layerMask;

    public Vector3 Evolve;
    private Mesh mesh;
    private Vector3 origin;
    private float fov;

    private float AddAngle;

    //public Unit_Manager Unit;
    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        //Evolve = new Vector3(0,-1,1);
        fov = 90f;
        //Unit = transform.parent.GetComponent<Unit_Manager>();

    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = Vector3.zero;
        origin = Vector3.zero;
        int rayCount = 50;
        float angle = 0f;
        float angleIncrease = fov / rayCount;
        float angle3dIncrease = 2.8f/rayCount;
        float angle3d = -1.4f;
        float viewDistance = 12;
        Vector3[] Vertices = new Vector3[rayCount +1 +1];
        Vector2[] uv = new Vector2[Vertices.Length];
        int[] triangles = new int[rayCount * 3];
        
        Vector2 v2 = new Vector2(Input_Manager.m_InputManager.pos.x -transform.position.x,Input_Manager.m_InputManager.pos.y - transform.position.y);

        AddAngle = Mathf.Atan2(v2.y,v2.x) * Mathf.Rad2Deg;
        AddAngle += fov / 2;
        Vertices[0] = origin;

        int VertexIndex = 1;
        int triangleIndex = 0;
        for (int i = 0; i <= rayCount; i++)
        {
            Vector3 vertex;
            Evolve.x = angle3d;
            RaycastHit2D raycastHit2D = Physics2D.Raycast(origin, GetVectorFromAngle(angle + AddAngle), viewDistance,layerMask);
            
            Debug.DrawRay(origin,GetVectorFromAngle(angle+ AddAngle),Color.red,0.1f);
            if (raycastHit2D.collider == null)
            {
                vertex = origin + GetVectorFromAngle(angle + AddAngle) * viewDistance;
            }
            else
            {
                vertex = raycastHit2D.point;
                
                
            }

            Vertices[VertexIndex] = vertex;

            if (i > 0)
            {
                triangles[triangleIndex + 0] = 0;
                triangles[triangleIndex + 1] = VertexIndex - 1;
                triangles[triangleIndex + 2] = VertexIndex;

                triangleIndex += 3;
            }

            VertexIndex++;
            angle -= angleIncrease;
            angle3d += angle3dIncrease;
        }

        mesh.vertices = Vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
    }

    Vector3 GetVectorFromAngle(float angle)
    {
        float angleRad = angle * (Mathf.PI/180f);
        return new Vector3(Mathf.Cos(angleRad),Mathf.Sin(angleRad));
    }

    public void SetOrigin(Vector3 origin)
    {
        this.origin = origin;
    }
}
