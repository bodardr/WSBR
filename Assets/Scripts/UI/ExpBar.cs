using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpBar : MonoBehaviour {

    public GameObject Player;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.Find("LevelBarFull").GetComponent<Image>().fillAmount = (float)((float)Player.GetComponent<CharacterStats>().Exp / (float)Player.GetComponent<CharacterStats>().MaxExp);
        transform.Find("Level").GetComponent<Text>().text = Player.GetComponent<CharacterStats>().Level.ToString();
    }
}
