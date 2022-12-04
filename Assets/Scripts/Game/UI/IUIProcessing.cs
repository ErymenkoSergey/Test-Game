namespace Morkwa.Interface
{
    public interface IUIProcessing
    {
        void SetNoiseDetected(float value);
        void SetValueNoiseSlider(float value);
        void GameOver(bool isWin);
    }
}