using System.Collections;
using UnityEngine;

namespace Utils
{
    public class AlphaBlinking : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer target;
        [SerializeField] private float duration;
        [SerializeField] private bool blinkOnStart;

        private void Start()
        {
            if (blinkOnStart)
                StartBlinking();
        }

        public void StartBlinking()
        {
            StartCoroutine(BlinkingCoroutine());
        }

        public void StopBlinking()
        {
            StopAllCoroutines();
            target.enabled = true;
        }

        private IEnumerator BlinkingCoroutine()
        {
            while (true)
            {
                yield return new WaitForSecondsRealtime(duration);

                target.enabled = !target.enabled;
            }
        }
    }
}