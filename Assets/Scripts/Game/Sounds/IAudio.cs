namespace Morkwa.Interface
{
    public interface IAudio
    {
        IAudio GetAudioManager();
        void PlaySoundFinish();
        void PlaySoundGameOver();
        void AddNoiseValue();
        void PlaySoundAlert();
        void SetDefaultState();
        float GetCurrectNoise();
        float GetNoiseDetection();
    }
}