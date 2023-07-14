using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

namespace SoundSystem
{

    public class SoundController : MonoBehaviour
    {
        public static SoundController instance;

        public List<AudioClip> menuSeAudioClipList = new List<AudioClip>();
        private AudioSource menuSeAudioSource;

        public List<AudioClip> bgmAudioClipList = new List<AudioClip>();
        private AudioSource bgmAudioSource;

        public List<AudioClip> engineSeAudioClipList = new List<AudioClip>();
        private AudioSource engineSeAudioSource;

        public List<AudioClip> jetSeAudioClipList = new List<AudioClip>();
        private AudioSource jetSeAudioSource;

        private List<IEnumerator> fadeCoroutines = new List<IEnumerator>();

        [SerializeField, HeaderAttribute("Audio Mixer")]
        public AudioMixer audioMixer;
        public AudioMixerGroup menuSeAMG, bgmAMG, engineSeAMG, jetSeAMG;

        public AudioMixer effectAudioMixer;


        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Destroy(this.gameObject);
                return;
            }

            menuSeAudioSource = InitializeAudioSource(this.gameObject, false, menuSeAMG);
            bgmAudioSource = InitializeAudioSource(this.gameObject, true, bgmAMG);
            engineSeAudioSource = InitializeAudioSource(this.gameObject, true, engineSeAMG);
            jetSeAudioSource = InitializeAudioSource(this.gameObject, false, jetSeAMG);
        }

        private AudioSource InitializeAudioSource(GameObject parentGameObject, bool isLoop = false, AudioMixerGroup amg = null)
        {
            //このゲームオブジェクトにaudioSourceコンポーネントを追加し、audioSourceの箱を作り、操作できるようになった//
            AudioSource audioSource = parentGameObject.AddComponent<AudioSource>();

            audioSource.loop = isLoop;
            audioSource.playOnAwake = false;

            if (amg != null)
            {
                audioSource.outputAudioMixerGroup = amg;
            }

            //各設定が施されたaudioSourceを返す
            return audioSource;
        }

        public void PlayMenuSe(string clipName)
        {
            AudioClip audioClip = menuSeAudioClipList.FirstOrDefault(clip => clip.name == clipName);

            if (audioClip == null)
            {
                Debug.Log(clipName + "は見つかりません");
                return;
            }
            menuSeAudioSource.Play(audioClip);
        }

        public void PlayBGM(string clipName)
        {
            AudioClip audioClip = bgmAudioClipList.FirstOrDefault(clip => clip.name == clipName);

            if (audioClip == null)
            {
                Debug.Log(clipName + "は見つかりません");
                return;
            }
            bgmAudioSource.Play(audioClip);
        }

        public void PlayEngineSe(string clipName)
        {
            AudioClip audioClip = engineSeAudioClipList.FirstOrDefault(clip => clip.name == clipName);

            if (audioClip == null)
            {
                Debug.Log(clipName + "は見つかりません");
                return;
            }
            engineSeAudioSource.Play(audioClip);
        }

        public void PlayJetSe(string clipName)
        {
            AudioClip audioClip = jetSeAudioClipList.FirstOrDefault(clip => clip.name == clipName);

            if (audioClip == null)
            {
                Debug.Log(clipName + "は見つかりません");
                return;
            }
            jetSeAudioSource.Play(audioClip);
        }

        public void StopEngine()
        {
            engineSeAudioSource.Stop();
        }

    }
}