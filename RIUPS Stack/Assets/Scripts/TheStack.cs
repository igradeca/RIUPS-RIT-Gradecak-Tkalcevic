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
            PlaceTile();
            if (scoreCount >= 12) {
                RemoveLowestTile();            
            }            
        }

	}

    void ChangeCurrentTile(GameObject tile) {
        
        currentTile = tile;
    }

    void ModifyTilePosition (GameObject tile) {
        tile.GetComponent<TileScript>().previousTilePositionX = currentTile.transform.position.x;
        tile.GetComponent<TileScript>().previousTilePositionZ = currentTile.transform.position.z;
    }

    void InstantiateTile () {        
        GameObject newTile = Instantiate(tilePrefab, gameObject.transform) as GameObject;        
        newTile.name = "Tile" + (scoreCount + 1);        

        if (currentTile) {
            ModifyTilePosition(newTile);

            if ((scoreCount + 1) % 2 == 0) {
                newTile.GetComponent<TileScript>().movingOnX = true;
            } else {
                newTile.GetComponent<TileScript>().movingOnX = false;
            }
        }
        ChangeCurrentTile(newTile);
        newTile.transform.position = currentTile.transform.position;
    }
    
    void DropStack () {
        transform.position -= new Vector3(0, 1, 0);
        //transform.position = Vector3.Lerp(transform.position, Vector3.down * scoreCount, 0.025f);
    }

    void PlaceTile() {        
        scoreCount++;
        currentTile.GetComponent<TileScript>().move = false;
        currentTile.GetComponent<TileScript>().CutTile();
        DropStack();
        InstantiateTile();
    }

    void RemoveLowestTile () {
        Destroy(GameObject.Find("Tile" + (scoreCount - 12)) );
    }

}
