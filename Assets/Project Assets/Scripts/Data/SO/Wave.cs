namespace Project_Assets.Scripts.Data.SO
{
    public class Wave
    {
        private WaveSO _waveSO;

        public float AdditionalWaveDuration;
        
        public float WaveDuration => _waveSO.Duration + AdditionalWaveDuration;

        public Wave(WaveSO waveSO)
        {
            _waveSO = waveSO;
        }
        
        public void Reset()
        {
            AdditionalWaveDuration = 0;
        }
    }
}