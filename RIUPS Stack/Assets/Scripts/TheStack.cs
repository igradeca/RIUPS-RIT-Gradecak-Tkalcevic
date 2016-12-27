using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TheStack : MonoBehaviour {

    public GameObject tilePrefab;
    public GameObject currentTile;
    public int scoreCount;
    public int comboCount;
    public bool isGameOver;
    public Color32[] gameColors = new Color32[4];
    public Material stackMat;

    public Text scoreDisplay;

	void Start () {
        scoreCount = 0;
        scoreDisplay.text = "COUNT: " + scoreCount;

        comboCount = 0;
        isGameOver = false;

        InstantiateTile();
	}

	void Update () {

        if (!isGameOver) {
            if (Input.GetKeyDown(KeyCode.Space) && Time.timeScale == 1.0f) {
                PlaceTile();
                if (scoreCount >= 12) {
                    RemoveLowestTile();
                }
                GameObject.Find("Achievements(Clone)").GetComponent<AchievementScript>().GameplayAchievements(scoreCount, comboCount);
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

        ColorMesh(newTile.GetComponent<MeshFilter>().mesh, scoreCount);

        if (currentTile.name != "Tile0") {
            newTile.GetComponent<TileScript>().ChangeTileBounds(currentTile.GetComponent<TileScript>().tileBounds);
        } else {
            ColorMesh(currentTile.GetComponent<MeshFilter>().mesh, scoreCount);
        }

        ModifyTilePosition(newTile);

        if ((scoreCount + 1) % 2 == 0) {
            newTile.GetComponent<TileScript>().movingOnX = true;
        } else {
            newTile.GetComponent<TileScript>().movingOnX = false;
        }

        ChangeCurrentTile(newTile);
        newTile.transform.position = currentTile.transform.position;

        newTile.SetActive(true);
    }
    
    void DropStack () {
        transform.position -= new Vector3(0, 1, 0);
    }

    void PlaceTile() {        
        currentTile.GetComponent<TileScript>().move = false;
        currentTile.GetComponent<TileScript>().CutTile();
        DropStack();
        InstantiateTile();
        scoreCount++;
        scoreDisplay.text = "COUNT: " + scoreCount;
    }

    void RemoveLowestTile () {
        Destroy(GameObject.Find("Tile" + (scoreCount - 12)) );
    }

    private Color32 Lerp4(Color32 a, Color32 b, Color32 c, Color32 d, float t) {
        if (t < 0.33f) {
            return Color.Lerp(a, b, t / 0.33f);
        } else if (t < 0.66f) {
            return Color.Lerp(b, c, (t - 0.33f) / 0.33f);
        } else {
            return Color.Lerp(c, d, (t - 0.66f) / 0.66f);
        }
    }

    public void ColorMesh(Mesh mesh, int scoreCount) {
        Vector3[] verticles = mesh.vertices;
        Color32[] colors = new Color32[verticles.Length];
        float f = Mathf.Sin(scoreCount * 0.10f);

        for (int i = 0; i < verticles.Length; i++) {
            colors[i] = Lerp4(gameColors[0], gameColors[1], gameColors[2], gameColors[3], f);
        }

        mesh.colors32 = colors;
    }

    public void GameOver() {
        --scoreCount;
        isGameOver = true;
    }

}
