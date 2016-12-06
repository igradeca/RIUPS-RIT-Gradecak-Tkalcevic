using UnityEngine;
using System.Collections;

public class TileScript : MonoBehaviour {
    
    [Range(0.0f, 5.0f)]
    public float tileSpeed = 1.10f;
    public bool move;
    public bool movingOnX = false;
    public float previousTilePositionX = 0;
    public float previousTilePositionZ = 0;
    public TheStack stack;
    public Vector2 tileBounds = new Vector2(5.0f, 5.0f);

    private const float boundSize = 8.0f;
    private float tileTransition;
    private const float errorMargin = 0.3f;
    private float delta;

    void Start () {
        stack = transform.parent.GetComponent<TheStack>();
        //transform.localScale = new Vector3(tileBounds.x, 1.0f, tileBounds.y);
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
            delta = previousTilePositionX - transform.position.x;
        } else {
            delta = previousTilePositionZ - transform.position.z;
        }

        if (Mathf.Abs(delta) < errorMargin) {
            stack.IncreaseComboCount();
            if (stack.comboCount > 4) {
                tileBounds.x += 0.25f;
                tileBounds.y += 0.25f;

                transform.localScale = new Vector3(tileBounds.x, 1.0f, tileBounds.y);
                Debug.Log("Scale increased by combo!");
            }
            
            gameObject.transform.position = new Vector3(previousTilePositionX, gameObject.transform.position.y, previousTilePositionZ);
        } else {
            stack.comboCount = 0;

            if (movingOnX) {
                tileBounds.x -= Mathf.Abs(delta);
                if (tileBounds.x <= 0) {
                    stack.GameOver();
                    gameObject.AddComponent<Rigidbody>();
                    return;
                }

                float middle = previousTilePositionX + transform.position.x / 2;
                transform.localScale = new Vector3(tileBounds.x, 1.0f, tileBounds.y);
                CreateRubble(
                    new Vector3(transform.position.x > 0 ?
                    transform.position.x + (transform.localScale.x / 2) : 
                    transform.position.x - (transform.localScale.x / 2),
                    transform.position.y, 
                    transform.position.z),
                    new Vector3(Mathf.Abs(delta), 1, transform.localScale.z)
                    );
                transform.localPosition = new Vector3(middle - (previousTilePositionX / 2), transform.localPosition.y, previousTilePositionZ);

            } else {
                tileBounds.y -= Mathf.Abs(delta);
                if (tileBounds.y <= 0) {
                    stack.GameOver();
                    gameObject.AddComponent<Rigidbody>();
                    return;
                }

                float middle = previousTilePositionZ + transform.position.z / 2;
                transform.localScale = new Vector3(tileBounds.x, 1.0f, tileBounds.y);
                CreateRubble(
                    new Vector3(
                    transform.position.x,
                    transform.position.y,
                    transform.position.z > 0 ?
                    transform.position.z + (transform.localScale.z / 2) :
                    transform.position.z - (transform.localScale.z / 2)),
                    new Vector3(transform.localScale.x, 1, Mathf.Abs(delta))
                    );
                transform.localPosition = new Vector3(previousTilePositionX, transform.localPosition.y, middle - (previousTilePositionZ / 2));

            }

        }
    }

    public void ChangeTileBounds(Vector2 previousTileBounds) {
        tileBounds = previousTileBounds;
        if (tileBounds.x > 5.0f) {
            tileBounds.x = 5.0f;
        }
        if (tileBounds.y > 5.0f) {
            tileBounds.y = 5.0f;
        }
        transform.localScale = new Vector3(tileBounds.x, 1.0f, tileBounds.y);
    }

    void CreateRubble(Vector3 pos, Vector3 scl) {

        GameObject rubble = GameObject.CreatePrimitive(PrimitiveType.Cube);
        rubble.name = "Rubble of " + transform.name;
        rubble.transform.localScale = scl;
        rubble.transform.position = pos;
        rubble.AddComponent<Rigidbody>();
        rubble.GetComponent<Rigidbody>().mass = 2;
        rubble.GetComponent<MeshRenderer>().material = gameObject.GetComponent<MeshRenderer>().material;
        stack.ColorMesh(rubble.GetComponent<MeshFilter>().mesh, stack.scoreCount-1);
        Destroy(rubble, 10);
    }

}
