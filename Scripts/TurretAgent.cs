using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class TurretAgent : Agent
{
    private GameObject turret;
    private float _rotationSpeed;
    protected Rigidbody mRigidbody;
    

    // Start is called before the first frame update
    void Start()
    {
        turret = GameObject.Find("Turret(Clone)");
        if (turret == null)
        {
            turret = GameObject.Find("Turret Hard(Clone)");
        }
        Turret t = turret.GetComponent<Turret>();
        _rotationSpeed = t._rotationSpeed;
        mRigidbody = GetComponent<Rigidbody>();
    }

    public override void OnEpisodeBegin()
    {

    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Friendly and Enemy Units
        // GameObject manager = GameObject.Find("Manager");
        // TurretManager tm = manager.GetComponent<TurretManager>();
        // int counter = 0;
        // foreach (GameObject friendly in tm.mFriendly)
        // {
        //     if (friendly != null){
        //         sensor.AddObservation(friendly.transform.position);
        //         counter++;
        //     }
            
        //     if(counter == 5)
        //     {
        //         break;
        //     }
        // }
        // while(counter < 5)
        // {
        //     sensor.AddObservation(Vector3.zero);
        //     counter++;
        // }
        // counter = 0;
        // foreach (GameObject enemy in tm.mEnemy)
        // {
        //     if (enemy != null)
        //     {
        //         sensor.AddObservation(enemy.transform.position);
        //         counter++;
        //     }
            
        //     if(counter == 5)
        //     {
        //         break;
        //     }
        // }
        // while(counter < 5)
        // {
        //     sensor.AddObservation(Vector3.zero);
        //     counter++;
        // }

        // Current rotation
        sensor.AddObservation(this.transform.rotation);
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        // Rotate
        float rotationDegree = _rotationSpeed * Time.deltaTime * vectorAction[0];
        Quaternion rotQuat = Quaternion.Euler(0f, rotationDegree, 0f);
        mRigidbody.MoveRotation(mRigidbody.rotation * rotQuat);

        // Fire
        Turret t = turret.GetComponent<Turret>();
        if (vectorAction[1] > 0)
        {   
            if (t.mTurretShot.Fire())
            {
                t.PlaySFX(t._clipShotFired);
            }
        }
            
    }

    public override void Heuristic(float[] actionsOut)
    {
        actionsOut[0] = Input.GetAxis("Horizontal");
        if (Input.GetButton("Fire")){
            actionsOut[1] = 1;
        }else{
            actionsOut[1] = 0;
        }
    }

}
