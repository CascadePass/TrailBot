using System;
using System.IO;
using System.Xml.Serialization;

namespace CascadePass.TrailBot
{
    public class FileStore
    {
        public static bool SerializeToXmlFile(object o, string filename)
        {
            return FileStore.SerializeToXmlFile(o, filename, null);
        }

        public static bool SerializeToXmlFile(object o, string filename, Type[] additionalTypes)
        {
            if (o == null || string.IsNullOrWhiteSpace(filename))
            {
                return false;
            }

            try
            {
                if (File.Exists(filename))
                {
                    string backupFile = $"{filename}.old";

                    if (File.Exists(backupFile))
                    {
                        File.Delete(backupFile);
                    }

                    File.Move(filename, backupFile);
                }

                XmlSerializer serializer = additionalTypes == null ? new(o.GetType()) : new(o.GetType(), additionalTypes);
                using StreamWriter writer = new(filename);

                serializer.Serialize(writer, o);
                writer.Flush();
                writer.Close();
            }
            catch (Exception ex)
            {
                string backupFile = $"{filename}.old";

                File.Delete(filename);
                File.Move(backupFile, filename);
                return false;
            }

            return true;
        }

        public static T DeserializeFromXmlFile<T>(string filename)
        {
            if (!File.Exists(filename))
            {
                return default;
            }

            XmlSerializer serializer = new(typeof(T));
            using StreamReader reader = new(filename);

            object o = serializer.Deserialize(reader);

            if (o is T t)
            {
                return t;
            }

            return default;
        }


        public static bool TestWriteAccess(string path)
        {
            try
            {
                string testFile = Path.Combine(path, Path.GetRandomFileName());

                using FileStream fs = File.Create(
                    testFile,
                    1,
                    FileOptions.DeleteOnClose
                );

                return true;
            }
            catch (UnauthorizedAccessException authEx)
            {
                System.Diagnostics.Debug.WriteLine(authEx.Message);
                return false;
            }
        }

    }
}
