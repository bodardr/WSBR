using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySpawner : MonoBehaviour {

    public GameObject enemy;

    public int probability = 300;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Random.Range(0, probability) >= probability - 1)
            Instantiate(enemy, transform.position + (Vector3)Random.insideUnitCircle.normalized * 2, Quaternion.identity);		
	}
}
