using UnityEngine;
using System.Collections;

public class MenuSceneBehavior : MonoBehaviour
{

    public SpriteRenderer Titulo;
    public GameObject[] Botoes;
    public SpriteRenderer noivos;
    public SpriteRenderer fundo;


    int step = 0;

    bool ativarAnimBG = false;

    int stepAnimBG = 0;

    float timeWaitAnim = 2;
    float cronAnimWaiting = 0;

    // Update is called once per frame
    void Update()
    {
        Color corNova;

        if (ativarAnimBG)
        {
            switch (stepAnimBG)
            {
                case 0:
                    if (fundo.color.a < 1)
                    {
                        corNova = fundo.color;
                        corNova.a += 0.01f;
                        fundo.color = corNova;
                    }
                    else
                    {
                        stepAnimBG = 1;
                    }

                    break;
                case 1:
                    cronAnimWaiting += Time.deltaTime;
                    if (cronAnimWaiting > timeWaitAnim)
                    {
                        cronAnimWaiting = 0;
                        stepAnimBG = 2;
                    }
                    break;

                case 2:
                    if (fundo.color.a > 0)
                    {
                        corNova = fundo.color;
                        corNova.a -= 0.01f;
                        fundo.color = corNova;
                    }
                    else
                    {
                        stepAnimBG = 0;
                    }
                    break;

            }
        }


        switch (step)
        {
            case 0:
                if (noivos.color.a < 1)
                {
                    corNova = noivos.color;
                    corNova.a += 0.1f;
                    noivos.color = corNova;
                }
                else
                {
                    step = 1;
                    ativarAnimBG = true;
                }

                break;
            case 1:
                if (Titulo.color.a < 1)
                {
                    corNova = Titulo.color;
                    corNova.a += 0.01f;
                    Titulo.color = corNova;
                }
                else
                {
                    step = 2;
                }

                break;
            case 2:
                for (int i = 0; i < Botoes.Length; i++ )
                {
                    Botoes[i].SetActive(true);
                    step = 3;
                }

                break;
            case 3:
                break;
        }

    }
}
