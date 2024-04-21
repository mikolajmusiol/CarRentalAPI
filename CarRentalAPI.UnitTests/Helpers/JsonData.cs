using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit.Sdk;

namespace CarRentalAPI.UnitTests.Helpers
{
    public class JsonData : DataAttribute
    {
        private readonly string _jsonData;

        public JsonData(string jsonPath)
        {
            _jsonData = jsonPath;
        }
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            if (testMethod == null) throw new ArgumentNullException(nameof(testMethod));

            var currectDir = Directory.GetCurrentDirectory();
            var jsonFullPath = Path.GetRelativePath(currectDir, _jsonData);

            if (!File.Exists(jsonFullPath))
            {
                throw new ArgumentException($"Couldn't find file: {jsonFullPath}");
            }

            var jsonData = File.ReadAllText(jsonFullPath);
            var deserialized = JsonConvert.DeserializeObject<IEnumerable<object[]>>(jsonData);

            return deserialized;

        }
    }
}
