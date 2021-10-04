using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSwitcher : MonoBehaviour
{
    [SerializeField] Transform screenCenter;
    [SerializeField] LaunchControl control;
    [SerializeField] AsteroidCatcher catcher;
    [SerializeField] ScoreTracker score;

    List<GameObject> currentSceneObjects = new List<GameObject>();

    GameObject cam;

    int levelCurrent;
    int lastLevel = 1;

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
            NextLevel();
        }

    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Transform root = scene.GetRootGameObjects()[0].transform;
        root.position = screenCenter.position;
        foreach(Transform t in root)
        {
            currentSceneObjects.Add(t.gameObject);
            if (t.CompareTag("Sun")) catcher.sun = t;
        }

        catcher.SetTrackedObjects(currentSceneObjects);
        PlanetoidManager.UpdateScene();
    }

    public void LoadLevel(int i, Vector2 center)
    {

        SceneManager.LoadScene("L" + i, LoadSceneMode.Additive);

    }

    void UnloadLevel(int i)
    {
        if (SceneManager.GetSceneByName("L" + i).isLoaded)
        {
            currentSceneObjects = new List<GameObject>();
            SceneManager.UnloadSceneAsync("L" + i);
        }
    }

    public void NextLevel() => StartCoroutine(SwitchLevel(levelCurrent + 1));
    public void PreviousLevel() => StartCoroutine(SwitchLevel(levelCurrent - 1));
    public void RestartLevel() => StartCoroutine("Restart");
    public void ReturnToMenu()
    {
        lastLevel = levelCurrent;
        StartCoroutine(SwitchLevel(0));
    }
    public void ReturnToLevel() => StartCoroutine(SwitchLevel(lastLevel));

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

    IEnumerator SwitchLevel(int i)
    {
        screenCenter.position = Vector2.right * i * 200;

        catcher.SetTrackedObjects(null);
        LeanTween.moveX(cam, screenCenter.position.x, 2.5f).setEase(LeanTweenType.easeInOutExpo);

        yield return new WaitForSeconds(1.2f);
        UnloadLevel(levelCurrent);
        levelCurrent = i;

        score.SetCurrentLevel(i);
        if (levelCurrent > 0)
        {
            control.NewTry();
            control.canLaunch = true;
            control.inMainMenu = false;

            yield return new WaitForSeconds(0.15f);
            LoadLevel(levelCurrent, screenCenter.position);
        }
        else
        {
            control.canLaunch = false;
            control.inMainMenu = true;
        }

        
    }

    IEnumerator Restart()
    {
        LeanTween.moveY(cam, screenCenter.position.y - 100, 0.8f).setEaseInExpo();

        yield return new WaitForSeconds(1);
        catcher.SetTrackedObjects(null);
        UnloadLevel(levelCurrent);

        LeanTween.moveY(cam, screenCenter.position.y, 2f).setEaseOutExpo();

        yield return new WaitForSeconds(0.15f);

        score.SetCurrentLevel(levelCurrent);
        if (levelCurrent > 0)
        {
            control.NewTry();
            control.canLaunch = true;

            yield return new WaitForSeconds(0.15f);
            LoadLevel(levelCurrent, screenCenter.position);
        }
    }


}
