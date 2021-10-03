using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Update()
    {
        //debug
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine("NextLevel");
            counter++;
        }

    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Transform root = scene.GetRootGameObjects()[0].transform;
        root.position = screenCenter.position;
        foreach(Transform t in root)
        {
            currentSceneObjects.Add(t.gameObject);
        }

        catcher.SetTrackedObjects(currentSceneObjects);
        PlanetoidManager.UpdateScene();
    }

    public void LoadLevel(int i, Vector2 center)
    {
        SceneManager.LoadScene("L"+i, LoadSceneMode.Additive);
    }

    void UnloadLevel(int i)
    {
        if (SceneManager.GetSceneByName("L" + i).isLoaded)
        {
            currentSceneObjects = new List<GameObject>();
            SceneManager.UnloadSceneAsync("L" + i);
        }
    }

    /*void LoadLevel(LevelData data, Vector2 center)
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
    }*/

    IEnumerator NextLevel()
    {
        screenCenter.position += Vector3.right * 200;
        catcher.SetTrackedObjects(null);
        LeanTween.moveX(cam, screenCenter.position.x, 2.5f).setEase(LeanTweenType.easeInOutExpo);

        yield return new WaitForSeconds(1.2f);
        UnloadLevel(levelCurrent);
        levelCurrent++;

        control.NewTry();
        control.canLaunch = true;

        yield return new WaitForSeconds(0.15f);
        LoadLevel(levelCurrent, screenCenter.position);
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
