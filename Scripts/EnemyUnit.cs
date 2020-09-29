using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : MonoBehaviour
{
    protected Rigidbody mRigidbody;

    public GameObject _unitEntered;

    public float health;


    // Start is called before the first frame update
    void Start()
    {
        mRigidbody = GetComponent<Rigidbody>();
        GameObject manager = GameObject.Find("Manager");
        TurretManager tm = manager.GetComponent<TurretManager>();
        health = tm.customHealth;

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
        if (turret == null)
        {
            turret = GameObject.Find("Turret Hard(Clone)");
        }
        float distance = Vector3.Distance(this.transform.position, turret.transform.position);
        // -1.5 for the turret size
        if (distance-1.5 <= tm.reachDistance)
        {
            Turret t = turret.GetComponent<Turret>();
            t.EnemyEntered();
            GameObject unitAnimation = Instantiate(_unitEntered, transform.position, transform.rotation);
            Destroy(unitAnimation, 1);
            Destroy(gameObject);
        }

    }

    public void Death()
    {
        GameObject turret = GameObject.Find("Turret(Clone)");
        if (turret == null)
        {
            turret = GameObject.Find("Turret Hard(Clone)");
        }
        TurretAgent agent = turret.GetComponent<TurretAgent>();
        agent.AddReward(1.0f);
        health -= 1;
        if (health <= 0){
            Destroy(gameObject);
        }
    }
}
