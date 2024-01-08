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

    private Dictionary<Vector2Int, GameObject> m_DestructibleTiles = new Dictionary<Vector2Int, GameObject>();

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
    public class Explosion
    {
        public float timeOfExplosion;
        public Vector2Int position;
        public Vector2Int direction;
        public int popagation;
    }
    private List<Explosion> m_ExplosionQueue = new List<Explosion>();

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
                    if (c != ' ')
                        go.AddComponent<BoxCollider>();

                    go.transform.parent = m_TileChildTransform;
                    go.name = c.ToString();

                    m_DestructibleTiles[new Vector2Int(x, y)] = go;
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

                Explosion ex0 = new Explosion();
                ex0.timeOfExplosion = Time.time;
                ex0.position = pos;
                ex0.direction = new Vector2Int(0, 0);
                ex0.popagation = 0;

                Explosion ex1 = new Explosion();
                ex1.timeOfExplosion = Time.time;
                ex1.position = new Vector2Int(pos.x-1, pos.y);
                ex1.direction = new Vector2Int(-1, 0);
                ex1.popagation = 4;

                Explosion ex2 = new Explosion();
                ex2.timeOfExplosion = Time.time;
                ex2.position = new Vector2Int(pos.x+1, pos.y);
                ex2.direction = new Vector2Int(1, 0);
                ex2.popagation = 4;

                Explosion ex3 = new Explosion();
                ex3.timeOfExplosion = Time.time;
                ex3.position = new Vector2Int(pos.x, pos.y-1);
                ex3.direction = new Vector2Int(0,-1);
                ex3.popagation = 4;

                Explosion ex4 = new Explosion();
                ex4.timeOfExplosion = Time.time;
                ex4.position = new Vector2Int(pos.x, pos.y+1);
                ex4.direction = new Vector2Int(0, 1);
                ex4.popagation = 4;

                m_ExplosionQueue.Add(ex0);
                m_ExplosionQueue.Add(ex1);
                m_ExplosionQueue.Add(ex2);
                m_ExplosionQueue.Add(ex3);
                m_ExplosionQueue.Add(ex4);

                //Instantiate(m_ExplosionPrefab, new Vector3(pos.x, 0, pos.y), Quaternion.identity, m_ExplosionChildTransform);
                //m_BombQueue.Remove(entry.Key);
            }

        }

        foreach (Vector2Int key in valuesToDelete)
        {
            m_BombQueue.Remove(key);
        }

        List<Explosion> newExplosionQueue = new List<Explosion>();
        foreach (Explosion ex in m_ExplosionQueue)
        {
            if(ex.timeOfExplosion <= Time.time)
            {
                Vector2Int pos = ex.position;
                char tile = m_Arena[pos.y, pos.x];
                if (tile == '#')
                {
                    continue;
                }

                Instantiate(m_ExplosionPrefab, new Vector3(ex.position.x, 0, ex.position.y), Quaternion.identity, m_ExplosionChildTransform);
                
                if(tile == 'X')
                {
                    Destroy(m_DestructibleTiles[pos]);
                    m_Arena[pos.y, pos.x] = ' ';

                    GameObject go = new GameObject();
                    go.transform.position = new Vector3(pos.x, 0, pos.y);
                    go.AddComponent<MeshFilter>();
                    go.AddComponent<MeshRenderer>();
                    go.GetComponent<MeshFilter>().mesh = m_Tiles[' '].Mesh;
                    go.GetComponent<MeshRenderer>().material = m_Tiles[' '].Material;
                    go.transform.parent = m_TileChildTransform;
                    go.name = " ";

                    m_DestructibleTiles[pos] = go;

                    continue;
                }
                // Propagate the explosion
                if(ex.popagation > 0)
                {
                    Explosion ex0 = new Explosion();
                    ex0.timeOfExplosion = ex.timeOfExplosion + 0.1f;
                    ex0.position = pos + ex.direction;
                    ex0.direction = ex.direction;
                    ex0.popagation = ex.popagation - 1;

                    newExplosionQueue.Add(ex0);
                }

            }
            else
            {
                newExplosionQueue.Add(ex);
            }
        }
        m_ExplosionQueue = newExplosionQueue;
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
