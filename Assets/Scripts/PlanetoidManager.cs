using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetoidManager : MonoBehaviour
{
    [SerializeField] private int frames = 600;

    public static Dictionary<int, Vector2[]> predictions;
    
    private static Planetoid[] scene;

    private void Start()
    {
        UpdateScene();
        StartCoroutine(SlowUpdate());
    }

    /// <summary>
    /// Should be called when the scene is updated.
    /// Will update the scene references.
    /// </summary>
    public static void UpdateScene() => scene = FindObjectsOfType<Planetoid>();

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
