using System;
using Framework.Tools.Data;
using UnityEngine;

namespace Game.Data
{
    [Serializable]
    public class Data
    {
        public int Level = 1;
        public int Score;
    }

    public static class GameData
    {
        public const string FileName = "GameData";

        private static readonly JsonDataKeeper<Data> DataKeeper;

        public static Data Data { get; private set; }

        static GameData()
        {
            DataKeeper = new JsonDataKeeper<Data>(Application.persistentDataPath + "/" + FileName, true);
        }

        public static void Load()
        {
            Data = DataKeeper.Load();
        }

        public static void Save()
        {
            DataKeeper.Save(Data);
        }
    }
}