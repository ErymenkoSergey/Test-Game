namespace Morkwa.Interface
{
    public interface IVisions
    {
        void SetVision(float distance, float angle);
        void SetStatusIsSeeing(bool status);
        bool GetIsSeeingStatus();
    }
}