using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class PlanetoidPhysics
{
    /// Universal Gravitational Constant
    public const float G = 0.0000006674f;
    public const float Near = 0.1f;

    /// <summary>
    /// Get the magnitude of the force between two planetary bodies.
    /// </summary>
    /// <param name="m2">The mass of the second body.</param>
    /// <param name="r">The distance between the two bodies.</param>
    /// <returns>The magnitude of the force between the two bodies.</returns>
    public static float GetForceBetween(float m2, float r) => G * (m2 / Mathf.Pow(r, 2));

    /// <summary>
    /// Get the force to apply to a planetoid based on the scene around it.
    /// </summary>
    /// <param name="p">The planetoid to calculate for.</param>
    /// <param name="px">All the planetoids in the scene.</param>
    /// <returns>The force to add to the velocity of the planetoid.</returns>
    public static Vector2 GetSceneForce(Planetoid p, params Planetoid[] px)
    {
        // Clear out any destroyed planetoids.
        px = px.Where(f => f != null).ToArray();

        Vector2 force = Vector2.zero;

        // Loop over all planets in the scene:
        foreach (Planetoid p2 in px)
        {
            if (p2.ID != p.ID && Vector2.Distance(p2.position, p.position) > Near) // If they're not the same planet, Add its force to the sum.
                force += (p2.position - p.position).normalized * GetForceBetween(p2.mass, Vector2.Distance(p2.position, p.position));
        }

        return force;
    }

    /// <summary>
    /// Get the force to apply to a planetoid based on the scene around it.
    /// </summary>
    /// <param name="p">The planetoid to calculate for.</param>
    /// <param name="px">All the planetoids in the scene.</param>
    /// <returns>The force to add to the velocity of the planetoid.</returns>
    public static Vector2 GetSceneForce(Vector2 p, float mass, int id, params Planetoid[] px)
    {
        // Clear out any destroyed planetoids.
        px = px.Where(f => f != null).ToArray();

        Vector2 force = Vector2.zero;

        // Loop over all planets in the scene:
        foreach (Planetoid p2 in px)
        {
            if (p2.ID != id && p2.position != p && Vector2.Distance(p2.position, p) > Near) // If they're not the same planet, Add its force to the sum.
                force += (p2.position - p).normalized * GetForceBetween(p2.mass, Vector2.Distance(p2.position, p));
        }

        return force;
    }

    /// <summary>
    /// Predict the trajectory of a planetoid.
    /// </summary>
    /// <param name="p">The planetoid to predict for.</param>
    /// <param name="records">The number of positions to record.</param>
    /// <param name="px">All the planetoids in the scene.</param>
    /// <returns>A prediction of the planetoids trajectory.</returns>
    public static Vector2[] PredictTrajectory(Planetoid p, int records, params Planetoid[] px)
    {
        // Clear out any destroyed planetoids.
        px = px.Where(f => f != null).ToArray();

        // Create the record:
        List<Vector2> record = new List<Vector2>();

        // Keep track of the velocity and position:
        Vector2 velocity = p.velocity;
        Vector2 position = p.position;

        // Collect all the records:
        for (int i = 0; i < records; i++)
        {
            velocity += GetSceneForce(position, p.mass, p.ID, px);
            position += velocity;
            record.Add(position);
        }

        return record.ToArray();
    }

    /// <summary>
    /// Predict the trajectory of all planetoids.
    /// </summary>
    /// <param name="records">The number of positions to record.</param>
    /// <param name="px">All the planetoids in the scene.</param>
    /// <returns>A prediction of the planetoids trajectories.</returns>
    public static Dictionary<int, Vector2[]> PredictTrajectory2x(int records, params Planetoid[] px)
    {
        // Clear out any destroyed planetoids.
        px = px.Where(f => f != null).ToArray();

        // Create the record:
        Dictionary<int, Vector2[]> record = new Dictionary<int, Vector2[]>();

        // Keep track of the velocity and position:
        Vector2[] velocities = px.Select(f => f.velocity).ToArray();
        Vector2[] positions = px.Select(f => f.position).ToArray();
        record = px.ToDictionary(f => f.ID, f => new Vector2[records]);

        // Collect all the records:
        for (int i = 0; i < records; i++)
        {
            Vector2[] current_positions = px.Select(f => f.position).ToArray();

            // Loop over all planets in the scene:
            for (int j = 0; j < px.Length; j++)
            {
                if (px[j].isStatic == false)
                {
                    px[j].velocity += GetSceneForce(current_positions[j], px[j].mass, px[j].ID, px);
                    // Check in case there are non numbers in the velocity.
                    if (!float.IsNaN(px[j].velocity.x) && !float.IsNaN(px[j].velocity.y))
                        px[j].transform.position += (Vector3)px[j].velocity;
                    record[px[j].ID][i] = px[j].position;
                }
            }
        }

        // Reset all planets in the scene:
        for (int j = 0; j < px.Length; j++)
        {
            px[j].velocity = velocities[j];
            px[j].transform.position = positions[j];
        }

        return record;
    }
}