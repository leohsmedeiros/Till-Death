using UnityEngine;
using System.Collections;

public class GameoverSceneController : MonoBehaviour {

    public Font fonte;
    public Rect textGameOver;
    public Rect textScore;
    public Rect textPressToContinue;

    public GameObject[] telasGameover;

	// Use this for initialization
	void Start () {
        int valor = Random.Range(0, 11);
        Debug.Log(valor);
        valor = valor % 2;
        telasGameover[valor].SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Application.LoadLevel("cena Menu");
        }
	}


    void OnGUI()
    {
        GUIStyle style2 = new GUIStyle();
        style2.font = fonte;
        style2.normal.textColor = new Color(244, 138, 80, 255);
        style2.alignment = TextAnchor.MiddleLeft;

        style2.fontSize = 60;
        GUI.Label(textGameOver, "Game over", style2);

        style2.fontSize = 30;
        GUI.Label(textScore, "Score: " + sceneGameplayController.score.ToString() , style2);

        style2.fontSize = 30;
        GUI.Label(textPressToContinue, "Press space to start again", style2);
    }
}
