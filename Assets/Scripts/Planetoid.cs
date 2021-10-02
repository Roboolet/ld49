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
    public Vector2 position;
}
