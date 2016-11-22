using UnityEngine;
using System.Collections;

public class TileScript : MonoBehaviour {
    
    [Range(0.0f, 5.0f)]
    public float tileSpeed = 1.10f;
    public bool move;
    public bool movingOnX = false;
    public float previousTilePositionX = 0;
    public float previousTilePositionZ = 0;

    private const float boundSize = 6.0f;
    private float tileTransition;
    private const float errorMargin = 0.1f;
    private float deltaX;

    void Start () {

        move = true;
	
	}
	
	void Update () {
        if (move) {
            MoveTile();
        }     

	}

    void MoveTile() {
        tileTransition += Time.deltaTime * tileSpeed;
        if (movingOnX) {            
            gameObject.transform.position = new Vector3(-(Mathf.Cos(tileTransition) * boundSize), gameObject.transform.position.y, previousTilePositionZ);
        } else {
            gameObject.transform.position = new Vector3(previousTilePositionX, gameObject.transform.position.y, Mathf.Cos(tileTransition) * boundSize);
        }        
    }

    public void CutTile() {

        if (movingOnX) {
            deltaX = previousTilePositionX - gameObject.transform.position.x;
        } else {
            deltaX = previousTilePositionZ - gameObject.transform.position.z;
        }

        if (Mathf.Abs(deltaX) < errorMargin) {
            Debug.Log("Combo!");
            gameObject.transform.position = new Vector3(previousTilePositionX, gameObject.transform.position.y, previousTilePositionZ);
        } else {
            // CUT TEH PLO�A!
        }
    }

}
