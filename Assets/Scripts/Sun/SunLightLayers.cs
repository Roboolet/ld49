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
    private GameObject lightLayer;

    private SpriteRenderer sp;
    private Color color;

    // Start is called before the first frame update
    void Start()
    {
        // Getting color of the sun
        sp = GetComponent<SpriteRenderer>();
        color = sp.color;

        // Setting the alfa of the new color to small a amount
        color.a = 0.2f;

        instantiateLayers();
    }

    void instantiateLayers()
    {
        float stepSize = lightRadius / lightLayerAmount;
        for (int i = 1; i <= lightLayerAmount; i++)
        {
            GameObject layer = Instantiate(lightLayer, transform.position, Quaternion.identity);
            layer.transform.SetParent(transform);
            layer.GetComponent<SpriteRenderer>().color = color;
            layer.transform.localScale *= stepSize * i;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
