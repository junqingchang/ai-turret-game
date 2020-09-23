using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendlyUnit : MonoBehaviour
{
    protected Rigidbody mRigidbody;

    public GameObject _unitEntered;

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
        Vector3 moveVect = transform.forward * tm.FriendlySpeed * Time.deltaTime;
        mRigidbody.MovePosition(mRigidbody.position + moveVect);

        GameObject turret = GameObject.Find("Turret(Clone)");
        float distance = Vector3.Distance(this.transform.position, turret.transform.position);
        // -2.5 for the turret size
        if (distance-2.5 <= tm.reachDistance)
        {
            Turret t = turret.GetComponent<Turret>();
            t.FriendlySaved();
            GameObject unitAnimation = Instantiate(_unitEntered, transform.position, transform.rotation);
            Destroy(unitAnimation, 1);
            Destroy(gameObject);
        }
    }

    public void Death()
    {
        GameObject turret = GameObject.Find("Turret(Clone)");
        Turret t = turret.GetComponent<Turret>();
        t.FriendlyKilled();
        GameObject manager = GameObject.Find("Manager");
        TurretManager tm = manager.GetComponent<TurretManager>();
        tm.mFriendly.Remove(gameObject);
        Destroy(gameObject);
    }
}
