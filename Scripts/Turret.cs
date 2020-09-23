using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public float _rotationSpeed;

    public bool _inputIsEnabled = true;

    public float friendlySaved = 0;
    public float friendlyKilled = 0;
    public float enemyEntered = 0;

    protected Rigidbody mRigidbody;

    public AudioSource _turretSFX;
    public AudioClip _clipShotFired;
    public AudioClip _clipTurretExplode;
    protected float mOriginalPitch;
    public float _pitchRange = 0.2f;

    public TurretFiringSystem mTurretShot;

    protected string mHorizontalAxisInputName = "Horizontal";
    protected float mHorizontalInputValue = 0f;

    protected string mFireInputName = "Fire";

    public enum State {
        IDLE = 0,
        MOVING,
        WON,
        DEATH,
        INACTIVE
    }
    protected State mState;

    public delegate void TurretDestroyed(Turret target);
    public TurretDestroyed dTurretDestroyed;

    public delegate void TurretWon(Turret target);
    public TurretWon dTurretWon;

    public GameObject _turretExplosion;

    private TurretAgent mTurretAgent;

    void Awake()
    {
        mRigidbody = GetComponent<Rigidbody>();
        mTurretShot = GetComponent<TurretFiringSystem>();
        mTurretAgent = GetComponent<TurretAgent>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.IDLE:
            case State.MOVING:
                if (_inputIsEnabled)
                {
                    // Capture movement input
                    MovementInput();
                    // Capture firing input
                    FireInput();
                }
                break;
            case State.DEATH:
                mTurretAgent.EndEpisode();
                Death();
                break;
            case State.WON:
                mTurretAgent.EndEpisode();
                gameObject.SetActive(false);
                mRigidbody.isKinematic = true;
                _inputIsEnabled = false;
                break;
            case State.INACTIVE:
                gameObject.SetActive(false);
                mRigidbody.isKinematic = true;
                _inputIsEnabled = false;
                break;
            default:
                break;
        }
    }

    protected void MovementInput()
    {
        mHorizontalInputValue = Input.GetAxis(mHorizontalAxisInputName);

        if (Mathf.Abs(mHorizontalInputValue) > 0.1f)
            state = State.MOVING;
        else state = State.IDLE;
    }

    protected void FireInput()
    {
        if (Input.GetButton(mFireInputName))
            if (mTurretShot.Fire())
                PlaySFX(_clipShotFired);
    }

    public void Rotate()
    {
        float rotationDegree = _rotationSpeed * Time.deltaTime * mHorizontalInputValue;
        Quaternion rotQuat = Quaternion.Euler(0f, rotationDegree, 0f);
        mRigidbody.MoveRotation(mRigidbody.rotation * rotQuat);
    }

    void FixedUpdate()
    {
        Rotate();
    }

    protected void Death()
    {
        PlaySFX(_clipTurretExplode);
        GameObject turretExplosion = Instantiate(_turretExplosion, transform.position, transform.rotation);
        Destroy(turretExplosion, 1);
        StartCoroutine(ChangeState(State.INACTIVE, 1f));
    }

    private IEnumerator ChangeState(State state, float delay)
    {
        yield return new WaitForSeconds(delay);

        this.state = state;
    }

    public void PlaySFX(AudioClip clip)
    {
        _turretSFX.clip = clip;
        _turretSFX.pitch = mOriginalPitch + Random.Range(-_pitchRange, _pitchRange);
        _turretSFX.Play();
    }

    public void Restart(Vector3 pos, Quaternion rot)
    {
        // Reset position, rotation and health
        transform.position = pos;
        transform.rotation = rot;
        friendlySaved = 0;
        enemyEntered = 0;
        friendlyKilled = 0;

        // Diable kinematic and activate the gameobject and input
        mRigidbody.isKinematic = false;
        gameObject.SetActive(true);
        _inputIsEnabled = true;

        // Change state
        state = State.IDLE;
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
                    case State.IDLE:
                        break;
                    case State.MOVING:
                        break;
                    case State.DEATH:
                        mTurretAgent.EndEpisode();
                        Death();
                        break;
                    case State.WON:
                        mTurretAgent.EndEpisode();
                        gameObject.SetActive(false);
                        dTurretWon.Invoke(this);
                        mRigidbody.isKinematic = true;
                        _inputIsEnabled = false;
                        break;
                    case State.INACTIVE:
                        gameObject.SetActive(false);
                        dTurretDestroyed.Invoke(this);
                        mRigidbody.isKinematic = true;
                        _inputIsEnabled = false;
                        break;
                    default:
                        break;
                }
                
                mState = value;
            }
        }
    }

    public void EnemyEntered(){
        if (mState != State.INACTIVE || mState != State.DEATH || mState != State.WON)
        {
            enemyEntered++;
            mTurretAgent.AddReward(-1.0f);
            GameObject manager = GameObject.Find("Manager");
            TurretManager tm = manager.GetComponent<TurretManager>();
            if (enemyEntered >= tm.EnemyEnterToLose)
            {
                tm.ClearAllUnits();
                state = State.DEATH;
            }
                
        }
    }

    public void FriendlySaved(){
        if (mState != State.INACTIVE || mState != State.DEATH|| mState != State.WON)
        {
            friendlySaved++;
            mTurretAgent.AddReward(1.0f);
            GameObject manager = GameObject.Find("Manager");
            TurretManager tm = manager.GetComponent<TurretManager>();
            if (friendlySaved >= tm.FriendlySaveToWin)
            {
                state = State.WON;
            }
                
        }
    }

    public void FriendlyKilled(){
        if (mState != State.INACTIVE || mState != State.DEATH|| mState != State.WON)
        {
            friendlyKilled++;
            mTurretAgent.AddReward(-1.0f);
            GameObject manager = GameObject.Find("Manager");
            TurretManager tm = manager.GetComponent<TurretManager>();
            if (friendlyKilled >= tm.FriendlyKilledToLose)
            {
                state = State.DEATH;
            }
                
        }
    }
}
