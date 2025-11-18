using TMPro;
using UnityEngine;

namespace _Scripts
{
    public class CountDownUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_countdownText;

        public void SetCountdownText(string text)
        {
            m_countdownText.text = text;
        }

        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }
    }
}
