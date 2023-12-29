using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaBuilder : MonoBehaviour
{
    [System.Serializable]
    public class ArenaTile
    {
        public Mesh Mesh;
        public Material Material;
    }

    public Dictionary<char, ArenaTile> m_Tiles = new Dictionary<char, ArenaTile>();

    private char[,] m_Arena;
    /* Just so we can set tiles in the editor */
    [System.Serializable]
    public class KeyValue
    {
        public char Key;
        public ArenaTile Value;
    }

    public KeyValue[] m_KeyValueList;

    void Start()
    {
        foreach (var kv in m_KeyValueList)
        {
            m_Tiles[kv.Key] = kv.Value;
        }

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
                
                if(m_Tiles.ContainsKey(c))
                {
                    var m = m_Tiles[c];

                    GameObject go = new GameObject();
                    go.transform.position = new Vector3(x, 0, y);
                    go.AddComponent<MeshFilter>();
                    go.AddComponent<MeshRenderer>();
                    go.GetComponent<MeshFilter>().mesh = m.Mesh;
                    go.GetComponent<MeshRenderer>().material = m.Material;

                    if(c != ' ')
                        go.AddComponent<BoxCollider>();
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
