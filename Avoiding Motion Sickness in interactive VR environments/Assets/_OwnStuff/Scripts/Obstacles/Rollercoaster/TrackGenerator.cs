using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[RequireComponent(typeof(MeshFilter))]
public class TrackGenerator : MonoBehaviour
{

    public float m_Scale = 1.0f;
    public int m_Resolution = 8;
    
    public Spline m_BezierSpline;
    public Mesh2D m_Mesh2D;
    public bool m_GenerateInEditor = false;

    protected Mesh m_Mesh3D;

    protected void Awake()
    {
        if(m_Mesh3D)
            m_Mesh3D.Clear();

        m_Mesh3D = new Mesh();
        GetComponent<MeshFilter>().sharedMesh = m_Mesh3D;
    }


    protected void Start()
    {
        GenerateTrack();
    }

    protected void GenerateTrack()
    {
        if(!m_Mesh3D)
        {
            m_Mesh3D = new Mesh();
            GetComponent<MeshFilter>().sharedMesh = m_Mesh3D;
        }

        Debug.Log("Generating Mesh!"); 
        m_Mesh3D.Clear();

        int segmentCount = m_Resolution * m_BezierSpline.SegmentCount;

        List<Vector3> vertices = new List<Vector3> ();
        List<Vector3> normals = new List<Vector3> ();       
        for (int i = 0; i < segmentCount + 1; i++)
        {
            float t = i / (float)(segmentCount - 1);


            PathPoint point = m_BezierSpline.GetPoint(t);
            Quaternion invRot = Quaternion.Inverse(transform.rotation);
            foreach (Vertice2D vert in m_Mesh2D.m_Vertices)
            {
                vertices.Add(invRot * (point.LocalToWorldPosition(vert.Position * m_Scale) - transform.position));
                normals.Add(invRot * point.LocalToWorldDirection(vert.Normal));
            }


        }


        
        //Debug.Log("Generating Triangles | Vertex Count => " + m_Mesh2D.VertexCount + ", LineCount => " + m_Mesh2D.LineCount); 
        List<int> triangles = new List<int>();
        for (int i = 0; i < segmentCount - 1; i++)
        {
            int currStartIndex = i * m_Mesh2D.VertexCount;
            int nextStartIndex = (i + 1) * m_Mesh2D.VertexCount;


            foreach(Mesh2D.LineMesh2D line in m_Mesh2D.m_Lines)
            {
                int currentStart = currStartIndex + line.StartIndex;
                int currentEnd = currStartIndex + line.EndIndex;

                int nextStart = nextStartIndex + line.StartIndex;
                int nextEnd = nextStartIndex + line.EndIndex;


                // |\
                // | \
                // |  \ 
                // |___\
                triangles.Add(currentStart);
                triangles.Add(nextStart);
                triangles.Add(currentEnd);

                // ____
                // \   |
                //  \  |
                //   \ |
                //    \|

                triangles.Add(currentEnd);
                triangles.Add(nextStart);
                triangles.Add(nextEnd);
            }
        }

        m_Mesh3D.SetVertices(vertices);
        m_Mesh3D.SetNormals(normals);
        m_Mesh3D.SetTriangles(triangles, 0);
        

        //m_Mesh3D.RecalculateNormals();
    }
}
