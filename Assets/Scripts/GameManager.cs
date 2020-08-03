using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static bool mapGenerated = false;
    private static bool hasKey = false;
    private static bool levelStarted = false;

    public static bool MapGenerated { get => mapGenerated; set => mapGenerated = value; }
    public static bool HasKey { get => hasKey; set => hasKey = value; }
    public static bool LevelStarted { get => levelStarted; set => levelStarted = value; }
}
