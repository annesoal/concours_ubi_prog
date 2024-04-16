using Unity.Netcode.Components;

namespace Utils
{
    public class OwnerNetworkAnimator : NetworkAnimator
    {
        protected override bool OnIsServerAuthoritative()
        {
            return false;
        } 
    }
}