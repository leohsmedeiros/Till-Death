using UnityEngine;
using System.Collections;

public class pathBehavior : MonoBehaviour
{

    public enum lanePath { North, South, East, West };
    public lanePath lane;

    public GameObject bossPrefab;
    public GameObject[] enemysPrefabs;
    
    public Vector2 startEnemyPosition;
    public float[] RangetimeToRespawn;
    float timeToRespawnPerLane;
    float timePerLane;


    int convertLaneToIndex()
    {
        switch (lane)
        {
            case lanePath.North:
                return 0;
            case lanePath.South:
                return 1;
            case lanePath.East:
                return 2;
            case lanePath.West:
                return 3;
        }

        return 0;
    }

    // Use this for initialization
    void Start()
    {
        timeToRespawnPerLane = Random.Range(RangetimeToRespawn[0], RangetimeToRespawn[1]);
        timePerLane = timeToRespawnPerLane - 1;

    }

    bool BossEstaEmCena()
    {
        GameObject boss = GameObject.FindGameObjectWithTag("Monstro_Boss");

        if (boss == null)
            return false;

        return true;
    }

    // Update is called once per frame
    void Update()
    {
        timePerLane += Time.deltaTime;

        if (timePerLane > timeToRespawnPerLane)
        {
            if (enemysPrefabs.Length > 0)
            {
                GameObject go;

                int chanceDeNascerBoss = Random.Range(0, 11);

                if(bossPrefab == null || chanceDeNascerBoss < 10 || BossEstaEmCena())
                    go = Instantiate(enemysPrefabs[Random.Range(0, enemysPrefabs.Length)], startEnemyPosition, Quaternion.identity) as GameObject;
                else
                    go = Instantiate(bossPrefab, startEnemyPosition, Quaternion.identity) as GameObject;

                go.GetComponent<enemyBehavior>().indexLane = convertLaneToIndex();
            }
            timePerLane = 0;
            timeToRespawnPerLane = Random.Range(RangetimeToRespawn[0], RangetimeToRespawn[1]);
        }
    }
}
