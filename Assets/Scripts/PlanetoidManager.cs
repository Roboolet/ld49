using System.Collections;
using UnityEngine;

public class PlanetoidManager : MonoBehaviour
{
    public static Vector2[,] predictions;

    private Planetoid[] scene;

    private void Start()
    {
        scene = FindObjectsOfType<Planetoid>();
        StartCoroutine(SlowUpdate());
    }

    private IEnumerator SlowUpdate()
    {
        predictions = PlanetoidPhysics.PredictTrajectory2x(2400, scene);

        yield return new WaitForSeconds(0.1f);

        yield return SlowUpdate();
    }
}
