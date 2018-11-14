using UnityEngine;
using System.Collections;

public class bulletBehavior : MonoBehaviour {

    [HideInInspector]
    public int indexLane;
    public float velocidade = 100;
    public int dano;
    public AudioClip audio;

    void Start()
    {
        GameObject go = Instantiate(new GameObject("tiroSound")) as GameObject;
        go.AddComponent<AudioSource>().clip = audio;
        go.GetComponent<AudioSource>().Play();
        Destroy(go, 2);
    }

    void Update()
    {

        Vector3 newPos = this.transform.position;
        switch (indexLane)
        {
            case 0:
                newPos.y += velocidade * Time.deltaTime;
                break;

            case 1:
                newPos.y -= velocidade * Time.deltaTime;
                break;

            case 2:
                newPos.x += velocidade * Time.deltaTime;
                break;

            case 3:
                newPos.x -= velocidade * Time.deltaTime;
                break;
        }

        this.transform.position = newPos;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag.Contains("Monstro"))
        {
            other.GetComponent<enemyBehavior>().hurtEnemy(dano);
        }

        Destroy(this.gameObject);
    }

}
