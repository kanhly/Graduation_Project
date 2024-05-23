using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoSingleton<AudioManager>
{
    public AudioClip clickClip;
    public AudioClip doorClip;
    public AudioClip enemyDie;
    public AudioClip columnRotate;
    public AudioClip crystalOn;
    public AudioClip conClip;
    public AudioClip playerMove;
    public AudioClip playerHurt;
    public AudioClip failClip;

    public AudioClip bgmClip;

    AudioSource fxSource;
    AudioSource envirFxSource;
    AudioSource playerSource;
    AudioSource bgmSource;

    private void Start()
    {
        fxSource = gameObject.AddComponent<AudioSource>();
        bgmSource = gameObject.AddComponent<AudioSource>();
        envirFxSource = gameObject.AddComponent<AudioSource>();
        playerSource = gameObject.AddComponent<AudioSource>();

        PlayBgmClip();
    }

    public static void PlayBgmClip()
    {
        Instance.bgmSource.clip = Instance.bgmClip;
        Instance.bgmSource.loop = true;
        Instance.bgmSource.volume = 0.1f;
        Instance.bgmSource.Play();
    }

    public void PlayClickAudio()
    {
        fxSource.clip = clickClip;
        fxSource.Play();
    }

    public static void PlayEnemyDie()
    {
        Instance.fxSource.clip = Instance.enemyDie;
        Instance.fxSource.Play();
    }

    public void PlayColumnRotate()
    {
        envirFxSource.clip = columnRotate;
        envirFxSource.Play();
    }

    public void PlayCrystalOn()
    {
        fxSource.clip = crystalOn;
        fxSource.Play();
    }

    public void PlayConClip()
    {
        fxSource.clip = conClip;
        fxSource.Play();
    }

    public static void PlayDoorClip()
    {
        Instance.envirFxSource.clip = Instance.doorClip;
        Instance.envirFxSource.Play();
    }

    public static void PlayPlayerMove()
    {
        Instance.playerSource.clip = Instance.playerMove;
        Instance.playerSource.volume = 0.2f;
        Instance.playerSource.Play();
    }

    public void PlayPlayerHurt()
    {
        playerSource.clip = playerHurt;
        playerSource.Play();
    }

    public void PlayFailClip()
    {
        envirFxSource.clip = failClip;
        envirFxSource.Play();
    }
}
