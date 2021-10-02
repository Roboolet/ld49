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
            if (paused) Exit(); else Enter();
        }
    }

    public void Enter()
    {
        paused = true;
        LeanTween.moveY(cam, screenCenter.position.y + 24, 1.5f).setEaseInOutBack();
    }

    public void Exit()
    {
        paused = false;
        LeanTween.moveY(cam, screenCenter.position.y, 1.5f).setEaseInOutBack();
    }
}
