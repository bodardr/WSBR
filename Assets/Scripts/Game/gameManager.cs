using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class gameManager : MonoBehaviour {

    List<GameObject> players = new List<GameObject>(4);

    public List<GameObject> availableClasses = new List<GameObject>();

    GameObject crystal;

    public GameObject crystalPrefab;

    BetaGenerator generator;

	// Use this for initialization
	void Start ()
    {
        generator = GetComponent<BetaGenerator>();
        Initialise();
	}

    /// <summary>
    /// Initialisation of the game.
    /// </summary>
    public void Initialise()
    {
        //For now we instantiate the mage.

        generator.InitialiseSeed();
        generator.Generate(transform.Find("Map").GetComponent<Tilemap>());

        GameObject playerOne = Instantiate(availableClasses.Find(x => x.name == AvailableClasses.FireCaster.ToString()),
            generator.SpawningPosition + new Vector2(1, 1), Quaternion.identity);

        players.Add(playerOne);

        crystal = Instantiate(crystalPrefab, 
            new Vector3(generator.SpawningPosition.x,generator.SpawningPosition.y), Quaternion.identity);

         GameObject.Find("Player1VirtualCamera").GetComponent<CinemachineVirtualCamera>().Follow = players[0].transform; 
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}

public enum AvailableClasses
{
    FireCaster
}
