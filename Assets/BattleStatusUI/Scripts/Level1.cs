using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1 : MonoBehaviour {

    //Debug - List
    public List<Enemy> enemyList;
    Enemy e;
    Enemy e2;

    void Awake () {
        //Debug - List
        enemyList = new List<Enemy>();
        e = new Enemy() { Name = "Solder1", Type = Enemy.EnemyType.type1 };
        e2 = new Enemy() { Name = "Solder2", Type = Enemy.EnemyType.type2 };
        enemyList.Add(e);
        enemyList.Add(e2);
        Debug.Log("EnemyList's capacity = " + enemyList.Count);
        Debug.Log("Return Object = " + enemyList.IndexOf(e2));
        Debug.Log(enemyList[enemyList.IndexOf(e2)].Name);
    }
	
	void Update () {
		
	}
}
