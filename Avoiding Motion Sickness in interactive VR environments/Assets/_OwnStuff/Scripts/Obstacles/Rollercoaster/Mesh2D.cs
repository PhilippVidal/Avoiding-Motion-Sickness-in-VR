using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Mesh2D : ScriptableObject
{
    [System.Serializable]
    public struct LineMesh2D
    {
        public int StartIndex;
        public int EndIndex;
    }

    public Vertice2D[] m_Vertices;
    public LineMesh2D[] m_Lines;

    public int VertexCount { get { return m_Vertices.Length; } }
    public int LineCount { get { return m_Lines.Length; } }
}


[System.Serializable]
public struct Vertice2D
{
    public Vector2 Position, Normal; 
}