using UnityEngine;
using UnityEngine.UI;

namespace Borodar.FarlandSkies.CloudyCrownPro
{
    public class TimeEditorButtons : MonoBehaviour
    {
        [SerializeField] private GameObject _parent;
        public void Pause()
        {
            var sceneManager = SkyboxCycleManager.Instance;
            sceneManager.Paused = !sceneManager.Paused;
        }
        public void Close()
        {
            _parent.SetActive(false);
            //gameObject.GetComponentInParent<GameObject>().SetActive(false);
           // Debug.Log(gameObject.GetComponentInParent<GameObject>().activeSelf);
        }
    }
}