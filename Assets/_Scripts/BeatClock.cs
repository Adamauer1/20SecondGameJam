using UnityEngine;

namespace _Scripts
{
    public class BeatClock : MonoBehaviour
    {
        //public static BeatClock Instance { get; private set; }

        [SerializeField] private float m_bpm = 120f;

        private double m_dspStart;
        private float m_secondsPerBeat;
        
        public float CurrentBeat => (float)((AudioSettings.dspTime - m_dspStart) / m_secondsPerBeat);
        
        
        // private void Awake()
        // {
        //     Instance = this;
        // }

        private void Start()
        {
            m_secondsPerBeat = 60 / m_bpm;
            m_dspStart = AudioSettings.dspTime;
            
        }
    }
}
