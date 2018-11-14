using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class sceneGameplayController : MonoBehaviour
{
    public Transform municaoNoivo;
    public Transform municaoNoiva;

    public float[] timeToReloadGunPlayer;

    int[] MaxNumOfShotsPlayer;
    int[] CurNumOfShotsPlayer;

    float[] cronometroReloadShotsPlayer;

    bool[] playerReloadingGun;

    public GameObject ataqueFinal;
    public Font fonte;

    public Rect textScore;
    public Rect textValueScore;

    public static int score;

    static public void Pontuar(int pontos)
    {
        score += pontos;
    }

    // Use this for initialization
    void Start()
    {
        MaxNumOfShotsPlayer = new int[] { municaoNoivo.childCount, municaoNoiva.childCount };

        CurNumOfShotsPlayer = new int[] { MaxNumOfShotsPlayer[0], MaxNumOfShotsPlayer[1] };

        playerReloadingGun = new bool[] { false, false };
        cronometroReloadShotsPlayer = new float[] { 0, 0 };

        score = 0;
    }

    public bool isPossibleToFireBullet(int indexBullet)
    {
        // Tiro comum noivo
        if (indexBullet == 0)
        {
            if (CurNumOfShotsPlayer[0] - 1 >= 0)
            {
                CurNumOfShotsPlayer[0] -= 1;
                return true;
            }
        }
        // Tiro comum noiva
        if (indexBullet == 1)
        {
            if (CurNumOfShotsPlayer[1] - 1 >= 0)
            {
                CurNumOfShotsPlayer[1] -= 1;
                return true;
            }
        }
        // Tiro Combinado
        if (indexBullet == 2)
        {
            if (CurNumOfShotsPlayer[0] - 2 >= 0 && CurNumOfShotsPlayer[1] - 2 >= 0)
            {
                CurNumOfShotsPlayer[0] -= 2;
                CurNumOfShotsPlayer[1] -= 2;
                return true;
            }
        }
        // Tiro carregado Noivo
        if (indexBullet == 3)
        {
            if (CurNumOfShotsPlayer[0] - 3 >= 0)
            {
                CurNumOfShotsPlayer[0] -= 3;
                return true;
            }
        }
        // Tiro carregado Noiva
        if (indexBullet == 4)
        {
            if (CurNumOfShotsPlayer[1] - 3 >= 0)
            {
                CurNumOfShotsPlayer[1] -= 3;
                return true;
            }
        }

        return false;
    }

    void UpdatePainelMunicao(Transform painelMunicao, int curNumOfShots)
    {
        for (int i = 0; i < painelMunicao.childCount; i++)
        {
            if (i < curNumOfShots)
            {
                painelMunicao.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                painelMunicao.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    void Reload(int player)
    {
        if (CurNumOfShotsPlayer[player] == 0)
        {
            playerReloadingGun[player] = true;
            cronometroReloadShotsPlayer[player] += Time.deltaTime;

            if (cronometroReloadShotsPlayer[player] > timeToReloadGunPlayer[player])
            {
                playerReloadingGun[player] = false;
                CurNumOfShotsPlayer[player] = MaxNumOfShotsPlayer[player];
                cronometroReloadShotsPlayer[player] = 0;
            }
        }
    }

    public bool isPlayerReloading(int player)
    {
        return playerReloadingGun[player];
    }

    IEnumerator endGame()
    {
        ataqueFinal.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        Application.LoadLevel("cena Gameover");
    }

    public void finalizarGame()
    {
        StartCoroutine(endGame());
    }

    float cronScorePerSecond = 0;
    // Update is called once per frame
    void Update()
    {
        cronScorePerSecond += Time.deltaTime;
        if (cronScorePerSecond > 1)
        {
            score++;
            cronScorePerSecond = 0;
        }

        // o 2 no for eh pq sao 2 players (o noivo e a noiva)
        for(int i=0; i<2; i++)
        {
            if(i == 0)
                UpdatePainelMunicao(municaoNoivo, CurNumOfShotsPlayer[0]);
            else
                UpdatePainelMunicao(municaoNoiva, CurNumOfShotsPlayer[1]);

            Reload(i);

        }

    }


    void OnGUI()
    {
        GUIStyle style1 = new GUIStyle();
        style1.font = fonte;
        style1.normal.textColor = Color.white;
        style1.fontSize = 20;

        GUIStyle style2 = new GUIStyle();
        style2.font = fonte;
        style2.normal.textColor = Color.white;
        style2.alignment = TextAnchor.MiddleLeft;
        style2.fontSize = 50;

        GUI.Label(textScore, "SCORE: ", style1);
        GUI.Label(textValueScore, score.ToString(), style2);
    }
}
