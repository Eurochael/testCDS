using System;
using System.IO;
using System.Text;
using YamlDotNet.Serialization;
//1.0.0.0
namespace LogManager
{
    class Module_YAML
    {
        public string Serialize<T>(string path, string fileName, T value)
        {
            String result = "";
            try
            {
                if (Directory.Exists(Program.cg_main.path.yaml) == false) { Directory.CreateDirectory(Program.cg_main.path.yaml); }
                var builder = new SerializerBuilder().Build();
                using (var stream = File.Create(path + fileName))
                {
                    using (var writer = new StreamWriter(stream))
                    {
                        builder.Serialize(writer, value);
                    }
                }
                result = "";
            }
            catch (Exception ex)
            {
                result = ex.ToString();
            }
            return result;
        }
        public Byte[] DeSerialize(string path, string fileName)
        {
            String result = "";
            Byte[] data;
            try
            {
                var builder = new DeserializerBuilder().Build();
                using (var stream = File.Open(path + fileName, FileMode.Open, FileAccess.Read))
                {
                    var ms = new MemoryStream();
                    stream.CopyTo(ms);
                    data =  ms.ToArray();
                }
                result = "";
            }
            catch (Exception ex)
            {
                result = ex.ToString();
                data = null;
            }
            return data;
        }
        public T DeSerialize<T>(string path, string fileName) where T : class
        {
            var builder = new DeserializerBuilder().Build();
            using (var stream = File.Open(path + fileName, FileMode.Open, FileAccess.Read))
            {
                using (var reader = new StreamReader(stream))
                {
                    return builder.Deserialize(reader, typeof(T)) as T;
                }
            }
        }
        public T DeSerialize<T>(Byte[] data) where T : class
        {
            var builder = new DeserializerBuilder().Build();
            return builder.Deserialize(Encoding.Default.GetString(data), typeof(T)) as T;
        }
    }
}
