using UnityEngine;
using System.Collections;

public class TheStack : MonoBehaviour {

    public GameObject tilePrefab;
    public GameObject currentTile;
    public int scoreCount;
    public int comboCount;
    public bool isGameOver;

	void Start () {
        scoreCount = 0;
        comboCount = 0;
        isGameOver = false;

        InstantiateTile();
	}

	void Update () {

        if (!isGameOver) {

            if (Input.GetKeyDown(KeyCode.Space)) {
                PlaceTile();
                if (scoreCount >= 12) {
                    RemoveLowestTile();
                }
            }
        }


	}

    public void IncreaseComboCount() {
        comboCount++;
        Debug.Log("Combo! " + comboCount);
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
        newTile.transform.localScale = new Vector3(currentTile.transform.localScale.x, 1.0f, currentTile.transform.localScale.z);

        if (currentTile.name != "Tile0") {
            newTile.GetComponent<TileScript>().ChangeTileBounds(currentTile.GetComponent<TileScript>().tileBounds);
        }        

        ModifyTilePosition(newTile);

        if ((scoreCount + 1) % 2 == 0) {
            newTile.GetComponent<TileScript>().movingOnX = true;
        } else {
            newTile.GetComponent<TileScript>().movingOnX = false;
        }

        ChangeCurrentTile(newTile);
        newTile.transform.position = currentTile.transform.position;
    }
    
    void DropStack () {
        transform.position -= new Vector3(0, 1, 0);
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

    public void GameOver() {
        isGameOver = true;
        Debug.Log("Game over!");
    }

}
