using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Inventory : MonoBehaviour {

    int woodCount = 0;

    Animator woodCountAnim;

    TextMeshProUGUI woodCountText;

    public Transform canvas;

	// Use this for initialization
	void Start () {
        woodCountAnim = canvas.Find("WoodCount").GetComponent<Animator>();
        woodCountText = canvas.Find("WoodCount").Find("WoodCountText").GetComponent<TextMeshProUGUI>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public IEnumerator AddWood()
    {
        woodCount++;
        woodCountText.text = woodCount.ToString();
        woodCountAnim.SetTrigger("Triggered");
        yield return new WaitForEndOfFrame();
        woodCountAnim.ResetTrigger("Triggered");
    }
}
