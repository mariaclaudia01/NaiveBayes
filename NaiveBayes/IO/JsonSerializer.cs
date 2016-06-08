using System.IO;
using NaiveBayes.Classifiers;
using Newtonsoft.Json;

namespace NaiveBayes.IO
{
    public class JsonSerializer
    {
        private readonly ITrainable classifier;

        public JsonSerializer(ITrainable classifier)
        {
            this.classifier = classifier;
        }

        public void SerializeTo(string outputFile)
        {
            string serialization = JsonConvert.SerializeObject(classifier, Formatting.Indented);
            File.WriteAllText(outputFile, serialization);
        }
    }
}
