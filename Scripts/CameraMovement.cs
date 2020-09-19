using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    protected Camera mCamera;
    // Start is called before the first frame update
    void Start()
    {
        mCamera = GetComponentInChildren<Camera>();
        GameObject manager = GameObject.Find("Manager");
        TurretManager tm = manager.GetComponent<TurretManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        // Zoom();
    }
}
