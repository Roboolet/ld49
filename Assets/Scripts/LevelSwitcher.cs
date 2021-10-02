using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSwitcher : MonoBehaviour
{
    [SerializeField] LevelData[] levels;
    [SerializeField] Transform vcam;

    [Header("Prefabs")]
    [SerializeField] GameObject sunPrefab;
    [SerializeField] GameObject[] planetPrefab;

    List<GameObject> currentSceneObjects = new List<GameObject>();

    public void LoadLevel(int i)
    {
        if (i >= levels.Length) LoadLevel(GenerateLevel()); else LoadLevel(levels[i]);
    }

    void LoadLevel(LevelData data)
    {
        StartCoroutine(CleanupScene(currentSceneObjects));
        currentSceneObjects = new List<GameObject>();

        vcam.position += Vector3.right * 50;
        Vector2 anchor = (Vector2)vcam.position + data.sunOffset;

        currentSceneObjects.Add(Instantiate(sunPrefab, anchor, Quaternion.identity));
        for(int i = 0; i < data.planets.Length; i++)
        {
            PlanetData d = data.planets[i];
            int rnd = Random.Range(0, planetPrefab.Length);

            GameObject obj = Instantiate(planetPrefab[rnd], anchor + d.position, Quaternion.identity);
            //obj.GetComponent<Planetoid>().Launch(d.velocity);
            currentSceneObjects.Add(obj);
        }

    }

    LevelData GenerateLevel()
    {

        return new LevelData();
    }

    IEnumerator CleanupScene(List<GameObject> obj)
    {
        yield return new WaitForSeconds(1);
        foreach(GameObject go in obj)
        {
            Destroy(go);
        }
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
