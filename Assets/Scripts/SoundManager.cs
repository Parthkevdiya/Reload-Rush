using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("Audio Source")]
    [SerializeField] private AudioSource getBulletAudioSource;
    [SerializeField] private AudioSource clickSoundAudioSource;
    [SerializeField] private AudioSource reloadAudioSource;
    [SerializeField] private AudioSource shotAudioSource;
    [SerializeField] private AudioSource happySoundAudioSource;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip getBulletAudioClip;
    [SerializeField] private AudioClip clickSoundAudioClip;
    [SerializeField] private AudioClip reloadSoundAudioClip;
    [SerializeField] private AudioClip shotSoundAudioClip;
    [SerializeField] private AudioClip happySoundAudioClip;

    private void Awake()
    {
        Instance = this;
    }

    public void PlayGetBulletSound()
    {
        /*AudioSource audioSource = Instantiate(audioSourcePrefeb , Vector3.forward , Quaternion.identity , cameraTransform);
        audioSource.clip = getBulletAudioClip;
        audioSource.Play();
        Destroy(audioSource.gameObject , getBulletAudioClip.length);*/

        if (ReloadRushGameManager.Instance.GetVolumeState())
        {
            getBulletAudioSource.clip = getBulletAudioClip;
            getBulletAudioSource.Play();
        }
    }

    public void PlayClickSound()
    {
        if (ReloadRushGameManager.Instance.GetVolumeState())
        {
            clickSoundAudioSource.clip = clickSoundAudioClip;
            clickSoundAudioSource.Play();
        }
    }

    public void PlayReloadSound()
    {
        if (ReloadRushGameManager.Instance.GetVolumeState())
        {
            reloadAudioSource.clip = reloadSoundAudioClip;
            reloadAudioSource.Play();
        }
    }

    public void PlayShotSound()
    {
        if (ReloadRushGameManager.Instance.GetVolumeState())
        {
            shotAudioSource.clip = shotSoundAudioClip;
            shotAudioSource.Play();
        }
    }

    public void PlayHappySound()
    {
        if (ReloadRushGameManager.Instance.GetVolumeState())
        {
            happySoundAudioSource.clip = happySoundAudioClip;
            happySoundAudioSource.Play();
        }
    }


}
