using System;
using UnityEngine;
using System.Linq;

namespace FG
{
    public class AudioManager : MonoBehaviour
    {
        public Sound[] sounds;

        private float master = 1f;
        public float Master
        {
            get
            {
                return master;
            }
        }
        private float music = 1f;
        public float Music
        {
            get
            {
                return music;
            }
        }
        private float effects = 1f;
        public float Effects
        {
            get
            {
                return effects;
            }
        }

        private static AudioManager instance;
        public static AudioManager Instance
        {
            get
            {
                return instance;
            }
        }

        public enum SoundType
        {
            Master,
            Music,
            Effect
        }

        [Serializable]
        public class Sound
        {
            public string name;
            public AudioClip clip;
            public SoundType type = SoundType.Master;

            [Range(0f, 1f)] public float volume;
            [Range(.1f, 3f)] public float pitch = 1f;
            public bool loop;

            [HideInInspector] public AudioSource source;
            [HideInInspector] public int accumulation = 0;
        }

        public void Play(string inputName)
        {
            Sound s = Array.Find(sounds, sound => sound.name == inputName);
            if (s == null)
            {
                Debug.LogError($"You made a typo stupid : {inputName}");
                return;
            }
            s.source.Play();
        }

        public void PlayAccumulated(string inputName)
        {
            Sound s = Array.Find(sounds, sound => sound.name == inputName);
            if (s == null)
            {
                Debug.LogError($"You made a typo stupid : {inputName}");
                return;
            }
            s.accumulation++;
            s.source.Play();
        }

        public void Stop(string inputName)
        {
            Sound s = Array.Find(sounds, sound => sound.name == inputName);
            if (s == null)
            {
                Debug.LogError($"You made a typo stupid : {inputName}");
                return;
            }
            s.source.Stop();
            s.accumulation = 0;
        }

        public void StopAccumulated(string inputName)
        {
            Sound s = Array.Find(sounds, sound => sound.name == inputName);
            if (s == null)
            {
                Debug.LogError($"You made a typo stupid : {inputName}");
                return;
            }
            if(--s.accumulation == 0)
                s.source.Stop();
        }

        public void TryStop(string inputName, string tryName)
        {
            Sound s = Array.Find(sounds, sound => sound.name == inputName);
            if (s == null)
            {
                Debug.LogError($"You made a typo stupid : {inputName}");
                return;
            }

            Sound t = Array.Find(sounds, sound => sound.name == tryName);
            if (t == null)
            {
                Debug.LogError($"You made a typo stupid : {tryName}");
                return;
            }

            if(!t.source.isPlaying)
                s.source.Play();
        }

        public void MasterVolume(float multiplier)
        {
            master = multiplier;

            UpdateVolume();
        }

        public void MusicVolume(float multiplier)
        {
            music = multiplier;

            UpdateVolume();
        }

        public void EffectVolume(float multiplier)
        {
            effects = multiplier;

            UpdateVolume();
        }

        private void UpdateVolume()
        {
            foreach (Sound s in sounds.Where(each => each.type == SoundType.Music))
                s.source.volume = s.volume * master * music;

            foreach (Sound s in sounds.Where(each => each.type == SoundType.Effect))
                s.source.volume = s.volume * master * effects;

            foreach (Sound s in sounds.Where(each => each.type == SoundType.Master))
                s.source.volume = s.volume * master;
        }


        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
            {
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(gameObject);

            foreach (Sound s in sounds)
            {
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.clip = s.clip;

                s.source.volume = s.volume;
                s.source.pitch = s.pitch;
                s.source.loop = s.loop;
            }
        }

        private void Start()
        {
            Play("BackgroundMusic");
        }

        private void OnLevelWasLoaded(int level)
        {
            if(instance == this)
                foreach (Sound s in sounds)
                    if (s.name != "BackgroundMusic")
                        s.source.Stop();
        }
    }
}