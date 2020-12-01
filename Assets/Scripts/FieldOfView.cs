using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class FieldOfView : MonoBehaviour
{
    [SerializeField] private LayerMask layermask;
    private Mesh mesh;
    private Vector3 origin;
    float startingAngle;
    public float fov;
    public float viewDistance;
    private void Start()
    {

         mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        origin = Vector3.zero;
        fov = 90f;
        viewDistance = 2.5f;
    }
    private void LateUpdate()
    {


        
        int rayCount = 50;
        float angle = startingAngle;
        float angleIncrease = fov / rayCount;
        

        Vector3[] vertices = new Vector3[rayCount + 1 + 1];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[rayCount * 3];

        vertices[0] = origin;
        int vertexIndex = 1;
        int triangleIndex = 0;
        for (int i = 0; i < rayCount; i++)
        {
            Vector3 vertex;
            RaycastHit2D raycasthit2d = Physics2D.Raycast(origin, UtilsClass.GetVectorFromAngle(angle), viewDistance, layermask);
            if (raycasthit2d.collider == null)
            {
                //no hit
                vertex = origin + UtilsClass.GetVectorFromAngle(angle) * viewDistance;
            }
            else
            {
                //hit object
                vertex = raycasthit2d.point;
               
                
            }
            vertices[vertexIndex] = vertex;
            if (i > 0)
            {
                triangles[triangleIndex + 0] = 0;
                triangles[triangleIndex + 1] = vertexIndex - 1;
                triangles[triangleIndex + 2] = vertexIndex;

                triangleIndex += 3;
            }
            vertexIndex++;
            angle -= angleIncrease;
            
        }

        

       //  mesh.vertices = vertices;
       // mesh.uv = uv;
     //   mesh.triangles = triangles;
    
    
    }
    public void setOrigin(Vector3 origin)
    {
        this.origin = origin;
    }
    public void SetAimDirection(Vector3 aimDirection)
    {
        startingAngle = UtilsClass.GetAngleFromVectorFloat(aimDirection) + fov / 2f;
    }

}
