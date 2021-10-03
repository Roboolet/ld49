using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidCatcher : MonoBehaviour
{
    int score;
    List<GameObject> trackedObjects = new List<GameObject>();
    [SerializeField, Range(1, 30)] float sunCircle, boundsCircle;

    private void Update()
    {
        //slow update
        if(Time.frameCount % 6 == 0)
        {
            for(int i = 0; i <trackedObjects.Count; i++)
            {
                GameObject go = trackedObjects[i];

                float dist = Vector2.Distance(transform.position, go.transform.position);
                if (dist < sunCircle) OnSunEnter(go);
                else if (dist > boundsCircle) OnRadiusExit(go);
            }
        }
    }

    public void SetTrackedObjects(List<GameObject> list)
    {
        score = 0;

        if (list != null && list.Count > 0)
        {
            foreach (GameObject go in list)
            {
                if(go.CompareTag("Planet") || go.CompareTag("Asteroid"))
                {
                    trackedObjects.Add(go);
                }
            }
        }
        else trackedObjects = new List<GameObject>();
    }

    void OnRadiusExit(GameObject go)
    {
        DestroyListItem(go);
    }

    void OnSunEnter(GameObject go)
    {
        DestroyListItem(go);
    }

    void DestroyListItem(GameObject go)
    {
        trackedObjects.Remove(go);
        if (go.CompareTag("Planet"))
        {
            score++;
        }
        print("Destroyed " + go.name);
    }
}
