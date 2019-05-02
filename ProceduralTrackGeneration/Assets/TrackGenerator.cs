using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackGenerator : MonoBehaviour
{
    public GameObject newOb;
    public int tpSize;
    public TrackPoint[] tPoint;
    public float halfTrackWidth = 5;
    GameObject nodeGrid;

    Mesh mesh;
    //public Vector3[] verts;
    //public int[] tris;
    Vector3[] nPositions;
    Vector3[] vertices;
    int[] triangles;


    private void Start()
    {
        nodeGrid = GameObject.FindGameObjectWithTag("NodeGrid");
        nPositions = nodeGrid.GetComponent<NodeGrid>().nodePositions;
        vertices = new Vector3[nPositions.Length * 2];
        //tPoint = new TrackPoint[tpSize];
        mesh = new Mesh();

        GetComponent<MeshFilter>().mesh = mesh;
        //newMethod();
        CreateMesh();
        UpdateMesh();
    }

    /*
    private void OnDrawGizmos()
    {
        if (vertices == null)
            return;
        for (int i = 0; i < vertices.Length; i++)
        {
            Gizmos.DrawSphere(vertices[i], 3f);
        }
    }*/

    void CreateMesh()
    {
        Vector3 current;
        Vector3 next;

        int i = 0;
        for (int n = 0; n < nPositions.Length; n++)
        {
            current = nPositions[n];
            Debug.Log(n);
            if (n + 1 >= nPositions.Length)
            {
                next = nPositions[0];
            }
            else
            {
                next = nPositions[n + 1];
            }
            Vector3 midPoint = new Vector3((current.x + next.x) / 2, 101, (current.z + next.z) / 2);
            GameObject test = newOb;
            test.transform.position = midPoint; //move test to mid point postion
            test.transform.LookAt(next); //make test look at the next point
            test.transform.position = midPoint + (test.transform.right * halfTrackWidth); //move test to right of current point
            vertices[i] = new Vector3(test.transform.position.x, test.transform.position.y, test.transform.position.z); //set right point as current test point
            test.transform.position = midPoint + (-test.transform.right * halfTrackWidth); //move test to left of mid point
            vertices[i+1] = new Vector3(test.transform.position.x, test.transform.position.y, test.transform.position.z); //set left point as current test postion
            i += 2;
        }

        triangles = new int[(nPositions.Length * 2) * 6];
        int vert = 0;
        int tris = 0;

        for (int p = 0; p < vertices.Length; p+= 2)
        {
            if (p == vertices.Length -2)
            {
                //Debug.Log("here before error");
                triangles[tris + 0] = vert;
                triangles[tris + 1] = vert + 1;
                triangles[tris + 2] = 0;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = 1;
                triangles[tris + 5] = 0;
            }
            else
            {
                triangles[tris + 0] = vert;
                triangles[tris + 1] = vert + 1;
                triangles[tris + 2] = vert + 2;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + 3;
                triangles[tris + 5] = vert + 2;
            }

            vert+=2;
            tris += 6;
        }
        //vert++;
    }


    void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
    }

    //https://gamedev.stackexchange.com/questions/70075/how-can-i-find-the-perpendicular-to-a-2d-vector
    //modified to accept vector 3
    static Vector3 PerpendicularClockwise(Vector3 vector3)
    {
        return new Vector3(vector3.z, 0, -vector3.x);
    }

    static Vector3 PerpendicularCounterClockwise(Vector3 vector3)
    {
        return new Vector3(-vector3.z, 0, vector3.x);
    }

    /*
    void newMethod()
    {
        int currentPoint = 0;
        Vector3 next;
        Vector3 t;
        verts = new Vector3[nPositions.Length * 2];
        for (int i = 0; i< nPositions.Length; i++)
        {
            t = nPositions[i];
            if ((i + 1) >= nPositions.Length)
            {
                next = nPositions[0];
            }
            else
            {
                next = nPositions[i + 1];
            }
            Vector3 midP = new Vector3((t.x + next.x) / 2, 0, (t.z + next.z) / 2); //position vector between 2 points
            GameObject test = newOb;
            test.transform.position = midP; //move test to mid point postion
            test.transform.LookAt(next); //make test look at the next point
            test.transform.position = midP + (test.transform.right * halfTrackWidth); //move test to right of current point
            Vector3 rightPoint = test.transform.position; //set right point as current test point
            verts[currentPoint] = rightPoint;
            test.transform.position = midP + (-test.transform.right * halfTrackWidth); //move test to left of mid poitn
            Vector3 leftPoint = test.transform.position; //set left point as current test postion
            verts[currentPoint + 1] = leftPoint;
            currentPoint += 2;
        }

        tris = new int[nPositions.Length * 6];
        int cTris = 0;
        for (int i = 0; i < verts.Length; i+=2)
        {
            if (i == verts.Length)
            {
                //0
                //1
                //3
                tris[cTris] = i;
                tris[cTris + 1] = i + 1;
                tris[cTris + 2] = 1;

                //0
                //3
                //2
                tris[cTris + 3] = i;
                tris[cTris + 4] = 1;
                tris[cTris + 5] = 0;
            }
            else
            {
                //0
                //1
                //3
                tris[cTris] = i;
                tris[cTris + 1] = i + 1;
                tris[cTris + 2] = i + 3;

                //0
                //3
                //2
                tris[cTris + 3] = i;
                tris[cTris + 4] = i + 3;
                tris[cTris + 5] = i + 2;
            }
            cTris += 6;
        }

    }
    */
}
