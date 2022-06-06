
namespace AWGP
{
    interface iMouseMonitor : iControlsMonitor
    {
        AWGP.MouseMonitor.MouseButtons[] ButtonsPressed();

        AWGP.MouseMonitor.MouseButtons[] ButtonsReleased();
    }
}
