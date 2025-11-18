using System;
using System.Collections;
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

        [SerializeField] private CatVisual m_cueCat;
        [SerializeField] private CatVisual m_playerCat;
        [SerializeField] private SpriteRenderer m_rhythmErrorLight;
        
        [SerializeField] private CountDownUI m_countDownUI;

        [SerializeField] private Transform m_turnIndicator;
        // [SerializeField] private Sprite m_silenceSprite;
        // [SerializeField] private Sprite m_soundSprite;
        private Color m_baseColor = Color.white;
        private Color m_cueFlashColor = Color.gray;

        private int m_eventIndex = 0;
        private int m_nextPlayerEvent = 0;
        // private float m_flashDuration = 0.1f;
        // private float m_flashTimer = 0.0f;
        private float m_flashErrorDuration = 0.1f;
        private float m_flashErrorTimer = 0.0f;


        private List<RhythmEvent> m_rhythmEvents = new List<RhythmEvent>();
        
        private List<RhythmEvent> m_playerEvents = new List<RhythmEvent>();

        private void Start()
        {
            m_rhythmEvents.Add(new RhythmEvent{ BeatCount = 1f, BeatType = "Cue"} );
            m_rhythmEvents.Add(new RhythmEvent{ BeatCount = 2f, BeatType = "Cue"} );
            m_rhythmEvents.Add(new RhythmEvent{ BeatCount = 3f, BeatType = "Cue"} );
            m_rhythmEvents.Add(new RhythmEvent{ BeatCount = 4f, BeatType = "Cue"} );
            m_rhythmEvents.Add(new RhythmEvent{ BeatCount = 4.5f, BeatType = "Switch"} );
            m_rhythmEvents.Add(new RhythmEvent{ BeatCount = 5f, BeatType = "Press"} );
            m_rhythmEvents.Add(new RhythmEvent{ BeatCount = 6f, BeatType = "Press"} );
            m_rhythmEvents.Add(new RhythmEvent{ BeatCount = 7f, BeatType = "Press"} );
            m_rhythmEvents.Add(new RhythmEvent{ BeatCount = 8f, BeatType = "Press"} );
            m_rhythmEvents.Add(new RhythmEvent{ BeatCount = 8.5f, BeatType = "Switch"} );
            m_rhythmEvents.Add(new RhythmEvent{ BeatCount = 9f, BeatType = "Cue"} );
            m_rhythmEvents.Add(new RhythmEvent{ BeatCount = 10f, BeatType = "Cue"} );
            m_rhythmEvents.Add(new RhythmEvent{ BeatCount = 12f, BeatType = "Cue"} );
            m_rhythmEvents.Add(new RhythmEvent{ BeatCount = 12.5f, BeatType = "Switch"} );
            m_rhythmEvents.Add(new RhythmEvent{ BeatCount = 13f, BeatType = "Press"} );
            m_rhythmEvents.Add(new RhythmEvent{ BeatCount = 14f, BeatType = "Press"} );
            m_rhythmEvents.Add(new RhythmEvent{ BeatCount = 16f, BeatType = "Press"} );
            m_rhythmEvents.Add(new RhythmEvent{ BeatCount = 16.5f, BeatType = "Switch"} );
            m_rhythmEvents.Add(new RhythmEvent{ BeatCount = 17f, BeatType = "Cue"} );
            m_rhythmEvents.Add(new RhythmEvent{ BeatCount = 17.25f, BeatType = "Cue"} );
            m_rhythmEvents.Add(new RhythmEvent{ BeatCount = 18f, BeatType = "Cue"} );
            m_rhythmEvents.Add(new RhythmEvent{ BeatCount = 19f, BeatType = "Cue"} );
            m_rhythmEvents.Add(new RhythmEvent{ BeatCount = 19.25f, BeatType = "Cue"} );
            m_rhythmEvents.Add(new RhythmEvent{ BeatCount = 20f, BeatType = "Cue"} );
            m_rhythmEvents.Add(new RhythmEvent{ BeatCount = 20.5f, BeatType = "Switch"} );
            m_rhythmEvents.Add(new RhythmEvent{ BeatCount = 21f, BeatType = "Press"} );
            m_rhythmEvents.Add(new RhythmEvent{ BeatCount = 21.25f, BeatType = "Press"} );
            m_rhythmEvents.Add(new RhythmEvent{ BeatCount = 22f, BeatType = "Press"} );
            m_rhythmEvents.Add(new RhythmEvent{ BeatCount = 23f, BeatType = "Press"} );
            m_rhythmEvents.Add(new RhythmEvent{ BeatCount = 23.25f, BeatType = "Press"} );
            m_rhythmEvents.Add(new RhythmEvent{ BeatCount = 24f, BeatType = "Press"} );
            m_rhythmEvents.Add(new RhythmEvent{ BeatCount = 25f, BeatType = "End"} );

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
            // Debug.Log(m_beatClock.CurrentBeat);
            // if (m_flashTimer > 0f)
            // {
            //     m_flashTimer -= Time.deltaTime;
            //     if (m_flashTimer <= 0f) m_rhythmRenderer.sprite = m_silenceSprite;
            // }

            if (m_flashErrorTimer > 0f)
            {
                m_flashErrorTimer -= Time.deltaTime;
                if (m_flashErrorTimer <= 0f) m_rhythmErrorLight.color = Color.white;
            }
            
            // if (m_nextPlayerEvent >= m_playerEvents.Count) return;
            
            float beat = m_beatClock.CurrentBeat;
            //Debug.Log(beat);
            while (m_eventIndex < m_rhythmEvents.Count && m_rhythmEvents[m_eventIndex].BeatCount <= beat)
            {
                if (m_rhythmEvents[m_eventIndex].BeatType == "Cue")
                {
                    ExecuteEvent(m_rhythmEvents[m_eventIndex]);   
                }
                else if (m_rhythmEvents[m_eventIndex].BeatType == "Switch")
                {
                    m_turnIndicator.Rotate(new Vector3(0f,0f, 180f));
                }
                else if (m_rhythmEvents[m_eventIndex].BeatType == "End")
                {
                    m_beatClock.StopClock();   
                }
                else if (beat - m_playerEvents[m_nextPlayerEvent].BeatCount > m_badWindow)
                {
                    Debug.Log("MISS");
                    // m_rhythmErrorLight.color = Color.red;
                    // m_flashErrorTimer = m_flashErrorDuration;
                    m_nextPlayerEvent++;
                }
                //m_rhythmRenderer.color = Color.white; 
                m_eventIndex++;
            }
            
            
        }

        private void ExecuteEvent(RhythmEvent rhythmEvent)
        {
            if (rhythmEvent.BeatType == "Cue")
            {
                m_cueCat.TriggerVisual();
             // m_rhythmRenderer.color = Color.gray;
             // m_rhythmRenderer.sprite = m_soundSprite;
             // m_flashTimer = m_flashDuration;
            }
            else if (rhythmEvent.BeatType == "Press")
            {
                m_playerCat.TriggerVisual();
            }
            
            m_rhythmSource.PlayOneShot(m_rhythmClip);
            Debug.Log($"Event triggered at beat {rhythmEvent.BeatCount}: {rhythmEvent.BeatType}");
        }

        private void GameInput_OnButtonClicked(object sender, EventArgs eventArgs)
        {
            if (!m_beatClock.GetIsPlaying())
            {
                StartCoroutine(StartCountdown());
                // m_beatClock.StartClock();
                return;
            }
            
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
            ExecuteEvent(target);
            m_nextPlayerEvent++;
            
            // Debug.Log($"Button clicked");
        }

        private void JudgePlayerError(float absolutePlayerError)
        {
            if (absolutePlayerError <= m_perfectWindow)
            {
                Debug.Log("PERFECT");
                m_rhythmErrorLight.color = Color.green; 
            }
            else if (absolutePlayerError <= m_goodWindow)
            {
                Debug.Log("GOOD");
                m_rhythmErrorLight.color = Color.yellow; 
            }
            else if (absolutePlayerError <= m_badWindow)
            {
                Debug.Log("BAD");
                m_rhythmErrorLight.color = Color.red; 
            }
            else
            {
                Debug.Log("MISS");
                m_rhythmErrorLight.color = Color.red;
            }
            m_flashErrorTimer =  m_flashErrorDuration;
        }

        private IEnumerator StartCountdown()
        {
            // start music
            
            yield return new WaitForSeconds(6);
            m_countDownUI.SetActive(true);
            m_countDownUI.SetCountdownText("3");
            yield return new WaitForSeconds(1);
            m_countDownUI.SetCountdownText("2");
            yield return new WaitForSeconds(1);
            m_countDownUI.SetCountdownText("1");
            yield return new WaitForSeconds(1);
            m_countDownUI.SetCountdownText("GO");
            yield return new WaitForSeconds(1);
            m_countDownUI.SetActive(false);
            m_beatClock.StartClock();
        }

    }
}
