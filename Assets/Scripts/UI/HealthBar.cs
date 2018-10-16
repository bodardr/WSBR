using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

    public GameObject Player;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.Find("HealthBarFull").GetComponent<Image>().fillAmount = (float)((float)Player.GetComponent<CharacterStats>().Health / (float)Player.GetComponent<CharacterStats>().MaxHealth);
    }
}
