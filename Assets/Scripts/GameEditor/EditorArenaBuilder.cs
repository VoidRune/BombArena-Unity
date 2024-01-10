using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorArenaBuilder : MonoBehaviour
{
    private Plane m_Plane;

    private char[,] m_Arena;
    private Dictionary<Vector2Int, GameObject> m_DestructibleTiles = new Dictionary<Vector2Int, GameObject>();
    public Dictionary<char, ArenaBuilder.ArenaTile> m_Tiles = ArenaBuilder.m_Tiles;

    private Transform m_TileChildTransform;

    void Start()
    {
        //ArenaBuilder.FillTiles();

        m_Plane = new Plane(Vector3.up, Vector3.zero);
        m_TileChildTransform = gameObject.transform.GetChild(0).transform;

        Debug.Log(ArenaBuilder.m_Tiles.Count);
        m_Arena = new char[,]
        {
            { 'V','V','V','#','#','#','#','#','#','#','#','#','#','#','V','V','V' },
            { 'V','V','#',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','#','V','V' },
            { 'V','#',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','#','V' },
            { '#',' ',' ',' ','#',' ',' ','#',' ','#',' ',' ','#',' ',' ',' ','#' },
            { '#',' ',' ','#','#',' ',' ',' ',' ',' ',' ',' ','#','#',' ',' ','#' },
            { '#',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','#' },
            { '#',' ',' ',' ',' ',' ','#','#','#','#','#',' ',' ',' ',' ',' ','#' },
            { '#',' ',' ','#',' ',' ','#','V','V','V','#',' ',' ','#',' ',' ','#' },
            { '#',' ',' ',' ',' ',' ','#','V','V','V','#',' ',' ',' ',' ',' ','#' },
            { '#',' ',' ','#',' ',' ','#','V','V','V','#',' ',' ','#',' ',' ','#' },
            { '#',' ',' ',' ',' ',' ','#','#','#','#','#',' ',' ',' ',' ',' ','#' },
            { '#',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','#' },
            { '#',' ',' ','#','#',' ',' ',' ',' ',' ',' ',' ','#','#',' ',' ','#' },
            { '#',' ',' ',' ','#',' ',' ','#',' ','#',' ',' ','#',' ',' ',' ','#' },
            { 'V','#',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','#','V' },
            { 'V','V','#',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','#','V','V' },
            { 'V','V','V','#','#','#','#','#','#','#','#','#','#','#','V','V','V' }
        };

        for (int y = 0; y < m_Arena.GetLength(0); y++)
        {
            for (int x = 0; x < m_Arena.GetLength(1); x++)
            {
                char c = m_Arena[y, x];

                if (ArenaBuilder.m_Tiles.ContainsKey(c))
                {
                    var m = ArenaBuilder.m_Tiles[c];

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
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float dist;
        if (m_Plane.Raycast(ray, out dist))
        {
            Vector3 p = ray.GetPoint(dist);
            Debug.Log(p);
        }
    }
}
