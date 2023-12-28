using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaBuilder : MonoBehaviour
{
    public Mesh m_WallMesh;
    public Material m_WallMaterial;

    public Mesh m_FloorMesh;
    public Material m_FloorMaterial;

    private char[,] m_Arena;

    void Start()
    {
        m_Arena = new char[,]
        {
            { '#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#' },
            { '#',' ',' ','X','X','X','X',' ','X',' ','X','X','X','X',' ',' ','#' },
            { '#',' ','#','X','#','X','#','X','#','X','#','X','#','X','#',' ','#' },
            { '#','X','X','X','X','X','X','X','X','X','X','X','X','X','X','X','#' },
            { '#','X','#','X','X',' ','#','X','#','X','#',' ','X','X','#','X','#' },
            { '#','X','X',' ','X','X','X',' ','#',' ','X','X','X',' ','X','X','#' },
            { '#','X','#','X','#','X','#','#','#','#','#','X','#','X','#','X','#' },
            { '#','X','X',' ','X','X','X',' ','#',' ','X','X','X',' ','X','X','#' },
            { '#','X','#','X','X',' ','#','X','#','X','#',' ','X','X','#','X','#' },
            { '#',' ','X','X','X','X','X','X','X','X','X','X','X','X','X','X','#' },
            { '#',' ','#','X','#','X','#','X','#','X','#','X','#','X','#',' ','#' },
            { '#',' ',' ','X',' ','X','X',' ','X',' ','X','X','X','X',' ',' ','#' },
            { '#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#' }
        };

        for (int y = 0; y < m_Arena.GetLength(0); y++)
        {
            for (int x = 0; x < m_Arena.GetLength(1); x++)
            {
                char c = m_Arena[y, x];

                if(c == '#')
                {
                    GameObject go = new GameObject();
                    go.transform.position = new Vector3(x, 0, y);
                    go.AddComponent<MeshFilter>();
                    go.AddComponent<MeshRenderer>();
                    go.GetComponent<MeshFilter>().mesh = m_WallMesh;
                    go.GetComponent<MeshRenderer>().material = m_WallMaterial;
                    go.AddComponent<BoxCollider>();
                }
                else
                {
                    GameObject go = new GameObject();
                    go.transform.position = new Vector3(x, 0, y);
                    go.AddComponent<MeshFilter>();
                    go.AddComponent<MeshRenderer>();
                    go.GetComponent<MeshFilter>().mesh = m_FloorMesh;
                    go.GetComponent<MeshRenderer>().material = m_FloorMaterial;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
