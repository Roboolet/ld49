using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchControl : MonoBehaviour
{
    [SerializeField] float distance;
    [SerializeField] GameObject asteroidPrefab;
    [SerializeField] ParticleSystem psystem;

    Planetoid pltd;

    Vector2 screenMiddle;
    Vector2 launchPoint, launchAngle;

    public bool canLaunch;

    private void Awake()
    {
        screenMiddle = new Vector2(Screen.width/2, Screen.height/2);
        NewTry();

        var sh = psystem.shape;
        sh.radius = distance;
    }

    private void Update()
    {
        if (canLaunch)
        {
            pltd.transform.position = launchPoint;
            
            if (Input.GetButtonUp("Fire1"))
            {
                FireAsteroid();
            }

            if (!Input.GetButton("Fire1"))
            {
                launchPoint = CalculateLaunchPoint(Input.mousePosition);
                //pltd.velocity = Vector2.zero;
            }

            else
            {
                launchAngle = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - launchPoint;
                pltd.velocity = Vector2.ClampMagnitude(-launchAngle * 0.01f, 0.15f);
            }

        }

        if (Input.GetKeyDown(KeyCode.A)) NewTry();
    }

    public void NewTry()
    {
        if(pltd != null) Destroy(pltd.gameObject);
        pltd = Instantiate(asteroidPrefab, Vector2.up * 100, Quaternion.identity).GetComponent<Planetoid>();

        canLaunch = true;
        pltd.mass = 2500;
        pltd.velocity = Vector2.zero;

        PlanetoidManager.UpdateScene();
    }

    void FireAsteroid()
    {
        /*int rnd = Random.Range(0, asteroids.Length);
        Planetoid launched = Instantiate(asteroids[rnd], position, Quaternion.identity).GetComponent<Planetoid>();
        launched.velocity = Vector2.ClampMagnitude(angle, 0.2f);*/

        canLaunch = false;
    }

    Vector2 CalculateLaunchPoint(Vector2 input)
    {
        Vector2 dir = (input - screenMiddle).normalized;
        return dir * distance;
    }
}
