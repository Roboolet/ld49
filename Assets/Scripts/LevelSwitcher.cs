using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSwitcher : MonoBehaviour
{
    [SerializeField] LevelData[] levels;
    [SerializeField] Transform screenCenter;
    [SerializeField] LaunchControl control;
    [SerializeField] AsteroidCatcher catcher;

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
        currentSceneObjects = new List<GameObject>();

        //Adding objects
        currentSceneObjects.Add(Instantiate(sunPrefab, center, Quaternion.identity));

        for(int i = 0; i < data.planets.Length; i++)
        {
            PlanetData d = data.planets[i];
            int rnd = Random.Range(0, planetPrefab.Length);

            GameObject obj = Instantiate(planetPrefab[rnd], center + d.position, Quaternion.identity);
            Planetoid p = obj.GetComponent<Planetoid>();
            p.velocity = d.velocity;
            p.mass = d.mass;

            currentSceneObjects.Add(obj);
        }

        catcher.SetTrackedObjects(currentSceneObjects);
        PlanetoidManager.UpdateScene();
    }

    public void NextLevel()
    {
        if (!Menu.paused)
        {
            screenCenter.position += Vector3.right * 200;
            StartCoroutine(SceneTransition(currentSceneObjects, levelCurrent));
            //LoadLevel(levelCurrent, screenCenter.position);
            levelCurrent++;
        }
    }

    IEnumerator SceneTransition(List<GameObject> obj, int level)
    {
        catcher.SetTrackedObjects(null);
        LeanTween.moveX(cam, screenCenter.position.x, 2.5f).setEase(LeanTweenType.easeInOutExpo);

        yield return new WaitForSeconds(1.2f);
        foreach(GameObject go in obj)
        {
            Destroy(go);
        }
        control.NewTry();
        control.canLaunch = true;

        yield return new WaitForSeconds(0.15f);
        LoadLevel(level, screenCenter.position);
    }
}

[System.Serializable]
public struct LevelData
{
    public float sunRadius;

    public PlanetData[] planets;
}

[System.Serializable]
public struct PlanetData
{
    public float radius, mass;
    public Vector2 position, velocity;
}
