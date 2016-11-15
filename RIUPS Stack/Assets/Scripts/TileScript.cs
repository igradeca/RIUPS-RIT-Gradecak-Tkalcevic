using UnityEngine;
using System.Collections;

public class TileScript : MonoBehaviour {
    
    [Range(0.0f, 5.0f)]
    public float tileSpeed = 1.10f;
    public bool move;
    public bool movingOnX = false;

    private const float BOUND_SIZE = 5.8f;
    private float tileTransition;
    
	// Use this for initialization
	void Start () {
        move = true;
	
	}
	
	// Update is called once per frame
	void Update () {
        if (move) {
            MoveTile();
        }     

	}

    void MoveTile() {
        tileTransition += Time.deltaTime * tileSpeed;
        if (movingOnX) {            
            gameObject.transform.position = new Vector3(Mathf.Sin(tileTransition) * BOUND_SIZE, gameObject.transform.position.y, 0);
        } else {
            gameObject.transform.position = new Vector3(0, gameObject.transform.position.y, Mathf.Sin(tileTransition) * BOUND_SIZE);
        }
        
    }

}
