﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static bool mapGenerated = false;
    private static bool hasKey = false;
    private static bool levelStarted = false;
    private static float soundVolume = 0.5f;
    private static float miniMapAlpha = 0.5f;
    private static bool isInPauseMenu = false;

    public static bool MapGenerated { get => mapGenerated; set => mapGenerated = value; }
    public static bool HasKey { get => hasKey; set => hasKey = value; }
    public static bool LevelStarted { get => levelStarted; set => levelStarted = value; }
    public static float SoundVolume { get => soundVolume; set => soundVolume = value; }
    public static bool IsInPauseMenu { get => isInPauseMenu; set => isInPauseMenu = value; }
    public static float MiniMapAlpha { get => miniMapAlpha; set => miniMapAlpha = value; }
}
