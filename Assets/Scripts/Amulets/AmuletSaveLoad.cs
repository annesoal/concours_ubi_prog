using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Amulets
{
    public class AmuletSaveLoad 
    {
        private bool hasBeenLoaded = false;
        private const string SAVE_FILE_PATH = "/save.json";
        private List<LevelAmuletsPair> FinalSave = new();

        public List<AmuletSO> GetAmuletsForScene(Loader.Scene scene)
        {
            Load();
            foreach (var savedScene in FinalSave)
                if (savedScene.level == (int)scene)
                    return AmuletsIDToAmulets(savedScene.amulets);
            return new List<AmuletSO>();
        }
        
        public int[] GetAmuletsIdsForScene(Loader.Scene scene)
        {
            Load();
            foreach (var savedScene in FinalSave)
                if (savedScene.level == (int)scene)
                    return savedScene.amulets;

            return new int[] {};
        }

        public List<AmuletSO> AmuletsIDToAmulets(int[] AmuletIDs)
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
            if (File.Exists(Application.dataPath + SAVE_FILE_PATH))
            {
                string savedJson = File.ReadAllText(Application.dataPath + SAVE_FILE_PATH);

                SaveFile sf = JsonUtility.FromJson<SaveFile>(savedJson);
                
                try
                {
                    FinalSave = sf.listOfPairs.ToList();
                }
                catch (NullReferenceException)
                {
                    FinalSave = new List<LevelAmuletsPair>();
                }
            }
            else
            {
                FinalSave = new List<LevelAmuletsPair>();
            }
            hasBeenLoaded = true;
        }

        private void Save()
        {
            SaveFile sf = new SaveFile();
            sf.listOfPairs = FinalSave.ToArray();

            string jsonToSave = JsonUtility.ToJson(sf);
            
            File.WriteAllText(Application.dataPath + SAVE_FILE_PATH, jsonToSave);
        }

        public void SaveSceneWithAmulets(Loader.Scene scene, AmuletSO[] amulets)
        {
            if (!hasBeenLoaded) 
                Load();
            
            var levelAmuletsPair = new LevelAmuletsPair
            {
                level = (int)scene,
                amulets = new int[amulets.Length]
            };
            Debug.Log(levelAmuletsPair.level); 
            for (var i = 0; i < amulets.Length; i++)
            {
                var amulet = amulets[i];
                levelAmuletsPair.amulets[i] = amulet.ID;
                Debug.Log(levelAmuletsPair.amulets[i]);
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
                    return;
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
        private struct LevelAmuletsPair
        {
            public int level;
            public int[] amulets;
        }
    }
}