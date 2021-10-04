using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunLightLayers : MonoBehaviour
{
    [SerializeField]
    private float lightRadius;
    [SerializeField]
    private int lightLayerAmount;
    [SerializeField]
    private GameObject lightLayer, parentObject;    
    [Range(0f, 1f)]
    [SerializeField]
    private float alpha;

    private SpriteRenderer sp;
    private ParticleSystem ps;
    private Color color;

    // Start is called before the first frame update
    void Start()
    {
        // Getting color of the sun
        sp = GetComponent<SpriteRenderer>();
        ps = GetComponentInChildren<ParticleSystem>();

        color = sp.color;

        ps.startColor = color;

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
            layer.transform.SetParent(parentObject.transform);
            layer.GetComponent<SpriteRenderer>().color = color;
            layer.transform.localScale *= stepSize * i + transform.localScale.x;
        }
    }
}
