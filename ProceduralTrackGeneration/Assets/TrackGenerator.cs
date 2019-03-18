using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackGenerator : MonoBehaviour
{
    public GameObject newOb;
    public int tpSize;
    public TrackPoint[] tPoint;
    public float halfTrackWidth = 5;

    Mesh mesh;
    public Vector3[] verts;
    public int[] tris;


    private void Start()
    {
        //tPoint = new TrackPoint[tpSize];
        mesh = new Mesh();

        GetComponent<MeshFilter>().mesh = mesh;
        Debug.Log("rolling");
        newMethod();
        UpdateMesh();
    }

    void CreateMesh()
    {
        int currentPoint = 0;
        int cVert = 0;
        int cTris = 0;
        TrackPoint next = tPoint[currentPoint +1];
        TrackPoint last = tPoint[tPoint.Length];

        verts = new Vector3[(tPoint.Length * 2)];

        for(int i = 0; i<tPoint.Length; i++)
        {
            TrackPoint t = tPoint[i];
            //idk
            Vector3 midD = new Vector3(next.Position.x - t.Position.x, 0, next.Position.z - t.Position.z); //direction vector of connection of points
            Vector3 midP = new Vector3((t.Position.x + next.Position.x) / 2, 0, (t.Position.z + next.Position.z) / 2); //position vector between 2 points
            Vector3 midcVec = PerpendicularClockwise(midD) + midP;
            Vector3 midccVec = PerpendicularCounterClockwise(midD) + midP;

            GameObject test = newOb;

            test.transform.position = midcVec += (transform.forward * 5);
            Vector3 rightPoint = newOb.transform.position;
            test.transform.position = midccVec += (transform.forward * 5);
            Vector3 leftPoint = newOb.transform.position;
            Debug.Log("rp is " + rightPoint + " which is 5 away from midpoint at " + midcVec);

            //verts[i] = new Vector3()

            currentPoint++;
            if (currentPoint >= tPoint.Length)
            {
                next = tPoint[0];
                last = t;
            }
            else
            {
                next = tPoint[currentPoint+1];
                last = t;
            }
        }


    }


    void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = verts;
        mesh.triangles = tris;

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

    void newMethod()
    {
        int currentPoint = 0;
        TrackPoint next;
        TrackPoint t;
        verts = new Vector3[tPoint.Length * 2];
        for (int i = 0; i<tPoint.Length; i++)
        {
            t = tPoint[i];
            if ((i + 1) >= tPoint.Length)
            {
                next = tPoint[0];
            }
            else
            {
                next = tPoint[i + 1];
            }
            Vector3 midP = new Vector3((t.Position.x + next.Position.x) / 2, 0, (t.Position.z + next.Position.z) / 2); //position vector between 2 points
            GameObject test = newOb;
            test.transform.position = midP; //move test to mid point postion
            test.transform.LookAt(next.transform.position); //make test look at the next point
            test.transform.position = midP + (test.transform.right * halfTrackWidth); //move test to right of current point
            Vector3 rightPoint = test.transform.position; //set right point as current test point
            verts[currentPoint] = rightPoint;
            test.transform.position = midP + (-test.transform.right * halfTrackWidth); //move test to left of mid poitn
            Vector3 leftPoint = test.transform.position; //set left point as current test postion
            verts[currentPoint + 1] = leftPoint;
            currentPoint += 2;
        }
        tris = new int[tPoint.Length * 6];
        int cTris = 0;
        for (int i = 0; i < verts.Length; i+=2)
        {
            if (i == 14)
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
}
