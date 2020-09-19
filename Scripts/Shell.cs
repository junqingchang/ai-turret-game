using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : MonoBehaviour
{
    public float _moveSpeed = 15f;
    public LayerMask _explosionMask;
    public float _explosionRadius = 5f;

    public AudioSource _shellSFX;
    public AudioClip _clipShellExplosion;

    protected float mOriginalPitch;
    public float _pitchRange = 0.2f;

    public enum State
    {
        Moving = 0,
        Explode
    }
    protected State mState;
    protected Rigidbody mRigidBody;

    public GameObject _shellExplosion;

    void Awake()
    {
        mRigidBody = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    protected void OnTriggerEnter(Collider other)
    {
        if(state == State.Moving)
            state = State.Explode;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Move()
    {
        Vector3 moveVect = transform.forward * _moveSpeed * Time.deltaTime;
        mRigidBody.MovePosition(mRigidBody.position + moveVect);
    }

    private void FixedUpdate()
    {
        switch (mState)
        {
            case State.Moving:
                Move();
                break;

            case State.Explode:
                break;
            default:
                break;
        }
    }

    protected void PlaySFX(AudioClip clip)
    {
        _shellSFX.clip = clip;
        _shellSFX.pitch = mOriginalPitch + Random.Range(-_pitchRange, _pitchRange);
        _shellSFX.Play();
    }

    public void Explosion()
    {
        PlaySFX(_clipShellExplosion);
        //get all the tanks caught in the explosion
        Collider[] allCollider = Physics.OverlapSphere(transform.position, _explosionRadius, _explosionMask);

        //destroy the shell
        GameObject shellExplosion = Instantiate(_shellExplosion, transform.position, transform.rotation);
        Destroy(gameObject);
        Destroy(shellExplosion, 1);

        //loop through the collider to apply force and damage
        foreach( Collider collider in allCollider)
        {
            Rigidbody trbody = collider.GetComponent<Rigidbody>();
            if (trbody == null)
                continue;

            if (collider.GetComponent<FriendlyUnit>() != null)
            {
                FriendlyUnit fu = collider.GetComponent<FriendlyUnit>();
                fu.Death();
            }
            
            if (collider.GetComponent<EnemyUnit>() != null)
            {
                EnemyUnit eu = collider.GetComponent<EnemyUnit>();
                eu.Death();
            }

        }
        
    }

    public State state
    {
        get { return mState; }
        set
        {
            if (mState != value)
            {
                switch (value)
                {
                    case State.Moving:
                        break;

                    case State.Explode:
                        Explosion();
                        break;

                    default:
                        break;
                }

                mState = value;
            }
        }
    }
    
}
