using UnityEngine;
using System.Collections;

public class TheStack : MonoBehaviour {

    public GameObject tilePrefab;
    public GameObject currentTile;
    public int scoreCount;

	void Start () {
        scoreCount = 0;
        InstantiateTile();
	}

	void Update () {
        if (Input.GetKeyDown(KeyCode.Space)) {
            scoreCount++;
            currentTile.GetComponent<TileScript>().move = false;
            DropStack();
            InstantiateTile();            

            if (scoreCount >= 12) {
                RemoveLowestTile();            
            }            
        }
                
	}

    void ChangeCurrentTile(GameObject tile) {
        currentTile = tile;
    }

    void InstantiateTile () {        
        GameObject newTile = Instantiate(tilePrefab, gameObject.transform) as GameObject;
        newTile.name = "Tile" + (scoreCount + 1);
        newTile.GetComponent<TileScript>().movingOnX = ((scoreCount + 1) % 2 == 0 ? true : false);
        ChangeCurrentTile(newTile);
    }
    
    void DropStack () {
        transform.position -= new Vector3(0, 1, 0);
        //transform.position = Vector3.Lerp(transform.position, Vector3.down * scoreCount, 0.025f);
    }

    void RemoveLowestTile () {
        Destroy(GameObject.Find("Tile" + (scoreCount - 11)) );
    }

}
