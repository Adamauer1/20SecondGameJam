using UnityEngine;

namespace _Scripts
{
    [DefaultExecutionOrder(-200)]
    public class BeatClock : MonoBehaviour
    {
        //public static BeatClock Instance { get; private set; }

        [SerializeField] private float m_bpm = 80f;

        [SerializeField] private int m_beatsPerMeasure = 4;
        [SerializeField] private int m_beatUnit = 4;
        
        private double m_dspStart;
        private float m_secondsPerBeat;
        
        // public float CurrentBeat => (float)((AudioSettings.dspTime - m_dspStart) / m_secondsPerBeat);
        public float CurrentBeat { get; private set; }
        
        public int CurrentMeasure => Mathf.FloorToInt(CurrentBeat / m_beatsPerMeasure);
        public float BeatInMeasure => CurrentBeat % m_beatsPerMeasure;
        
        private void Awake()
        {
            // Instance = this;
            m_secondsPerBeat = 60f / m_bpm;
        }

        private void Start()
        {
            m_secondsPerBeat = 60 / m_bpm;
            m_dspStart = AudioSettings.dspTime;
            
        }

        private void Update()
        {
            double eslapse = AudioSettings.dspTime - m_dspStart;
            CurrentBeat = (float) (eslapse / m_secondsPerBeat);
        }
    }
}
