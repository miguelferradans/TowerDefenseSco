using UnityEngine;

namespace TD.Data.Persistent
{
    public static class SaveDataLoader
    {
        public static void SaveData(SaveData data)
        {
            PlayerPrefs.SetString(kSaveDataKey, data.ToJson());
        }

        public static SaveData LoadData()
        {
            var json = PlayerPrefs.GetString(kSaveDataKey);

            var saveData = JsonUtility.FromJson<SaveData>(json);

            return (null != saveData) ? saveData : new SaveData();
        }

        private const string kSaveDataKey = "SaveData";
    }
}