using UnityEngine;

namespace Managers
{
    public class OptionManager : MonoBehaviour
    {
        void Start()
        {
            QualitySettings.vSyncCount = 1;
        }

    }
}
