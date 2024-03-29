using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlanetoidManager : MonoBehaviour
{
    [SerializeField] private int frames = 600;

    public static Dictionary<int, Vector2[]> predictions;
    
    public static Planetoid[] scene;
    public static Vector2[] scenario;

    static PlanetoidManager main;

    private void Awake()
    {
        if (main == null)
        {
            DontDestroyOnLoad(this);
            main = this;
        }
        else Destroy(gameObject);
    }

    private void Start()
    {
        UpdateScene();
        StartCoroutine(SlowUpdate());
    }

    private void FixedUpdate()
    {
        scene = scene.Where(f => f != null).ToArray();
        scenario = scene.Select(f => f.position).ToArray();
    }

    /// <summary>
    /// Should be called when the scene is updated.
    /// Will update the scene references.
    /// </summary>
    public static void UpdateScene()
    {
        scene = FindObjectsOfType<Planetoid>();
    }

    private IEnumerator SlowUpdate()
    {
        predictions = PlanetoidPhysics.PredictTrajectory2x(frames, scene);

        // Tell the scene to redraw the trajectories:
        for (int i = 0; i < scene.Length; i++)
            if (scene[i] != null)
                scene[i].DrawTrajectory();

        yield return new WaitForSecondsRealtime(0.03f);

        yield return SlowUpdate();
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying == false)
        {
            UpdateScene();
            predictions = PlanetoidPhysics.PredictTrajectory2x(frames, scene);
        }
    }
}
