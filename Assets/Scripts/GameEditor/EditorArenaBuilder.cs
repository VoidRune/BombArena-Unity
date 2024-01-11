using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EditorArenaBuilder : MonoBehaviour
{
    private Plane m_Plane;

    private char[,] m_Arena;
    private Dictionary<Vector2Int, GameObject> m_DestructibleTiles = new Dictionary<Vector2Int, GameObject>();
    public Dictionary<char, GameObject> m_Tiles = new Dictionary<char, GameObject>();

    private GameObject m_Selected;
    private int m_SelectedCharIndex;
    private List<char> m_PossibleTiles = new List<char>();
    private Vector2Int m_LastSelectedPos;

    /* Just so we can set tiles in the editor */
    [System.Serializable]
    public class KeyValue
    {
        public char Key;
        public GameObject Value;
    }

    public KeyValue[] m_KeyValueList;
    public GameObject m_Respawn;

    private Transform m_TileChildTransform;

    public Button m_PlayButton;
    public TMPro.TextMeshProUGUI m_PlayText;
    public GameObject m_ConditionsText;

    void Start()
    {
        foreach (var kv in m_KeyValueList)
        {
            m_Tiles[kv.Key] = kv.Value;

            m_PossibleTiles.Add(kv.Key);
        }
        m_Tiles['S'] = m_Respawn;

        m_Plane = new Plane(Vector3.up, Vector3.zero);
        m_TileChildTransform = gameObject.transform.GetChild(0).transform;

        Debug.Log(ArenaBuilder.m_Tiles.Count);

        if(GlobalVariables.CustomEditorArena != null)
        {
            m_Arena = GlobalVariables.CustomEditorArena;
        }
        else
        {
            m_Arena = new char[,]
            {
                { 'V','V','V','#','#','#','#','#','#','#','#','#','#','#','V','V','V' },
                { 'V','V','#',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','#','V','V' },
                { 'V','#','S',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','S','#','V' },
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
                { 'V','#','S',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','S','#','V' },
                { 'V','V','#',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','#','V','V' },
                { 'V','V','V','#','#','#','#','#','#','#','#','#','#','#','V','V','V' }
            };
        }


        for (int y = 0; y < m_Arena.GetLength(0); y++)
        {
            for (int x = 0; x < m_Arena.GetLength(1); x++)
            {
                char c = m_Arena[y, x];

                if (m_Tiles.ContainsKey(c))
                {
                    var m = m_Tiles[c];

                    m_DestructibleTiles[new Vector2Int(x, y)] = Instantiate(m, new Vector3(x, 0, y), Quaternion.identity, m_TileChildTransform);
                }
            }
        }
        m_SelectedCharIndex = 0;
        m_Selected = Instantiate(m_Tiles[m_PossibleTiles[m_SelectedCharIndex]], new Vector3(0, 0, 0), Quaternion.identity, m_TileChildTransform);
        m_LastSelectedPos = new Vector2Int(0, 0);
    }

    public void PlayCustomArena()
    {
        if(ValidateArena())
        {
            GlobalVariables.CustomArenaRespawnPositions = new List<Vector2Int>();
            for (int y = 0; y < m_Arena.GetLength(0); y++)
            {
                for (int x = 0; x < m_Arena.GetLength(1); x++)
                {
                    char c = m_Arena[y, x];

                    if (c == 'S')
                    {
                        GlobalVariables.CustomArenaRespawnPositions.Add(new Vector2Int(x, y));
                    }
                }
            }

            GlobalVariables.ArenaMapIndex = 4;
            GlobalVariables.CustomEditorArena = m_Arena;
            Debug.Log("Starting custom map!");
            SceneManager.LoadScene("PlayScene");
        }
        Debug.Log("Invalid map!");
    }

    public void ToMainMenu()
    {
        GlobalVariables.ArenaMapIndex = 4;
        GlobalVariables.CustomEditorArena = m_Arena;
        Debug.Log("To main menu!");
        SceneManager.LoadScene("MainMenu");
    }

    public bool ValidateArena()
    {
        int respawnNumber = 0;
        for (int y = 0; y < m_Arena.GetLength(0); y++)
        {
            for (int x = 0; x < m_Arena.GetLength(1); x++)
            {
                char c = m_Arena[y, x];

                if (c == 'S')
                    respawnNumber++;
                if (c == ' ' || c == 'X' || c == 'S')
                {
                    if(x == 0 || x == m_Arena.GetLength(1) - 1 || y == 0 || y == m_Arena.GetLength(0))
                        return false;
                    else
                    {
                        if(m_Arena[y - 1, x] == 'V' || m_Arena[y + 1, x] == 'V' || m_Arena[y, x - 1] == 'V' || m_Arena[y, x + 1] == 'V')
                        {
                            return false;
                        }
                    }
                }
            }
        }
        if (respawnNumber < 2)
            return false;

        return true;
    }

    void TryPlaceTile(Vector2Int pos, char tile)
    {
        char c = m_Arena[pos.y, pos.x];
        if (c != tile)
        {
            if (m_DestructibleTiles.ContainsKey(pos))
                Destroy(m_DestructibleTiles[pos]);
            if (m_Tiles.ContainsKey(tile) && m_Tiles[tile] != null)
                m_DestructibleTiles[pos] = Instantiate(m_Tiles[tile], new Vector3(pos.x, 0, pos.y), Quaternion.identity, m_TileChildTransform);
            m_Arena[pos.y, pos.x] = tile;
        }

    }
    void Update()
    {
        if (Input.mousePosition.y < 85)
        {
            return;
        }

        bool changed = false;

        if (Input.mouseScrollDelta.y > 0)
        {
            m_SelectedCharIndex++;
            m_SelectedCharIndex %= m_PossibleTiles.Count;

            Destroy(m_Selected);
            m_Selected = Instantiate(m_Tiles[m_PossibleTiles[m_SelectedCharIndex]], Vector3.zero, Quaternion.identity, m_TileChildTransform);
        }
        char selectedChar = m_PossibleTiles[m_SelectedCharIndex];

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float dist;
        if (m_Plane.Raycast(ray, out dist))
        {
            Vector3 p = ray.GetPoint(dist);
            p.x = Mathf.FloorToInt(p.x + 0.5f);
            p.z = Mathf.FloorToInt(p.z + 0.5f);
            p.x = Mathf.Clamp(p.x, 0, m_Arena.GetLength(1) - 1);
            p.z = Mathf.Clamp(p.z, 0, m_Arena.GetLength(0) - 1);

            if (m_Selected != null)
                m_Selected.transform.position = p;

            Vector2Int pos = new Vector2Int((int)p.x, (int)p.z);

            if (Input.GetMouseButton(0))
            {
                TryPlaceTile(pos, selectedChar);
                changed = true;
            }
            else if(Input.GetMouseButton(1))
            {
                if (m_Selected != null)
                    m_Selected.transform.position = new Vector3(-1000, 0, -1000);

                TryPlaceTile(pos, 'V');
                changed = true;
            }
            else if(Input.GetKey(KeyCode.E))
            {
                if (m_Selected != null)
                    m_Selected.transform.position = new Vector3(-1000, 0, -1000);

                //Respawn
                TryPlaceTile(pos, 'S');
                changed = true;
            }

            if (m_LastSelectedPos != pos)
            {
                if (m_DestructibleTiles.ContainsKey(m_LastSelectedPos) && m_DestructibleTiles[m_LastSelectedPos] != null)
                    m_DestructibleTiles[m_LastSelectedPos].SetActive(true);
            }

            if (m_DestructibleTiles.ContainsKey(pos) && m_DestructibleTiles[pos] != null)
                m_DestructibleTiles[pos].SetActive(false);

            m_LastSelectedPos = pos;
        }

        if(changed)
        {
            if(ValidateArena())
            {
                m_PlayButton.interactable = true;
                Color col1 = m_PlayText.color;
                col1.a = 1.0f;
                m_PlayText.color = col1;

                m_ConditionsText.SetActive(false);
            }
            else
            {
                m_PlayButton.interactable = false;
                Color col1 = m_PlayText.color;
                col1.a = 0.2f;
                m_PlayText.color = col1;

                m_ConditionsText.SetActive(true);
            }
        }
    }
}
