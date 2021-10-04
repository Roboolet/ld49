using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    GameObject cam;
    [SerializeField] Transform screenCenter;

    public static bool paused;

    private void Awake()
    {
        cam = Camera.main.gameObject;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel") && !LeanTween.isTweening(cam))
        {
            if (paused) MoveCam(0); else MoveCam(60);
            paused = !paused;
        }
    }

    public void MoveCam(float y)
    {
        LeanTween.moveY(cam, screenCenter.position.y + y, 1.5f).setEaseInOutBack();
    }
}
