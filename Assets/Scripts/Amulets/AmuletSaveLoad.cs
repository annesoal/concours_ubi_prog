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
        private List<LevelAmuletsPair> finalSave;

        public AmuletSaveLoad()
        {
            finalSave = new List<LevelAmuletsPair>();
        }

        public List<AmuletSO> GetAmuletsForScene(Loader.Scene scene, AmuletSO[] referenceAmuletsList)
        {
            Load();

            foreach (LevelAmuletsPair savedScene in finalSave)
            {
                if (savedScene.level == scene)
                {
                    return AmuletsIDToAmulets(savedScene.amulets, referenceAmuletsList);
                }
            }
            
            return new List<AmuletSO>();
        }
        
        public int[] GetAmuletsIdsForScene(Loader.Scene scene)
        {
            Load();
            foreach (var savedScene in finalSave)
            {
                if (savedScene.level == scene)
                {
                    return savedScene.amulets;
                }
            }

            return new int[] {};
        }

        public List<AmuletSO> AmuletsIDToAmulets(int[] amuletIDs, AmuletSO[] referenceAmuletsList)
        {
            var listOfAmulets = new List<AmuletSO>();
            foreach (int ID in amuletIDs)
            {
                foreach (AmuletSO amulet in referenceAmuletsList)
                {
                    if (amulet.ID == ID)
                    {
                        listOfAmulets.Add(amulet);
                    }
                }
            }

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
                    finalSave = sf.listOfPairs.ToList();
                }
                catch (NullReferenceException)
                {
                    finalSave = new List<LevelAmuletsPair>();
                }
            }
            else
            {
                finalSave = new List<LevelAmuletsPair>();
            }
            hasBeenLoaded = true;
        }

        private void Save()
        {
            SaveFile sf = new SaveFile();
            sf.listOfPairs = finalSave.ToArray();

            string jsonToSave = JsonUtility.ToJson(sf);
            
            File.WriteAllText(Application.dataPath + SAVE_FILE_PATH, jsonToSave);
        }

        public void SaveSceneWithAmulets(Loader.Scene scene, AmuletSO[] amulets)
        {
            if (!hasBeenLoaded) 
                Load();
            
            var levelAmuletsPair = new LevelAmuletsPair
            {
                level = scene,
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
            for (var i = 0; i < finalSave.Count; i++)
            {
                var savedPair = finalSave[i];
                if (savedPair.level == pair.level)
                {
                    finalSave[i] = pair;
                    return;
                }
            }

            finalSave.Add(pair);
        }

        [Serializable]
        private class SaveFile
        {
            public LevelAmuletsPair[] listOfPairs;
        }

        [Serializable]
        private struct LevelAmuletsPair
        {
            public Loader.Scene level;
            public int[] amulets;
        }
    }
}