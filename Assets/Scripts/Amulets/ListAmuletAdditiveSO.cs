using System.Collections.Generic;
using UnityEngine;

namespace Amulets
{
    [CreateAssetMenu(menuName = "ListAmuletAdditiveSO")]
    public class ListAmuletAdditiveSO : ScriptableObject
    {
        public List<AdditionAmuletSO> additionAmuletSos;
    }
}