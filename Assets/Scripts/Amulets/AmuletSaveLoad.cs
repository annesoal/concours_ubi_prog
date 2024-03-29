using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Amulets
{
    public class AmuletSaveLoad
    {
        private bool hasBeenLoaded;
        private const string JSONFile = "save.json";
        private List<LevelAmuletsPair> FinalSave;

        public List<AmuletSO> GetAmuletsForScene(Loader.Scene scene)
        {
            Load();
            foreach (var savedScene in FinalSave)
                if (savedScene.level == (int)scene)
                    return AmuletsIDToAmulets(savedScene.amulets);
            return new List<AmuletSO>();
        }

        private List<AmuletSO> AmuletsIDToAmulets(int[] AmuletIDs)
        {
            var listOfAmulets = new List<AmuletSO>();
            foreach (var ID in AmuletIDs)
            foreach (var amulet in AmuletSelector.Instance.amulets)
                if (amulet.ID == ID)
                    listOfAmulets.Add(amulet);

            return listOfAmulets;
        }

        private void Load()
        {
            try
            {
                var reader = new StreamReader(JSONFile);
                var loadedJson = reader.ReadToEnd();
                var sf = JsonUtility.FromJson<SaveFile>(loadedJson);
                FinalSave = sf.listOfPairs.ToList();
                hasBeenLoaded = true;
            }
            catch (FileNotFoundException)
            {
                FinalSave = new List<LevelAmuletsPair>();
            }
        }

        private void Save()
        {
            var sf = new SaveFile();
            sf.listOfPairs = FinalSave.ToArray();
            var writer = new StreamWriter(JSONFile);
            var jsonToSave = JsonUtility.ToJson(sf);
            writer.Write(jsonToSave);
        }

        public void SaveSceneWithAmulets(Loader.Scene scene, AmuletSO[] amulets)
        {
            if (!hasBeenLoaded) 
                Load();
            
            var levelAmuletsPair = new LevelAmuletsPair();
            levelAmuletsPair.level = (int)scene;
            levelAmuletsPair.amulets = new int[amulets.Length];
            
            for (var i = 0; i < amulets.Length; i++)
            {
                var amulet = amulets[i];
                levelAmuletsPair.amulets[i] = amulet.ID;
            }

            AddOrOverwriteSave(levelAmuletsPair);
            Save();
        }

        private void AddOrOverwriteSave(LevelAmuletsPair pair)
        {
            for (var i = 0; i < FinalSave.Count; i++)
            {
                var savedPair = FinalSave[i];
                if (savedPair.level == pair.level)
                {
                    FinalSave[i] = pair;
                    break;
                }
            }

            FinalSave.Add(pair);
        }

        [Serializable]
        private class SaveFile
        {
            public LevelAmuletsPair[] listOfPairs;
        }

        [Serializable]
        private class LevelAmuletsPair
        {
            public int level;
            public int[] amulets;
        }
    }
}