using System.Collections.Generic;
using UnityEngine;

namespace Framework.Signals
{
    [CreateAssetMenu(fileName = "SignalsStorage", menuName = "Signals/SignalsStorage")]
    public class SignalsStorage : ScriptableObject
    {
        public const string AssetPath = "Assets/Resources/Storage/";
        public List<string> Signals = new List<string>();
    }
}