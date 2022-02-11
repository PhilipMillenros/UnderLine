using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FG
{
    public class VolumeSlider : MonoBehaviour
    {
        [SerializeField] private Slider master;
        [SerializeField] private Slider music;
        [SerializeField] private Slider effect;

        public void MasterVolume(float val)
        {
            AudioManager.Instance.MasterVolume(val);
        }

        public void MusicVolume(float val)
        {
            AudioManager.Instance.MusicVolume(val);
        }

        public void EffectVolume(float val)
        {
            AudioManager.Instance.EffectVolume(val);
        }

        private void Start()
        {
            if (master != null)
                master.value = AudioManager.Instance.Master;
            if (music != null)
                music.value = AudioManager.Instance.Music;
            if (effect != null)
                effect.value = AudioManager.Instance.Effects;
        }
    }
}