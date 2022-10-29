using Hsinpa.Utility;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System.Linq;
using System.IO;

namespace Shingrix.Data {
    public class RankModel
    {
        ShingrixStatic.RankSetsStruct m_fullSets;

        public List<ShingrixStatic.RankStruct> DataArray => m_fullSets.sets;

        public RankModel()
        {
            m_fullSets = GetFullDataFromIO();
        }

        public void PushCurrentRankStruct(ShingrixStatic.RankStruct p_rankStruct) {
            m_fullSets.sets.Add(p_rankStruct);
        }

        public async Task<int> GetIndex(ShingrixStatic.RankStruct rankStruct) {
            return await Task.Run(() =>
            {
                return m_fullSets.sets.FindIndex(x => x.id == rankStruct.id);
            });
        }

        public async Task<List<ShingrixStatic.RankStruct>> GetScoreSortList() {
            var sorted = await Task.Run(() =>
            {
                return m_fullSets.sets.OrderByDescending(x => x.score).ToList();
            });

            m_fullSets.sets = sorted;

            return  sorted;
        }

        public void SaveToDisk() {

            IOUtility.SaveFileText(GetFullFilePath(), JsonUtility.ToJson(m_fullSets));
        }

        private ShingrixStatic.RankSetsStruct GetFullDataFromIO() {
            try
            {
                string rawJSON = IOUtility.GetFileText(GetFullFilePath());

                if (!string.IsNullOrEmpty(rawJSON))
                {
                    return JsonUtility.FromJson<ShingrixStatic.RankSetsStruct>(rawJSON);
                }
            }
            catch {
                Debug.LogError("File not exist");
            }

            return new ShingrixStatic.RankSetsStruct() { sets = new List<ShingrixStatic.RankStruct>() };
        }

        private string GetFullFilePath() {
            return Path.Combine(Application.persistentDataPath, ShingrixStatic.IO.FilePath);
        }
    }
}
