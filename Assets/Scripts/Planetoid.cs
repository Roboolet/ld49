using UnityEngine;

public class Planetoid : MonoBehaviour
{
    public int ID => gameObject.GetInstanceID();

    [Header("Options")]
    /// <summary>
    /// Whether this planetoid is static.
    /// </summary>
    public bool isStatic;

    /// <summary>
    /// Whether this planetoid should interfere with the world.
    /// </summary>
    public bool isDisabled;

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
    /// Radius of the planetoid.
    /// </summary>
    public float radius => transform.localScale.x;

    /// <summary>
    /// Velocity of the planetoid.
    /// </summary>
    public Vector2 velocity;

    [SerializeField] private GameObject line;

    private GameObject lineInstance;

    private void FixedUpdate()
    {
        if (!isStatic && PlanetoidPhysics.physicsEnabled)
        {
            velocity += PlanetoidPhysics.GetSceneForce(this, PlanetoidManager.scenario, PlanetoidManager.scene);
            // Check in case there are non numbers in the velocity.
            if (!float.IsNaN(velocity.x) && !float.IsNaN(velocity.y))
                transform.position += (Vector3)velocity;
        }
    }

    /// <summary>
    /// Draws the trajectory given by the PlanetoidManager.
    /// </summary>
    public void DrawTrajectory()
    {
        // Destroy the line if planetoid is static.
        if (isStatic && lineInstance != null)
            Destroy(lineInstance);

        if (isStatic)
            return;

        if (lineInstance == null)
        {
            lineInstance = Instantiate(line);
            LineRenderer reninst = lineInstance.GetComponent<LineRenderer>();
            Color planetcolor = GetComponent<SpriteRenderer>().color;
            reninst.colorGradient = new Gradient()
            {
                colorKeys = new GradientColorKey[]
                {
                    new GradientColorKey(planetcolor, 0.0f),
                    new GradientColorKey(planetcolor, 1.0f)
                },
                alphaKeys = new GradientAlphaKey[]
                {
                    new GradientAlphaKey(50.0f / 255.0f, 0.8f),
                    new GradientAlphaKey(0.0f, 1.0f)
                }
            };
        }

        LineRenderer ren = lineInstance.GetComponent<LineRenderer>();
        ren.positionCount = PlanetoidManager.predictions[ID].Length / 4;

        // Set the positions on the line:
        for (int i = 0; i < PlanetoidManager.predictions[ID].Length; i += 4)
        {
            if (ren.positionCount > i / 4)
                ren.SetPosition(i / 4, PlanetoidManager.predictions[ID][i]);
        }
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying == false && PlanetoidManager.predictions != null && PlanetoidManager.predictions.ContainsKey(ID))
        {
            Gizmos.color = Color.white;
            for (int i = 0; i < PlanetoidManager.predictions[ID].Length - 4; i += 4)
                Gizmos.DrawLine(PlanetoidManager.predictions[ID][i], PlanetoidManager.predictions[ID][i + 4]);
        }
    }

    private void OnDestroy()
    {
        if(lineInstance != null)
            Destroy(lineInstance);
    }
}
