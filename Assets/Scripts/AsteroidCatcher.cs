using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidCatcher : MonoBehaviour
{
    List<GameObject> trackedObjects = new List<GameObject>();
    [SerializeField, Range(1, 30)] float sunCircle, boundsCircle;
    [SerializeField] GameObject burnoutParticles;

    [SerializeField] ScoreTracker score;

    private void Update()
    {
        //slow update
        if(Time.frameCount % 4 == 0)
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
        score.ResetScore();

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

    public void AddTrackedObject(GameObject obj)
    {
        trackedObjects.Add(obj);
    }

    public void RemoveTrackedObject(GameObject obj)
    {
        if (trackedObjects.Contains(obj)) trackedObjects.Remove(obj);
    }

    void OnRadiusExit(GameObject go)
    {
        DestroyListItem(go);
    }

    void OnSunEnter(GameObject go)
    {
        DestroyListItem(go);
        Destroy(go);
        Instantiate(burnoutParticles, go.transform.position, Quaternion.identity);
    }

    void DestroyListItem(GameObject go)
    {
        trackedObjects.Remove(go);
        if (go.CompareTag("Planet"))
        {
            score.Increment();
        }
    }
}
