using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{
    public int meshSizeX = 20;
    public int meshSizeZ = 20;
    public float scale = 1;

    public int triSize = 5;
    Mesh mesh;

    Vector3[] verts;
    Vector2[] uvs;

    int[] tris;

    Color[] cols;
    public Gradient gradient;

    public float bumpFreq;


    private float minTer = 100;
    private float maxTer = 0;
	// Use this for initialization
	void Start ()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        CreateShape();
        UpdateMesh();

    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    void CreateShape()
    {
        float ct = Time.deltaTime;
        verts = new Vector3[(meshSizeX + 1) * (meshSizeZ + 1)];
        float r = Random.Range(0.3f,1.7f);
        float r2 = Random.Range(1.5f, 2f);

        /*
         * ## Loops for construction of terrain mesh, including noise for height of the terrain
         */
        for (int z = 0, i = 0; z <= meshSizeZ * triSize; z+= triSize)
        {
            for (int x = 0; x <= meshSizeX * triSize; x+= triSize)
            {
                
                //float y1 = Mathf.PerlinNoise(x * 0.001f, z * 0.001f) * 10f;
                //float y2 = Mathf.PerlinNoise((float)x / meshSize * scale, (float)z / meshSize * scale);

                float y3 = Mathf.PerlinNoise(x/r * bumpFreq, z/r * bumpFreq) * scale;
                float y2 = Mathf.PerlinNoise(x / r2 * bumpFreq, z / r2 * bumpFreq)* scale/3;
                float y1 = Mathf.PerlinNoise(x * r * 0.000005f, z * r * 0.000005f) * scale;
                float ny = y2 * y3 / 2;

                verts[i] = new Vector3(x, ny, z);
                if (ny > maxTer)
                {
                    maxTer = ny;
                }
                if (ny < minTer)
                {
                    minTer = ny;
                    //Debug.Log(minTer);
                }
                i++;
            }
        }

        
        for (int z = 0, i = 0; z <= meshSizeZ; z++)
        {
            for (int x = 0; x <= meshSizeX; x++)
            {
                verts[i].y = verts[i].y - minTer;
                if (verts[i].y > 100)
                {
                    verts[i].y = 100;
                }
                i++;
            }
        }



        tris = new int[meshSizeX * meshSizeZ * 6];

        int cVert = 0;
        int cTris = 0;

        for (int z = 0; z < meshSizeZ; z++)
        {
            for (int x = 0; x < meshSizeX; x++)
            {
                tris[cTris + 0] = cVert + 0;
                tris[cTris + 1] = cVert+ meshSizeZ + 1;
                tris[cTris + 2] = cVert + 1;

                tris[cTris + 3] = cVert + 1;
                tris[cTris + 4] = cVert + meshSizeZ + 1;
                tris[cTris + 5] = cVert + meshSizeZ + 2;

                cVert++;
                cTris += 6;
            }
            cVert++;

        }

        uvs = new Vector2[verts.Length];

        for (int z = 0, i = 0; z <= meshSizeZ; z++)
        {
            for (int x = 0; x <= meshSizeX; x++)
            {
                uvs[i] = new Vector2((float)x / meshSizeX, (float)z / meshSizeZ);
                i++;
            }
        }

        cols = new Color[verts.Length];

        for (int i = 0, z = 0; z <= meshSizeZ; z++)
        {
            for (int x = 0; x <= meshSizeX; x++)
            {
                float h = Mathf.InverseLerp(minTer, maxTer, verts[i].y);
                cols[i] = gradient.Evaluate(h);

                //uvs[i] = new Vector2((float)x / meshSize, (float)z / meshSize);
                i++;
            }
        }
    }


    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = verts;
        mesh.triangles = tris;
        mesh.uv = uvs;

        mesh.RecalculateNormals();
    }

    /*private void OnDrawGizmos()
    {
        if (verts == null)
            return;

        for (int i = 0; i < verts.Length; i++)
        {
            Gizmos.DrawSphere(verts[i], 0.1f);
        }
    }*/
}
