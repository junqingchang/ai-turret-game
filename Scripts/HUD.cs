using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    private float remainingFriendly = 10;
    public Text rfText;
    public Text fsText;
    public Text gameEndText;

    // Start is called before the first frame update
    void Start()
    {
        GameObject turret = GameObject.Find("Manager");
        TurretManager tm = turret.GetComponent<TurretManager>();
        remainingFriendly = tm.FriendlySaveToWin;
    }

    // Update is called once per frame
    void Update()
    {
        GameObject turret = GameObject.Find("Turret(Clone)");
        if (turret == null)
        {
            turret = GameObject.Find("Turret Hard(Clone)");
        }
        if (turret == null)
        {
            GameObject manager = GameObject.Find("Manager");
            TurretManager tm = manager.GetComponent<TurretManager>();
            rfText.text = "Friend Left to Save: 0";
            fsText.text = "Friendly Killed: 0";
            if (tm.gameEndState == "You Won")
            {
                gameEndText.color = Color.blue;
            }
            else
            {
                gameEndText.color = Color.red;
            }
            gameEndText.text = tm.gameEndState;
        }
        else
        {
            Turret t = turret.GetComponent<Turret>();
            rfText.text = "Friend Left to Save: " + (remainingFriendly - t.friendlySaved);
            fsText.text = "Friendly Killed: " + t.friendlyKilled;
            gameEndText.text = "";
        }
        
    }
}
