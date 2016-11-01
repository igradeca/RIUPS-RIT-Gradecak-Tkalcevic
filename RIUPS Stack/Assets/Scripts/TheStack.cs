using UnityEngine;
using System.Collections;

public class TheStack : MonoBehaviour {

    public GameObject TilePrefab;
    public int scoreCount;

	void Start () {
        scoreCount = 0;
	
	}

	void Update () {
        if (Input.GetKeyDown(KeyCode.Space)) {
            PlaceTile();
            DropWholeStack();
            if (scoreCount >= 12) {
                RemoveLowestTile();                
            }            
        }
        	
	}

    void PlaceTile () {
        GameObject NewTile = Instantiate(TilePrefab, gameObject.transform) as GameObject;
        NewTile.name = "Tile" + (scoreCount + 1);
        NewTile.transform.position += new Vector3(0.0f, 0.15f, 0.0f);
        scoreCount++;
    }

    void DropWholeStack() {
        gameObject.transform.position -= new Vector3(0.0f, 0.15f, 0.0f);
    }

    void RemoveLowestTile () {
        Destroy(GameObject.Find("Tile" + (scoreCount - 12)) );
    }

}
