using System.Collections.Generic;
using UnityEngine;

public class Planetoid : MonoBehaviour
{
    [Header("Unique ID")]
    /// This ID must be unqiue!
    public int ID;

    [Header("Properties")]
    /// <summary>
    /// Mass of the planetoid.
    /// </summary>
    public float mass;

    /// <summary>
    /// Position of the planetoid.
    /// </summary>
    public Vector2 position => transform.position;

    /// <summary>
    /// Velocity of the planetoid.
    /// </summary>
    public Vector2 velocity;

    [SerializeField] private GameObject line;
    private GameObject lineInstance;
    private Planetoid[] scene;

    private void Start()
    {
        scene = FindObjectsOfType<Planetoid>();
    }

    private void FixedUpdate()
    {
        velocity += PlanetoidPhysics.GetSceneForce(this, scene);
        transform.position += (Vector3)velocity;

        DrawTrajectory();
    }

    private void DrawTrajectory()
    {
        if (lineInstance == null)
            lineInstance = Instantiate(line, transform);

        LineRenderer ren = lineInstance.GetComponent<LineRenderer>();
        ren.positionCount = PlanetoidManager.predictions.GetLength(1);

        for (int i = 0; i < PlanetoidManager.predictions.GetLength(1); i++)
        {
            ren.SetPosition(i, PlanetoidManager.predictions[ID, i]);
        }
    }

    //private void OnDrawGizmos()
    //{
    //    if (Application.isPlaying)
    //    {
    //        for (int i = 0; i < PlanetoidManager.predictions.GetLength(1) - 1; i++)
    //        {
    //            Gizmos.color = Color.white;
    //            Gizmos.DrawLine(PlanetoidManager.predictions[ID, i], PlanetoidManager.predictions[ID, i + 1]);
    //        }
    //    }
    //}
}
