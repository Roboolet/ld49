using UnityEngine;

public class Planetoid : MonoBehaviour
{
    public int ID => gameObject.GetInstanceID();

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
    }

    /// <summary>
    /// Draws the trajectory given by the PlanetoidManager.
    /// </summary>
    public void DrawTrajectory()
    {
        if (lineInstance == null)
            lineInstance = Instantiate(line, transform);

        LineRenderer ren = lineInstance.GetComponent<LineRenderer>();
        ren.positionCount = PlanetoidManager.predictions[ID].Length;

        for (int i = 0; i < PlanetoidManager.predictions[ID].Length; i++)
            ren.SetPosition(i, PlanetoidManager.predictions[ID][i]);
    }
}
