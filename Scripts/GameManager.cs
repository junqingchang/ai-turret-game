using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public TurretManager _turretManager;

    public enum State
    {
        GameLoads = 0,
        GamePrep,
        GameLoop,
        GameEnds
    };
    private State mState = State.GameLoads;

    // Start is called before the first frame update
    void Start()
    {
        _turretManager.dOnGameComplete = OnGameEnd;

        state = State.GamePrep;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnGameEnd()
    {
        state = State.GameEnds;
    }


    private void InitGamePrep()
    {
        // Initialize all tanks
        _turretManager.Restart();

        // Change state to game loop
        state = State.GameLoop;
    }

    private IEnumerator InitGameEnd()
    {
        // Delay before starting a new round
        yield return new WaitForSeconds(3f);

        // Reinitialize tanks
        state = State.GamePrep;
    }

    public State state
    {
        get { return mState; }
        set
        {
            if(mState != value)
            {
                mState = value;

                switch (value)
                {
                    case State.GamePrep:
                        InitGamePrep();
                        break;

                    case State.GameLoop:
                        break;

                    case State.GameEnds:
                        StartCoroutine(InitGameEnd());
                        break;

                    default:
                        break;
                }
            }
        }
    }
}
