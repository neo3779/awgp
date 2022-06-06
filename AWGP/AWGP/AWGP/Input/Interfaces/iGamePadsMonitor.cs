using Microsoft.Xna.Framework.Input;

namespace AWGP
{
    public interface iGamePadsMonitor : iControlsMonitor
    {
        Buttons[] ButtonsPressed();

        Buttons[] ButtonsReleased();
    }
}
