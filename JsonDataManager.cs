using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;

namespace Task03
{
    public class JsonDataManager
    {
        public void SaveToFile<T>(string filePath, List<T> data)
        {
            string jsonData = JsonConvert.SerializeObject(data);
            File.WriteAllText(filePath, jsonData);
        }
        public List<T> LoadFromFile<T>(string filePath)
        {
            if (File.Exists(filePath))
            {
                string jsonData = File.ReadAllText(filePath);
                return JsonConvert.DeserializeObject<List<T>>(jsonData);
            }
            else
            {
                return new List<T>();
            }
        }
    }
}