using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class player
{
    public int typeOfPlayer;
    public bool carregouTiro;
    public int lane;

    public player(int typeOfPlayer, bool carregouTiro, int lane)
    {
        this.typeOfPlayer = typeOfPlayer;
        this.carregouTiro = carregouTiro;
        this.lane = lane;
    }

}

public class playerController : MonoBehaviour
{
    public int[] lanesIniciaisPlayer;

    // 0: Noivo 1: Noiva
    [HideInInspector]
    public player[] playerCharacter;

    public KeyCode[] comandosNoivo;
    public KeyCode[] comandosNoiva;

    public GameObject[] bullets;
    public float timeToChargeBullet;

    public float[] PlayerCoolDownBullet;

    float[] cronometroChargeBulletNoivo;
    float[] cronometroChargeBulletNoiva;

    List<float[]> cronometroChargePlayerBullet;

    public List<Vector3> positionsBulletsPerLaneNoivo;
    public List<Vector3> positionsBulletsPerLaneNoiva;

    public sceneGameplayController sceneController;

    public Transform[] AnimPlayerIdle;
    public Transform[] AnimPlayerShooting;
    public Transform[] AnimPlayerRecharging;
    public Transform[] AnimPlayerChargingShoot;

    public AudioClip[] NoivoAudios;
    public AudioClip[] NoivaAudios;

    public AudioSource noivo_audio_source;
    public AudioSource noiva_audio_source;

    bool[] isPlayerShooting;

    void Start()
    {
        playerCharacter = new player[] { new player(0, false, lanesIniciaisPlayer[0]), new player(1, false, lanesIniciaisPlayer[1]) };
        for (int i = 0; i < playerCharacter.Length; i++ )
            enableAnim(i, 0, lanesIniciaisPlayer[i]);

        cronometroChargeBulletNoivo = new float[] { 0, 0, 0, 0 };
        cronometroChargeBulletNoiva = new float[] { 0, 0, 0, 0 };

        isPlayerShooting = new bool[] { false, false };

        cronometroChargePlayerBullet = new List<float[]>();

        cronometroChargePlayerBullet.Add(cronometroChargeBulletNoivo);
        cronometroChargePlayerBullet.Add(cronometroChargeBulletNoiva);
    }

    IEnumerator animShoot(int player, int lane)
    {
        isPlayerShooting[player] = true;
        enableAnim(player, 1, lane);

        yield return new WaitForSeconds(PlayerCoolDownBullet[player]);
        isPlayerShooting[player] = false;

        enableAnim(player, 0, lane);
    }

    bool audioIsPlaying(int indexPlayer, int indiceAudio)
    {
        if (indexPlayer == 0)
        {
            if (noivo_audio_source.clip != NoivoAudios[indiceAudio])
                return false;

            return noivo_audio_source.isPlaying;
        }
        else
        {
            if (noiva_audio_source.clip != NoivaAudios[indiceAudio])
                return false;

            return noiva_audio_source.isPlaying;
        }
    }

    void playAudioPlayer(int indexPlayer, int indiceAudio)
    {
        if (indexPlayer == 0)
        {
            noivo_audio_source.clip = NoivoAudios[indiceAudio];
            noivo_audio_source.Play();
        }
        else
        {
            noiva_audio_source.clip = NoivaAudios[indiceAudio];
            noiva_audio_source.Play();
        }
    }

    // player (0, 1): (noivo, noiva) | tipo de animacao (0, 1, 2, 3): (idle, shoot, reload, charge bullet) | lane (0, 1, 2, 3): (north, south, east, west)
    void enableAnim(int player, int tipoDeAnimacao, int lane)
    {
        switch (tipoDeAnimacao)
        {
            case 0:
                AnimPlayerIdle[player].gameObject.SetActive(true);
                AnimPlayerShooting[player].gameObject.SetActive(false);
                AnimPlayerRecharging[player].gameObject.SetActive(false);
                AnimPlayerChargingShoot[player].gameObject.SetActive(false);

                for (int i = 0; i < AnimPlayerIdle[player].childCount; i++)
                {
                    AnimPlayerIdle[player].GetChild(i).gameObject.SetActive(i == lane);
                }
                break;
            case 1:
                AnimPlayerIdle[player].gameObject.SetActive(false);
                AnimPlayerShooting[player].gameObject.SetActive(true);
                AnimPlayerRecharging[player].gameObject.SetActive(false);
                AnimPlayerChargingShoot[player].gameObject.SetActive(false);

                for (int i = 0; i < AnimPlayerShooting[player].childCount; i++)
                {
                    AnimPlayerShooting[player].GetChild(i).gameObject.SetActive(i == lane);
                }

                break;
            case 2:
                if(!audioIsPlaying(player, 1))
                    playAudioPlayer(player, 1);

                AnimPlayerIdle[player].gameObject.SetActive(false);
                AnimPlayerShooting[player].gameObject.SetActive(false);
                AnimPlayerRecharging[player].gameObject.SetActive(true);
                AnimPlayerChargingShoot[player].gameObject.SetActive(false);

                for (int i = 0; i < AnimPlayerRecharging[player].childCount; i++)
                {
                    AnimPlayerRecharging[player].GetChild(i).gameObject.SetActive(i == lane);
                }

                break;
            case 3:
                AnimPlayerIdle[player].gameObject.SetActive(false);
                AnimPlayerShooting[player].gameObject.SetActive(false);
                AnimPlayerRecharging[player].gameObject.SetActive(false);
                AnimPlayerChargingShoot[player].gameObject.SetActive(true);

                for (int i = 0; i < AnimPlayerChargingShoot[player].childCount; i++)
                {
                    AnimPlayerChargingShoot[player].GetChild(i).gameObject.SetActive(i == lane);
                }
                break;

        }
    }

    void fireBullet(int indexBullet, int indexLane)
    {
        int indexPlayer = 0;

        if (indexBullet == 0 || indexBullet == 2 || indexBullet == 3)
            indexPlayer = 0;

        if (indexBullet == 1 || indexBullet == 2 || indexBullet == 4)
            indexPlayer = 1;

        enableAnim(indexPlayer, 0, indexLane);

        if (sceneController.isPossibleToFireBullet(indexBullet))
        {
            StartCoroutine(animShoot(indexPlayer, indexLane));
            Vector3 posBullet;

            if(indexPlayer == 0)
                posBullet = positionsBulletsPerLaneNoivo[indexLane];
            else
                posBullet = positionsBulletsPerLaneNoiva[indexLane];

            GameObject go = Instantiate(bullets[indexBullet], posBullet, Quaternion.identity) as GameObject;
            go.GetComponent<bulletBehavior>().indexLane = indexLane;
            if (indexLane == 0)
                go.transform.eulerAngles = new Vector3(0, 0, 90);
            if (indexLane == 1)
                go.transform.eulerAngles = new Vector3(0, 0, -90);
            if (indexLane == 2)
                go.transform.eulerAngles = new Vector3(0, 0, 0);
            if (indexLane == 3)
                go.transform.eulerAngles = new Vector3(0, 0, 180);

        }
    }

    // indexPlayer 0 = noivo indexPlayer 1 = noiva
    void controlarTiroPlayer(int indexPlayer)
    {
        // Recarregando arma
        if (sceneController.isPlayerReloading(indexPlayer))
        {
            if (!isPlayerShooting[indexPlayer])
            {
                enableAnim(indexPlayer, 2, playerCharacter[indexPlayer].lane);
            }
            return;
        }

        // tornando comandos generico para o metodo funcionar para qualquer um dos players
        KeyCode[] inputsKey;

        if (indexPlayer == 0)
        {
            inputsKey = comandosNoivo;
        }
        else
        {
            inputsKey = comandosNoiva;
        }

        //AnimPlayerRecharging[indexPlayer].GetChild(playerCharacter[indexPlayer].lane).gameObject.SetActive(false);

        // verificando input
        if (!isPlayerShooting[indexPlayer])
        {
            if (!playerCharacter[indexPlayer].carregouTiro)
                enableAnim(indexPlayer, 0, playerCharacter[indexPlayer].lane);

            for (int i = 0; i < inputsKey.Length; i++)
            {
                if (Input.GetKey(inputsKey[i]))
                {
                    if (playerCharacter[indexPlayer].lane != i)
                    {
                        cronometroChargePlayerBullet[indexPlayer][playerCharacter[indexPlayer].lane] = 0;
                        playerCharacter[indexPlayer].lane = i;
                    }

                    cronometroChargePlayerBullet[indexPlayer][i] += Time.deltaTime;
                    if (cronometroChargePlayerBullet[indexPlayer][i] > timeToChargeBullet)
                    {
                        enableAnim(indexPlayer, 3, playerCharacter[indexPlayer].lane);
                        playerCharacter[indexPlayer].carregouTiro = true;
                    }
                }
            }

            // verificando se soltou input
            if (Input.GetKeyUp(inputsKey[playerCharacter[indexPlayer].lane]))
            {
                if (playerCharacter[indexPlayer].carregouTiro)
                {
                    //int indexOutroPlayer;

                    //if (indexPlayer == 0)
                    //    indexOutroPlayer = 1;
                    //else
                    //    indexOutroPlayer = 0;

                    //if (playerCharacter[indexOutroPlayer].carregouTiro && playerCharacter[indexPlayer].lane == playerCharacter[indexOutroPlayer].lane)
                    //{
                    //    cronometroChargeBulletNoiva[playerCharacter[indexPlayer].lane] = 0;
                    //    playerCharacter[indexOutroPlayer].carregouTiro = false;
                    //    fireBullet(2, playerCharacter[indexPlayer].lane);
                    //}
                    //else
                    //{
                        if (indexPlayer == 0)
                            fireBullet(3, playerCharacter[indexPlayer].lane);
                        else
                            fireBullet(4, playerCharacter[indexPlayer].lane);
                    //}
                }
                else
                {
                    if (indexPlayer == 0)
                        fireBullet(0, playerCharacter[indexPlayer].lane);
                    else
                        fireBullet(1, playerCharacter[indexPlayer].lane);
                }

                cronometroChargePlayerBullet[indexPlayer][playerCharacter[indexPlayer].lane] = 0;
                playerCharacter[indexPlayer].carregouTiro = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        controlarTiroPlayer(0);
        controlarTiroPlayer(1);
    }
}
