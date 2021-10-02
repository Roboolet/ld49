using UnityEngine;

public class PlanetoidPhysics
{
    /// Universal Gravitational Constant
    public const float G = 0.00430091f;

    /// <summary>
    /// Get the magnitude of the force between two planetary bodies.
    /// </summary>
    /// <param name="m1">The mass of the first body.</param>
    /// <param name="m2">The mass of the second body.</param>
    /// <param name="r">The distance between the two bodies.</param>
    /// <returns>The magnitude of the force between the two bodies.</returns>
    public float GetForceBetween(float m1, float m2, float r) => G * (m1 * m2 / Mathf.Pow(r, 2));

    /// <summary>
    /// Get the force to apply to a planetoid based on the scene around it.
    /// </summary>
    /// <param name="p">The planetoid to calculate for.</param>
    /// <param name="px">All the planetoids in the scene.</param>
    /// <returns>The force to add to the velocity of the planetoid.</returns>
    public Vector2 GetSceneForce(Planetoid p, params Planetoid[] px)
    {
        Vector2 force = Vector2.zero;

        // Loop over all planets in the scene:
        foreach (Planetoid p2 in px)
        {
            if (p2.ID != p.ID) // If they're not the same planet, Add its force to the sum.
                force += (p2.position - p.position).normalized * GetForceBetween(p.mass, p2.mass, Vector2.Distance(p2.position, p.position));
        }

        return force;
    }

    public Vector2[] PredictTrajectory(Vector2 p, params Planetoid[] px)
    {
        return null;
    }
}