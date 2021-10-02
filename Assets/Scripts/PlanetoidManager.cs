using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetoidManager : MonoBehaviour
{
    public static Dictionary<int, Vector2[]> predictions;
    [SerializeField] private int frames = 600;

    private Planetoid[] scene;

    private void Start()
    {
        scene = FindObjectsOfType<Planetoid>();
        StartCoroutine(SlowUpdate());
    }

    private IEnumerator SlowUpdate()
    {
        predictions = PlanetoidPhysics.PredictTrajectory2x(frames, scene);

        yield return new WaitForSeconds(0.1f);

        yield return SlowUpdate();
    }
}
