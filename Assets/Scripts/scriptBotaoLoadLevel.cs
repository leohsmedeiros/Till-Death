using UnityEngine;
using System.Collections;

public class scriptBotaoLoadLevel : MonoBehaviour {

    public SpriteRenderer[] skinsBotao;
    public string proximaCena;

    void OnMouseEnter()
    {
        skinsBotao[1].enabled = true;
        skinsBotao[0].enabled = false;
    }
	
    void OnMouseExit()
    {
        skinsBotao[0].enabled = true;
        skinsBotao[1].enabled = false;
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Application.LoadLevel(proximaCena);
        }
    }
}
