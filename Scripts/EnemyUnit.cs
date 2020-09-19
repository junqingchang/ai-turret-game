using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : MonoBehaviour
{
    protected Rigidbody mRigidbody;

    // Start is called before the first frame update
    void Start()
    {
        mRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        Move();
    }

    public void Move()
    {
        GameObject manager = GameObject.Find("Manager");
        TurretManager tm = manager.GetComponent<TurretManager>();
        Vector3 moveVect = transform.forward * tm.EnemySpeed * Time.deltaTime;
        mRigidbody.MovePosition(mRigidbody.position + moveVect);

        GameObject turret = GameObject.Find("Turret(Clone)");
        float distance = Vector3.Distance(this.transform.position, turret.transform.position);
        // -2.5 for the turret size
        if (distance-2.5 <= tm.reachDistance)
        {
            Turret t = turret.GetComponent<Turret>();
            t.EnemyEntered();
            Destroy(gameObject);
        }

    }

    public void Death()
    {
        Destroy(gameObject);
    }

}
