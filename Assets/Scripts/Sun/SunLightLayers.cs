using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunLightLayers : MonoBehaviour
{

    [Header("Animation Settings")]
    [SerializeField]
    private float animFrequency;
    [SerializeField]
    private float animRange;

    [Header("Layer Settings")]
    [SerializeField]
    private float lightRadius;
    [SerializeField]
    private int lightLayerAmount;
    [SerializeField]
    private GameObject lightLayer;
    [Range(0f, 1f)]
    [SerializeField]
    private float alpha;

    private SpriteRenderer sp;
    private Color color;
    private List<Transform> layers = new List<Transform>();
    private List<Vector3> savedLayers = new List<Vector3>();
    public float progress;
    public bool goingIn;
    private float randomValue;

    // Start is called before the first frame update
    void Start()
    {
        // Getting color of the sun
        sp = GetComponent<SpriteRenderer>();
        color = sp.color;

        // Setting the alfa of the new color to small a amount
        color.a = alpha / lightLayerAmount;

        instantiateLayers();
    }

    void instantiateLayers()
    {
        // Getting increments to land at the radius precisely
        float stepSize = lightRadius / lightLayerAmount;

        // Loop for every wanted layer
        for (int i = 1; i <= lightLayerAmount; i++)
        {
            // Create layer and change the color, alfa, size and parent of it.
            GameObject layer = Instantiate(lightLayer, transform.position, Quaternion.identity);
            layer.transform.SetParent(transform);
            layer.GetComponent<SpriteRenderer>().color = color;
            layer.transform.localScale *= stepSize * i + transform.localScale.x;

            // Add the new layer to the layer collection ;)
            layers.Add(layer.transform);
            savedLayers.Add(layer.transform.localScale);
        }
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        progress += animFrequency / 100 * (goingIn ? 2 : 1);

        for (int i = 0; i < layers.Count; i++)
        {
            float waveValue = 1 - (i / 5);
            float scale = Mathf.Lerp(layers[i].localScale.x,(goingIn ? -animRange - waveValue: animRange + waveValue) + savedLayers[i].x, progress);;
            layers[i].localScale = new Vector2(scale, scale);
        }


        if (progress > 1)
        {
            goingIn = !goingIn;
            progress = 0;
        }
    }
}
