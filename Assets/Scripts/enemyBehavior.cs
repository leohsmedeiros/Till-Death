using UnityEngine;
using System.Collections;

public class enemyBehavior : MonoBehaviour
{

    public enum typeOfAnimationMoveEnemy { normal, boss_moving, dig_moving };

    public typeOfAnimationMoveEnemy animEnemy;

    [HideInInspector]
    public int indexLane;
    public float velocidade;
    public int HP;
    public int pontosOferecidos;

    // 3 variaveis q so serao usadas caso a animacao nao seja a normal
    float[] timeAnim;
    float[] cronToWaitAnim;
    int stepAnim = 0;

    bool willDie = false;

    // Use this for initialization
    void Start()
    {

        if (animEnemy == typeOfAnimationMoveEnemy.boss_moving)
        {
            timeAnim = new float[] { 1.9f, 1.25f };
            cronToWaitAnim = new float[] { 0f, 0f };
        }
        if (animEnemy == typeOfAnimationMoveEnemy.dig_moving)
        {
            timeAnim = new float[] { 1f, 1f, 0.8f, 1f };
            cronToWaitAnim = new float[] { 0f, 0f, 0f, 0f };
        }
    }

    IEnumerator piscarInimigoFerido()
    {
        foreach (SpriteRenderer sr in this.GetComponentsInChildren<SpriteRenderer>())
        {
            sr.color = Color.black;
        }
        yield return new WaitForSeconds(0.1f);
        foreach (SpriteRenderer sr in this.GetComponentsInChildren<SpriteRenderer>())
        {
            sr.color = Color.white;
        }
        yield return new WaitForSeconds(0.1f);
        foreach (SpriteRenderer sr in this.GetComponentsInChildren<SpriteRenderer>())
        {
            sr.color = Color.black;
        }
        yield return new WaitForSeconds(0.1f);
        foreach (SpriteRenderer sr in this.GetComponentsInChildren<SpriteRenderer>())
        {
            sr.color = Color.white;
        }
        yield return new WaitForSeconds(0.1f);
    }

    public void hurtEnemy(int damage)
    {
        if (HP - damage <= 0)
        {
            this.GetComponent<BoxCollider2D>().enabled = false;
            willDie = true;
            Destroy(this.gameObject, 0.6f);
            this.transform.GetChild(0).gameObject.SetActive(false);
            this.transform.GetChild(1).gameObject.SetActive(true);
            
            sceneGameplayController.Pontuar(pontosOferecidos);
        }
        else
        {
            StartCoroutine(piscarInimigoFerido());
            HP -= damage;
        }
    }

    Vector3 UpdatePosition(float speed)
    {
        Vector3 newPos = this.transform.position;
        switch (indexLane)
        {
            case 0:
                newPos.y -= speed;
                break;

            case 1:
                newPos.y += speed;
                break;

            case 2:
                newPos.x -= speed;
                break;

            case 3:
                newPos.x += speed;
                break;
        }

        return newPos;
    }


    int executeAnim()
    {
        for (int i = 0; i < this.transform.GetChild(0).childCount; i++)
        {
            this.transform.GetChild(0).GetChild(i).gameObject.SetActive(stepAnim == i);
        }

        cronToWaitAnim[stepAnim] += Time.deltaTime;
        if (cronToWaitAnim[stepAnim] > timeAnim[stepAnim])
        {
            cronToWaitAnim[stepAnim] = 0;

            stepAnim++;
            if (stepAnim == this.transform.GetChild(0).childCount)
                stepAnim = 0;
        }

        return stepAnim;
    }

    // Update is called once per frame
    void Update()
    {

        if (willDie)
            return;

        switch(animEnemy)
        {
            case typeOfAnimationMoveEnemy.normal:
                this.transform.position = UpdatePosition(velocidade * Time.deltaTime);
            
                break;

            case typeOfAnimationMoveEnemy.boss_moving:

                if (executeAnim() == 0)
                    this.transform.position = UpdatePosition(velocidade * Time.deltaTime);

                break;

            case typeOfAnimationMoveEnemy.dig_moving:
                int step = executeAnim();
                if (step == 1)
                {
                    this.GetComponent<BoxCollider2D>().enabled = true;
                }
                else
                {
                    this.GetComponent<BoxCollider2D>().enabled = false;
                }

                if (step == 3)
                    this.transform.position = UpdatePosition(velocidade * Time.deltaTime);

                break;
        }

        
        /*
        if (!this.gameObject.tag.Contains("Boss"))
        {
        }
        else
        {
            cronometroToWaitBoss += Time.deltaTime;
            if (cronometroToWaitBoss > timeToWaitBoss)
            {
                Debug.Log("Parou");
                cronometroToWaitBoss = 0;
            }
            else
            {
                Debug.Log("Andou");
                this.transform.position = UpdatePosition(velocidade * Time.deltaTime);
            }
        }
        */
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "baseNoivos")
        {

            //Vector2.Distance(this.transform.position, other.
            GameObject.FindGameObjectWithTag("SceneGameplayController").GetComponent<sceneGameplayController>().finalizarGame();
        }
    }
}
