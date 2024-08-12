using System;
using Tycoon.PlayerSystems;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Borodar.FarlandSkies.CloudyCrownPro
{
    public class TimeText : MonoBehaviour
    {
        private Text _text;
        protected void Awake()
        {
            _text = GetComponent<Text>();
        }

        protected void Update()
        {
            var timeSpan = TimeSpan.FromHours(SkyboxCycleManager.Instance.CycleProgress * 0.24f);
            _text.text = string.Format("{0:D2}:{1:D2}", timeSpan.Hours, timeSpan.Minutes);
        }
    }
}