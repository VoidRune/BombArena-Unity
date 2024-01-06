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

    public GameObject m_BombPrefab;
    public GameObject m_ExplosionPrefab;

    private char[,] m_Arena;
    /* Just so we can set tiles in the editor */
    [System.Serializable]
    public class KeyValue
    {
        public char Key;
        public ArenaTile Value;
    }

    public KeyValue[] m_KeyValueList;

    public class Bomb
    {
        public float timeOfExplosion;
        public GameObject obj;
    }
    private Dictionary<Vector2Int, Bomb> m_BombQueue = new Dictionary<Vector2Int, Bomb>();

    private Transform m_TileChildTransform;
    private Transform m_BombChildTransform;
    private Transform m_ExplosionChildTransform;
    void Start()
    {
        m_TileChildTransform = gameObject.transform.GetChild(0).transform;
        m_BombChildTransform = gameObject.transform.GetChild(1).transform;
        m_ExplosionChildTransform = gameObject.transform.GetChild(2).transform;

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
                    //if(c != ' ')
                    if (c == '#')
                        go.AddComponent<BoxCollider>();

                    go.transform.parent = m_TileChildTransform;
                    go.name = c.ToString();
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        List<Vector2Int> valuesToDelete = new List<Vector2Int>();

        foreach (KeyValuePair<Vector2Int, Bomb> entry in m_BombQueue)
        {
            if(entry.Value.timeOfExplosion <= Time.time)
            {
                Vector2Int pos = entry.Key;
                valuesToDelete.Add(entry.Key);
                m_Arena[pos.y, pos.x] = ' ';

                Instantiate(m_ExplosionPrefab, new Vector3(pos.x, 0, pos.y), Quaternion.identity, m_ExplosionChildTransform);
                //m_BombQueue.Remove(entry.Key);
            }

        }

        foreach (Vector2Int key in valuesToDelete)
        {
            m_BombQueue.Remove(key);
        }
    }

    public GameObject TryPlaceBomb(int x, int z)
    {
        if(m_Arena[z, x] == ' ')
        {
            m_Arena[z, x] = 'B';
            GameObject b = Instantiate(m_BombPrefab, new Vector3(x, 0, z), Quaternion.identity, m_BombChildTransform);
            Bomb bomb = new Bomb();
            bomb.timeOfExplosion = Time.time + 2.0f;
            bomb.obj = b;
            Vector2Int pos = new Vector2Int(x, z);
            m_BombQueue.Add(pos, bomb);

            return b;
        }

        return null;
    }
}
