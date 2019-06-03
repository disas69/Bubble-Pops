using UnityEngine;
using UnityEngine.UI;

namespace Framework.Utils.Debug
{
    public class FPSCounter : MonoBehaviour
    {
        private const string DisplayTemplate = "{0} FPS";

        private static FPSCounter _instance;
        private float _nextMeasurePeriod;
        private int _fpsAccumulator;
        private int _currentFps;

        public bool IsEnabled;
        public float MeasurePeriod = 1f;
        public Text Output;

        private void Awake()
        {
            _instance = this;

            if (IsEnabled)
            {
                Output.gameObject.SetActive(true);
                _nextMeasurePeriod = Time.realtimeSinceStartup + MeasurePeriod;
            }
            else
            {
                Output.gameObject.SetActive(false);
            }
        }

        public static void Enable(bool value)
        {
            if (_instance == null)
            {
                return;
            }

            _instance.IsEnabled = value;
            _instance.Output.gameObject.SetActive(value);

            if (value)
            {
                _instance._nextMeasurePeriod = Time.realtimeSinceStartup + _instance.MeasurePeriod;
            }
        }

        private void Update()
        {
            if (IsEnabled)
            {
                _fpsAccumulator++;

                if (Time.realtimeSinceStartup > _nextMeasurePeriod)
                {
                    _currentFps = (int) (_fpsAccumulator / MeasurePeriod);
                    _nextMeasurePeriod += MeasurePeriod;
                    _fpsAccumulator = 0;

                    Output.text = string.Format(DisplayTemplate, _currentFps);
                }
            }
        }
    }
}