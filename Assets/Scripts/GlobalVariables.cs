using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVariables : MonoBehaviour
{
    public static float MasterVolume = 0.5f;
    public static int ArenaMapIndex = 0;
    // ArenaMapIndex = 4 is custom editor map
    public static char[,] CustomEditorArena = null;
    public static List<Vector2Int> CustomArenaRespawnPositions = null;
    public static float powerupInterval = 2.0f;
    public static int maximumPowerups = 8;

}
