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

    [HideInInspector] public bool canLaunch;

    private void Awake()
    {
        screenMiddle = new Vector2(Screen.width/2, Screen.height/2);
        NewTry();
        canLaunch = false;

        var sh = psystem.shape;
        sh.radius = distance;
    }

    private void Update()
    {
        if (canLaunch && !Menu.paused)
        {
            pltd.transform.position = launchPoint;
            pltd.isDisabled = true;

            if (!psystem.isPlaying) psystem.Play();

            if (Input.GetButtonUp("Fire1"))
            {
                if (pltd.velocity.sqrMagnitude > 0.0005f)
                    FireAsteroid();
                else
                {
                    pltd.velocity = Vector2.zero;
                }
            }

            if (!Input.GetButton("Fire1"))
            {
                launchPoint = CalculateLaunchPoint(Input.mousePosition) + (Vector2)transform.position;
                //pltd.velocity = Vector2.zero;
            }

            else
            {
                launchAngle = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - launchPoint;
                pltd.velocity = Vector2.ClampMagnitude(-launchAngle * 0.01f, 0.15f);
            }

        }
        else if (psystem.isPlaying) psystem.Stop();

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

        pltd.isDisabled = false;
        canLaunch = false;
    }

    Vector2 CalculateLaunchPoint(Vector2 input)
    {
        Vector2 dir = (input - screenMiddle).normalized;
        if (dir == Vector2.zero) dir = Vector2.left;
        return dir * distance;
    }
}
