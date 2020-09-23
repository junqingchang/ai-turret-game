using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretManager : MonoBehaviour
{
    public GameObject _turretPrefab;
    public GameObject turret;

    public GameObject _enemyUnitPrefab;
    public GameObject _friendlyUnitPrefab;
    
    public Vector3 centerPoint;

    public float spawnDistance;
    public float enemySpawnTime = 3f;

    public float reachDistance;
    public float friendlySpawnTime;

    public Color EnemyColor = Color.red;
    public Color FriendlyColor = Color.blue;

    public float EnemySpeed;
    public float FriendlySpeed;

    public float FriendlySaveToWin;
    public float FriendlyKilledToLose;
    public float EnemyEnterToLose;

    public delegate void OnGameComplete();
    public OnGameComplete dOnGameComplete = null;

    protected Coroutine friendlySpawner;
    protected Coroutine enemySpawner;

    public List<GameObject> mFriendly = new List<GameObject>();
    public List<GameObject> mEnemy = new List<GameObject>();

    private void Awake()
    {
        SpawnTurret();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate() {
        
    }

    public void SpawnTurret()
    {
        turret = Instantiate(_turretPrefab, centerPoint, Quaternion.identity);
        Turret t = turret.GetComponent<Turret>();
        t.dTurretDestroyed = OnTurretDeath;
        t.dTurretWon = OnTurretWon;
    }

    public IEnumerator SpawnEnemyUnit(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);

            Vector3 newSpawnPoint;  //new spawn point 
            
            while(true)
            {
                Vector3 random = UnityEngine.Random.insideUnitCircle.normalized * spawnDistance;
                newSpawnPoint = new Vector3 (random.x, 0, random.y);
                var hitColliders = Physics.OverlapSphere(newSpawnPoint, 1);
                if(hitColliders.Length <= 1)
                {
                    break;
                }
            }

            newSpawnPoint += transform.position;
            GameObject newUnit = Instantiate (_enemyUnitPrefab, newSpawnPoint, Quaternion.identity);
            mEnemy.Add(newUnit);
            MeshRenderer rend = newUnit.GetComponent<MeshRenderer>();
            rend.material.color = EnemyColor;
            newUnit.transform.LookAt(turret.transform);
        }
        
    }

    private IEnumerator SpawnFriendlyUnit(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);

            Vector3 newSpawnPoint;  //new spawn point 
            while(true)
            {
                Vector3 random = UnityEngine.Random.insideUnitCircle.normalized * spawnDistance;
                newSpawnPoint = new Vector3 (random.x, 0, random.y);
                var hitColliders = Physics.OverlapSphere(newSpawnPoint, 1);
                if(hitColliders.Length <= 1)
                {
                    break;
                }
            }

            newSpawnPoint += transform.position;
            GameObject newUnit = Instantiate (_friendlyUnitPrefab, newSpawnPoint, Quaternion.identity);
            mFriendly.Add(newUnit);
            MeshRenderer rend = newUnit.GetComponent<MeshRenderer>();
            rend.material.color = FriendlyColor;
            newUnit.transform.LookAt(turret.transform);
        }
        
    }

    public void OnTurretDeath(Turret target)
    {
        StopCoroutine(friendlySpawner);
        StopCoroutine(enemySpawner);
        ClearAllUnits();
        dOnGameComplete.Invoke();
        Turret t = turret.GetComponent<Turret>();
        t._inputIsEnabled = false;
    }

    public void OnTurretWon(Turret target)
    {
        StopCoroutine(friendlySpawner);
        StopCoroutine(enemySpawner);
        ClearAllUnits();
        dOnGameComplete.Invoke();
        Turret t = turret.GetComponent<Turret>();
        t._inputIsEnabled = false;
    }

    public void Restart()
    {
        Turret t = turret.GetComponent<Turret>();
        t.Restart(centerPoint, Quaternion.identity);
        mFriendly = new List<GameObject>();
        mEnemy = new List<GameObject>();
        enemySpawner = StartCoroutine(SpawnEnemyUnit(enemySpawnTime));
        friendlySpawner = StartCoroutine(SpawnFriendlyUnit(friendlySpawnTime));
    }

    public void ClearAllUnits()
    {
        GameObject[] allUnits = GameObject.FindGameObjectsWithTag("unit");
        foreach(GameObject obj in allUnits) {
            Destroy(obj);
        }
    }
    
}
