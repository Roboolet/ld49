using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LaunchControl : MonoBehaviour
{
    [SerializeField] float distance;
    [SerializeField] GameObject asteroidPrefab;
    [SerializeField] ParticleSystem psystem;
    [SerializeField] Image restartButtonImage;
    [SerializeField] Button restartButton;

    Planetoid pltd;

    Vector2 screenMiddle;
    Vector2 launchPoint, launchAngle;

    [HideInInspector] public bool canLaunch;
    public bool inMainMenu;

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
            restartButtonImage.color = Color.Lerp(restartButtonImage.color, Color.clear, Time.deltaTime*2);
            restartButton.interactable = false;

            pltd.transform.position = launchPoint;
            pltd.isDisabled = true;

            if (!psystem.isPlaying) psystem.Play();

            if (Input.GetButtonUp("Fire1") && !EventSystem.current.IsPointerOverGameObject())
            {
                if (pltd.velocity.sqrMagnitude > 0.00025f)
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
                Time.timeScale = 1;
            }

            else
            {
                launchAngle = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - launchPoint;
                pltd.velocity = Vector2.ClampMagnitude(-launchAngle * 0.01f, 0.15f);
                Time.timeScale = Mathf.Lerp(Time.timeScale, 0.2f, 0.1f);
            }

        }
        else
        {
            Time.timeScale = 1;

            if (psystem.isPlaying) psystem.Stop();

            if (!inMainMenu)
            {
                restartButtonImage.color = Color.Lerp(restartButtonImage.color, Color.white, Time.deltaTime);
                restartButton.interactable = true;
            }
            else
            {
                restartButtonImage.color = Color.Lerp(restartButtonImage.color, Color.clear, Time.deltaTime * 2);
                restartButton.interactable = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.A)) NewTry();
    }

    public void NewTry()
    {
        if(pltd != null) Destroy(pltd.gameObject);
        pltd = Instantiate(asteroidPrefab, Vector2.up * 100, Quaternion.identity).GetComponent<Planetoid>();

        canLaunch = true;
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
