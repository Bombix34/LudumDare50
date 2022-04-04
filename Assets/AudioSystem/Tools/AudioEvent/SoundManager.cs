using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools.Audio;
using DG.Tweening;

namespace Tools.Managers
{
	public class SoundManager : MonoBehaviour
	{
        public static SoundManager instance;

        private SoundManagerSettings settings;

        private AudioSource audioSourceDefault;
        private AudioSource audioSourceMusic;
        private AudioSource audioSourceUI;
        private AudioSource audioSourceEffects;

        public void Awake()
		{
            if (instance != null && instance != this)
                Destroy(gameObject);
            instance = this;

			settings = (SoundManagerSettings)Resources.Load("SoundManagerSettings", typeof(SoundManagerSettings));

            GameObject audioSourceDefault_GO = new GameObject("SM_AudioSource_Default");
            audioSourceDefault_GO.transform.parent = transform;
            audioSourceDefault = audioSourceDefault_GO.AddComponent<AudioSource>();
            audioSourceDefault.playOnAwake = false;
            audioSourceDefault.spatialBlend = 0;

            GameObject audioSourceMusic_GO = new GameObject("SM_AudioSource_Music");
            audioSourceMusic_GO.transform.parent = transform;
            audioSourceMusic = audioSourceMusic_GO.AddComponent<AudioSource>();
            audioSourceMusic.playOnAwake = false;
            audioSourceMusic.spatialBlend = 0;

            GameObject audioSourceUI_GO = new GameObject("SM_AudioSource_UI");
            audioSourceUI_GO.transform.parent = transform;
            audioSourceUI = audioSourceUI_GO.AddComponent<AudioSource>();
            audioSourceUI.playOnAwake = false;
            audioSourceUI.spatialBlend = 0;

            GameObject audioSourceEffects_GO = new GameObject("SM_AudioSource_Effects");
            audioSourceEffects_GO.transform.parent = transform;
            audioSourceEffects = audioSourceEffects_GO.AddComponent<AudioSource>();
            audioSourceEffects.playOnAwake = false;
            audioSourceEffects.spatialBlend = 0;
        }

        public void Start()
        {
            audioSourceMusic.clip = settings.music;
            audioSourceMusic.loop = true;
            audioSourceMusic.Play();
        }


        public void FadeMusicForJingle()
        {
            StartCoroutine(FadeForJingleCoroutine());
        }

        private IEnumerator FadeForJingleCoroutine()
        {
            audioSourceMusic.DOFade(0.4f, 0.2f);
            yield return new WaitForSeconds(2f);
            audioSourceMusic.DOFade(1f, 1f);

        }

        public void FinalFade()
        {
            audioSourceMusic.Stop();
        }

        public void PlaySound(AudioFieldEnum sound)
        {
            if (settings.SoundDatabase.GetAudioEvent(sound) == null)
                return;
            AudioSourceType sourceType = settings.SoundDatabase.GetAudioEvent(sound).audioSourceType;
            settings.SoundDatabase.PlaySound(sound, GetAudioSource(sourceType));
        }

        public AudioSource GetAudioSource(AudioSourceType audioSourceType)
        {
            switch (audioSourceType)
            {
                case AudioSourceType.Music:
                    return audioSourceMusic;
                case AudioSourceType.UI:
                    return audioSourceUI;
                case AudioSourceType.Effects:
                    return audioSourceEffects;
                default:
                    return audioSourceDefault;
            }
        }

        public void SetVolume(AudioSourceType audioSourceType, float volume)
        {
            GetAudioSource(audioSourceType).volume = volume;
        }

        public void FadeInVolume(AudioSourceType audioSourceType, float time)
        {
            AudioSource source = GetAudioSource(audioSourceType);
            DOTween.To(() => source.volume, x => source.volume = x, 1, time);
        }

        public void FadeOutVolume(AudioSourceType audioSourceType, float time)
        {
            AudioSource source = GetAudioSource(audioSourceType);
            DOTween.To(() => source.volume, x => source.volume = x, 0, time);
        }

        public void StopAudioSource(AudioSourceType audioSourceType)
        {
            GetAudioSource(audioSourceType).Stop();
        }


        public enum AudioSourceType
        {
            Default = 0,
            Music = 1,
            UI = 2,
            Effects = 3
        }
    }
}

