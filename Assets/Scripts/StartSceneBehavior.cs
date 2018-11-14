using UnityEngine;
using System.Collections;

public class StartSceneBehavior : MonoBehaviour {


    public SpriteRenderer logo;
    int step = 0;
    public float timeToWait;
    float cronometro = 0;


	// Update is called once per frame
	void Update () {

        Color corLogo = logo.color;
        switch (step)
        {
            case 0:
                corLogo.a += 0.005f;
                if (corLogo.a >= 1)
                    step = 1;
                else
                    logo.color = corLogo;

                break;
            case 1:
                cronometro += Time.deltaTime;
                if (cronometro > timeToWait)
                {
                    cronometro = 0;
                    step = 2;
                }

                break;
            case 2:
                corLogo.a -= 0.005f;
                if (corLogo.a <= 0)
                    step = 3;
                else
                    logo.color = corLogo;

                break;
            case 3:
                Application.LoadLevel("cena Menu");
                break;
        }
	
	}
}
