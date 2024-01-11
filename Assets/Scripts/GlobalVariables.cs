using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVariables : MonoBehaviour
{
    public static float MasterVolume = 0.5f;
    public static int ArenaMapIndex = 0;
    // ArenaMapIndex = 4 is custom editor map
    public static char[,] CustomEditorArena = null;

}
