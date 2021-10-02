using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSwitcher : MonoBehaviour
{
    [SerializeField] LevelData[] levels;
    [SerializeField] Transform screenCenter;

    [Header("Prefabs")]
    [SerializeField] GameObject sunPrefab;
    [SerializeField] GameObject[] planetPrefab;

    List<GameObject> currentSceneObjects = new List<GameObject>();

    GameObject cam;

    int levelCurrent;
    int counter;

    private void Awake()
    {
        cam = Camera.main.gameObject;
    }

    private void Update()
    {
        //debug
        if (Input.GetKeyDown(KeyCode.Space))
        {
            NextLevel();
            counter++;
        }

    }

    public void LoadLevel(int i, Vector2 center)
    {
        LoadLevel(levels[i], center);
    }

    void LoadLevel(LevelData data, Vector2 center)
    {
        //Initialize
        StartCoroutine(CleanupScene(currentSceneObjects));
        currentSceneObjects = new List<GameObject>();

        //Transition        
        LeanTween.moveX(cam, center.x, 2.5f).setEase(LeanTweenType.easeInOutExpo);

        //Adding objects
        Vector2 anchor = center + data.sunOffset;
        currentSceneObjects.Add(Instantiate(sunPrefab, anchor, Quaternion.identity));

        for(int i = 0; i < data.planets.Length; i++)
        {
            PlanetData d = data.planets[i];
            int rnd = Random.Range(0, planetPrefab.Length);

            GameObject obj = Instantiate(planetPrefab[rnd], anchor + d.position, Quaternion.identity);
            Planetoid p = obj.GetComponent<Planetoid>();
            p.velocity = d.velocity;
            p.mass = d.mass;

            currentSceneObjects.Add(obj);
        }

        PlanetoidManager.UpdateScene();
    }

    public void NextLevel()
    {
        if (!Menu.paused)
        {
            screenCenter.position += Vector3.right * 100;
            LoadLevel(levelCurrent, screenCenter.position);
            levelCurrent++;
        }
    }

    LevelData GenerateLevel()
    {

        return new LevelData();
    }

    IEnumerator CleanupScene(List<GameObject> obj)
    {
        yield return new WaitForSeconds(3);
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
    public float radius, mass;
    public Vector2 position, velocity;
}
