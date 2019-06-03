using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace Framework.Tools.Data
{
    public class JsonDataKeeper<T> where T : class, new()
    {
        private readonly string _path;
        private readonly bool _storeDataAsBase64;

        public JsonDataKeeper(string path, bool storeDataAsBase64 = false)
        {
            _path = path;
            _storeDataAsBase64 = storeDataAsBase64;
        }

        public T Load()
        {
            if (!File.Exists(_path))
            {
                return new T();
            }

            using (var streamReader = new StreamReader(_path))
            {
                var serializedObject = streamReader.ReadToEnd();
                if (_storeDataAsBase64 && IsBase64Encoded(serializedObject))
                {
                    serializedObject = Encoding.UTF8.GetString(Convert.FromBase64String(serializedObject));
                }

                try
                {
                    return JsonUtility.FromJson<T>(serializedObject);
                }
                catch (Exception e)
                {
                    Debug.LogFormat("Cannot load data from json: {0}. Creating new data.", e);
                    return new T();
                }
            }
        }

        public void Save(T data)
        {
            using (var streamWriter = new StreamWriter(_path))
            {
                var serializedObject = JsonUtility.ToJson(data);
                if (_storeDataAsBase64)
                {
                    serializedObject = Convert.ToBase64String(Encoding.UTF8.GetBytes(serializedObject));
                }

                streamWriter.Write(serializedObject);
            }
        }

        private static bool IsBase64Encoded(string base64String)
        {
            try
            {
                Convert.FromBase64String(base64String);
                return base64String.Replace(" ", "").Length % 4 == 0;
            }
            catch
            {
                return false;
            }
        }
    }
}