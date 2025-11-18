using System;
using UnityEngine;

namespace _Scripts
{
    public class CatVisual : MonoBehaviour
    {
        [SerializeField] private Sprite m_silenceSprite;
        [SerializeField] private Sprite m_soundSprite;
        private SpriteRenderer m_renderer;
        [SerializeField] private float m_flashDuration = 0.1f;
        private float m_flashTimer = 0.0f;

        private void Awake()
        {
            m_renderer = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            if (!(m_flashTimer > 0.0f)) return;
            m_flashTimer -= Time.deltaTime;
            if (m_flashTimer <= 0.0f)
            {
                m_renderer.sprite = m_silenceSprite;
            }
        }

        public void TriggerVisual()
        {
            m_renderer.sprite = m_soundSprite;
            m_flashTimer = m_flashDuration;
        }
    }
}
