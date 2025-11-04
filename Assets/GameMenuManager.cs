using UnityEngine;
using System.Collections;
using System;
using UnityEngine.InputSystem;

public class GameMenuManager : MonoBehaviour
{
    public GameObject menu;
    public GameObject headSet;
    public float spawnDistance = 2;
    public InputActionProperty menuPressed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (menuPressed.action.WasPressedThisFrame())
        {
            menu.SetActive(!menu.activeSelf);
            menu.transform.position = headSet.transform.position + new Vector3(headSet.transform.forward.x, 0, headSet.transform.forward.z).normalized * spawnDistance;
        }
        menu.transform.LookAt(headSet.transform);
        menu.transform.forward *= -1;
        if (Vector3.Distance(headSet.transform.position, menu.transform.position) > 2.5f)
        {
            menu.SetActive(false);
        }
    }
}
