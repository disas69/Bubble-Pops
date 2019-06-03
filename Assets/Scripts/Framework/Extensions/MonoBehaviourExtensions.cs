using System;
using System.Collections;
using UnityEngine;

namespace Framework.Extensions
{
    public static class MonoBehaviourExtensions
    {
        public static T GetOrAddComponent<T>(this Component component) where T : Component
        {
            return component.GetComponent<T>() ?? component.gameObject.AddComponent<T>();
        }

        public static void SafeStopCoroutine(this MonoBehaviour monoBehaviour, Coroutine coroutine)
        {
            if (coroutine != null)
            {
                monoBehaviour.StopCoroutine(coroutine);
            }
        }

        public static void WaitForSeconds(this MonoBehaviour behaviour, float seconds, Action action)
        {
            behaviour.StartCoroutine(WaitForSeconds(seconds, action));
        }

        public static void WaitForNextFrame(this MonoBehaviour behaviour, Action action)
        {
            behaviour.StartCoroutine(WaitCoroutine((YieldInstruction) null, action));
        }

        public static void WaitForEndOfFrame(this MonoBehaviour behaviour, Action action)
        {
            behaviour.StartCoroutine(WaitCoroutine(new WaitForEndOfFrame(), action));
        }

        public static void WaitForFrames(this MonoBehaviour behaviour, int framesCount, Action action)
        {
            behaviour.StartCoroutine(WaitCoroutine(framesCount, action));
        }

        public static void WaitUntil(this MonoBehaviour behaviour, Func<bool> condition, Action action)
        {
            behaviour.StartCoroutine(WaitCoroutine(condition, action));
        }

        private static IEnumerator WaitForSeconds(float seconds, Action action)
        {
            yield return new WaitForSeconds(seconds);

            if (action != null)
            {
                action.Invoke();
            }
        }

        private static IEnumerator WaitCoroutine(YieldInstruction waitInstruction, Action action)
        {
            yield return waitInstruction;

            if (action != null)
            {
                action.Invoke();
            }
        }

        private static IEnumerator WaitCoroutine(int framesCount, Action action)
        {
            int frames = 0;

            while (frames < framesCount)
            {
                yield return null;
                frames++;
            }

            if (action != null)
            {
                action.Invoke();
            }
        }

        private static IEnumerator WaitCoroutine(Func<bool> condition, Action action)
        {
            if (condition != null)
            {
                while (!condition())
                {
                    yield return null;
                }
            }

            if (action != null)
            {
                action.Invoke();
            }
        }
    }
}