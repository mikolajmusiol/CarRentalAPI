using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Reflection;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace CarRentalAPI.IntegrationTests.Helpers
{
    public class JsonData<T> : DataAttribute
    {
        private readonly string _jsonPath;
        private readonly string _jsonProperty;

        public JsonData(string jsonPath, string property)
        {
            _jsonPath = jsonPath;
            _jsonProperty = property;
        }

        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            if (testMethod == null) throw new ArgumentNullException(nameof(testMethod));

            var jsonFile = GetJsonFromFile();
            var jsonData = GetJsonDataByProperty(jsonFile);
            return ParseJson(jsonData);
        }

        private IEnumerable<object[]> ParseJson(string jsonData)
        {
            var genericObjectArray = JsonConvert.DeserializeObject<T[]>(jsonData);
            if (genericObjectArray == null) throw new ArgumentNullException(nameof(genericObjectArray));

            List<object[]> data = new List<object[]>();
            foreach (var obj in genericObjectArray)
            {
                data.Add(new object[] { obj });
            }
            return data;
        }

        private string GetJsonDataByProperty(string jsonFile)
        {
            var parsedJson = JObject.Parse(jsonFile);
            if (parsedJson[_jsonProperty] == null)
            {
                throw new ArgumentNullException(nameof(_jsonProperty));
            }
            return parsedJson[_jsonProperty].ToString();

        }

        private string GetJsonFromFile()
        {
            var currectDir = Directory.GetCurrentDirectory();
            var jsonFullPath = Path.GetRelativePath(currectDir, _jsonPath);

            if (!File.Exists(jsonFullPath))
            {
                throw new ArgumentException($"Couldn't find file: {jsonFullPath}");
            }

            return File.ReadAllText(jsonFullPath);
        }
    }
}
