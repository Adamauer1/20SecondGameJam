using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Scripts
{

    [System.Serializable]
    public class RhythmEvent
    {
        public float BeatCount;
        public string BeatType;
    }
    
    public class MinigameManager : MonoBehaviour
    {
        [SerializeField] private AudioSource m_rhythmSource;
        [SerializeField] private AudioClip m_rhythmClip;
        [SerializeField] private BeatClock m_beatClock;


        [SerializeField] private float m_perfectWindow = 0.05f;
        [SerializeField] private float m_goodWindow = 0.15f;
        [SerializeField] private float m_badWindow = 0.3f;       
        
        [SerializeField] private SpriteRenderer m_rhythmRenderer;
        private Color m_baseColor = Color.white;
        private Color m_cueFlashColor = Color.gray;

        private int m_eventIndex = 0;
        private int m_nextPlayerEvent = 0;
        private float m_flashDuration = 0.1f;
        private float m_flashTimer = 0.0f;


        private List<RhythmEvent> m_rhythmEvents = new List<RhythmEvent>();
        
        private List<RhythmEvent> m_playerEvents = new List<RhythmEvent>();

        private void Start()
        {
            m_rhythmEvents.Add(new RhythmEvent{ BeatCount = 0f, BeatType = "Cue"} );
            m_rhythmEvents.Add(new RhythmEvent{ BeatCount = 1f, BeatType = "Cue"} );
            m_rhythmEvents.Add(new RhythmEvent{ BeatCount = 2f, BeatType = "Cue"} );
            m_rhythmEvents.Add(new RhythmEvent{ BeatCount = 3f, BeatType = "Press"} );
            m_rhythmEvents.Add(new RhythmEvent{ BeatCount = 4f, BeatType = "Cue"} );
            m_rhythmEvents.Add(new RhythmEvent{ BeatCount = 5f, BeatType = "Cue"} );
            m_rhythmEvents.Add(new RhythmEvent{ BeatCount = 6f, BeatType = "Press"} );
            m_rhythmEvents.Add(new RhythmEvent{ BeatCount = 7f, BeatType = "Press"} );

            GameInput.Instance.OnButtonClicked += GameInput_OnButtonClicked;

            foreach (RhythmEvent rhythmEvent in m_rhythmEvents)
            {
                if (rhythmEvent.BeatType == "Press")
                {
                    m_playerEvents.Add(rhythmEvent);
                }
            }
        }

        private void Update()
        {
            if (m_flashTimer > 0f)
            {
                m_flashTimer -= Time.deltaTime;
                if (m_flashTimer <= 0f) m_rhythmRenderer.color = m_baseColor;
            }
            
            
            if (m_nextPlayerEvent >= m_playerEvents.Count) return;
            
            float beat = m_beatClock.CurrentBeat;
            //Debug.Log(beat);
            while (m_eventIndex < m_rhythmEvents.Count && m_rhythmEvents[m_eventIndex].BeatCount <= beat)
            {
                if (beat - m_rhythmEvents[m_eventIndex].BeatCount > m_badWindow)
                {
                    Debug.Log("MISS");
                    m_rhythmRenderer.color = Color.red;
                    m_flashTimer = m_flashDuration;
                    m_nextPlayerEvent++;
                }
                ExecuteEvent(m_rhythmEvents[m_eventIndex]);
                //m_rhythmRenderer.color = Color.white; 
                m_eventIndex++;
            }
            
            
        }

        private void ExecuteEvent(RhythmEvent rhythmEvent)
        {
            if (rhythmEvent.BeatType == "Cue")
            {
                m_rhythmRenderer.color = Color.gray;    
                m_flashTimer = m_flashDuration;
            }
            m_rhythmSource.PlayOneShot(m_rhythmClip);
            Debug.Log($"Event triggered at beat {rhythmEvent.BeatCount}: {rhythmEvent.BeatType}");
        }

        private void GameInput_OnButtonClicked(object sender, EventArgs eventArgs)
        {
            if (m_nextPlayerEvent >= m_playerEvents.Count)
            {
                return;
            }
            
            float beat = m_beatClock.CurrentBeat;
            RhythmEvent target = m_playerEvents[m_nextPlayerEvent];
            float playerError = beat - target.BeatCount;

            if (playerError < -m_badWindow)
            {
                return;
            }
            
            float absolutePlayerError = Mathf.Abs(playerError);
            JudgePlayerError(absolutePlayerError);
            m_nextPlayerEvent++;
            
            // Debug.Log($"Button clicked");
        }

        private void JudgePlayerError(float absolutePlayerError)
        {
            if (absolutePlayerError <= m_perfectWindow)
            {
                Debug.Log("PERFECT");
                m_rhythmRenderer.color = Color.green; 
            }
            else if (absolutePlayerError <= m_goodWindow)
            {
                Debug.Log("GOOD");
                m_rhythmRenderer.color = Color.yellow; 
            }
            else if (absolutePlayerError <= m_badWindow)
            {
                Debug.Log("BAD");
                m_rhythmRenderer.color = Color.red; 
            }
            else
            {
                Debug.Log("MISS");
                m_rhythmRenderer.color = Color.red;
            }
            m_flashTimer =  m_flashDuration;
        }

    }
}
