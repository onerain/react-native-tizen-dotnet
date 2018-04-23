using System;
using System.IO;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Schema;

using Tizen.Applications;
using ReactNative.Tracing;
using PCLStorage;

namespace ReactNative.Common
{
    [Serializable]
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, IXmlSerializable
    {
        public SerializableDictionary() { }
        public void WriteXml(XmlWriter write)       // Serializer  
        {
            XmlSerializer KeySerializer = new XmlSerializer(typeof(TKey));
            XmlSerializer ValueSerializer = new XmlSerializer(typeof(TValue));

            foreach (KeyValuePair<TKey, TValue> kv in this)
            {
                write.WriteStartElement("SerializableDictionary");
                write.WriteStartElement("key");
                KeySerializer.Serialize(write, kv.Key);
                write.WriteEndElement();
                write.WriteStartElement("value");
                ValueSerializer.Serialize(write, kv.Value);
                write.WriteEndElement();
                write.WriteEndElement();
            }
        }
        public void ReadXml(XmlReader reader)       // Deserializer  
        {
            reader.Read();
            XmlSerializer KeySerializer = new XmlSerializer(typeof(TKey));
            XmlSerializer ValueSerializer = new XmlSerializer(typeof(TValue));

            while (reader.NodeType != XmlNodeType.EndElement)
            {
                reader.ReadStartElement("SerializableDictionary");
                reader.ReadStartElement("key");
                TKey tk = (TKey)KeySerializer.Deserialize(reader);
                reader.ReadEndElement();
                reader.ReadStartElement("value");
                TValue vl = (TValue)ValueSerializer.Deserialize(reader);
                reader.ReadEndElement();
                reader.ReadEndElement();
                this.Add(tk, vl);
                reader.MoveToContent();
            }
            reader.ReadEndElement();

        }
        public XmlSchema GetSchema()
        {
            return null;
        }
    }

    public static class ReactConfig
    {
        static SerializableDictionary<string, string> SerializableDictionary;
        static string LocalDataFile;

        static ReactConfig()
        {
            SerializableDictionary = new SerializableDictionary<string, string>();

            var localStorage = FileSystem.Current.LocalStorage;
            LocalDataFile = localStorage.Path;
            LocalDataFile += "/storage.db";
        }

        public static string GetValue(string key)
        {
            try
            {
                using (FileStream fileStream = new FileStream(LocalDataFile, FileMode.OpenOrCreate))
                {
                    if (fileStream.Length == 0)
                    {
                        return null;
                    }
                    XmlSerializer xmlFormatter = new XmlSerializer(typeof(SerializableDictionary<string, string>));
                    SerializableDictionary = (SerializableDictionary<string, string>)xmlFormatter.Deserialize(fileStream);
                }
                string ret = null;
                SerializableDictionary.TryGetValue(key, out ret);
                return ret;
            }
            catch (Exception ex)
            {
                RNTracer.Error(ReactConstants.Tag, "[RN_EXCEPTION]At ReactConfig GetValue:[" + ex.ToString() + "]");
                return null;
            }
        }

        public static void SetValue(string key, string value)
        {
            try
            {
                var keyExist = SerializableDictionary.ContainsKey(key);
                if (keyExist)
                {
                    SerializableDictionary[key] = value;
                }
                else
                {
                    SerializableDictionary.Add(key, value);
                }
                using (FileStream fileStream = new FileStream(LocalDataFile, FileMode.Create))
                {
                    XmlSerializer xmlFormatter = new XmlSerializer(typeof(SerializableDictionary<string, string>));
                    xmlFormatter.Serialize(fileStream, SerializableDictionary);
                }
            }
            catch (Exception ex)
            {
                RNTracer.Write(ReactConstants.Tag, "[RN_EXCEPTION]At ReactConfig GetValue:[" + ex.ToString() + "]");
            }
        }
    }
}

