using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static bool mapGenerated = false;
    private static bool hasKey = false;

    public static bool MapGenerated { get => mapGenerated; set => mapGenerated = value; }
    public static bool HasKey { get => hasKey; set => hasKey = value; }
}
