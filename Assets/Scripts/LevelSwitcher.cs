using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSwitcher : MonoBehaviour
{
    [SerializeField] LevelData[] levels;

    public void LoadLevel(int i)
    {
        if (i >= levels.Length) LoadLevel(GenerateLevel()); else LoadLevel(levels[i]);
    }

    void LoadLevel(LevelData data)
    {

    }

    LevelData GenerateLevel()
    {

            return new LevelData();
    }
}

[System.Serializable]
public struct LevelData
{
    public Vector2 sunOffset;
    public float sunRadius;

    public PlanetData[] planets;
}

[System.Serializable]
public struct PlanetData
{
    public float radius;
    public Vector2 position, velocity;
}
