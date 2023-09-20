using System;
using System.IO;
using System.Xml;
using FlowerEndSummer.Common;
using UnityEngine;

namespace FlowerEndSummer.GameData
{
    /// <summary>
    /// 이펙트 클립 리스트와 이펙트 파일 이름과 경로를 가지고 있으며 파일을 읽고 쓰는 기능
    /// </summary>
    public class EffectData : BaseData
    {
        public EffectClip[] effectClips = default;

        public string ClipPath = @"Effects\";
        private string xmlFilePath = "";
        private string xmlFilename = "EffectData.xml";
        private string dataPath = @"Data./EffectData";

        private const string EFFECt = "effect"; // 저장 키
        private const string CLIP = "clip"; // 저장 키
        
        

        private EffectData()
        {}

        /// <summary>
        /// read, wirte, delete, clip get, copy....
        /// </summary>
        public void LoadData()
        {
            xmlFilePath = Application.dataPath + DataDirectory;
            TextAsset asset = ResourceManager.Load(this.dataPath) as TextAsset;

            if (asset is null || asset.text is null)
            {
                this.AddData("New Effect");
                return;
            }

            int currentID = 0;
            using (var reader = new XmlTextReader(new StringReader(asset.text)))
            {
                
                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                        switch (reader.Name)
                        {
                            case "Length":
                                int length = int.Parse(reader.ReadString());
                                names = new string[length];
                                effectClips = new EffectClip[length];
                                break;
                            case "Id":
                                currentID = int.Parse(reader.ReadString());
                                effectClips[currentID] = new EffectClip
                                {
                                    Id = currentID
                                };
                                break;
                            case "Name":
                                names[currentID] = reader.ReadString();
                                break;
                            case "EffectType":
                                effectClips[currentID].effectType = (EffectType)
                                    Enum.Parse(typeof(EffectType), reader.ReadString());
                                break;
                            case "EffectName":
                                effectClips[currentID].effectName = reader.ReadString();
                                break;
                            case "EffectPath":
                                effectClips[currentID].effectPath = reader.ReadString();
                                break;
                        }
                    }
                }
            }
        }


        public void SaveData()
        {
            using (XmlTextWriter xml = new XmlTextWriter(xmlFilePath + xmlFilename, System.Text.Encoding.Unicode))
            {
                xml.WriteStartDocument();
                xml.WriteStartElement(EFFECt);
                
                xml.WriteElementString("Length", GetDataCount().ToString());

                for (int i = 0; i < names.Length; i++)
                {
                    EffectClip clip = effectClips[i];
                    xml.WriteStartElement(CLIP);
                    xml.WriteElementString("Id", i.ToString());
                    xml.WriteElementString("Name", names[i]);
                    xml.WriteElementString("EffectType", clip.effectType.ToString());
                    xml.WriteElementString("EffectPath", clip.effectPath);
                    xml.WriteElementString("EffectName", clip.effectName);
                    xml.WriteEndElement();
                }
                
                xml.WriteEndElement();;
                xml.WriteEndDocument();
            }
            
        }

        public override int AddData(string newName)
        {
            if (names is null)
            {
                names = new string[] { name };
                effectClips = new EffectClip[] { new EffectClip() };
            }
            else
            {
                names = ArrayHelper.Add(name, names);
                effectClips = ArrayHelper.Add(new EffectClip(), effectClips);
            }

            return GetDataCount();
        }


        public override void RemoveData(int index)
        {
            names = ArrayHelper.Remove(index, this.names);
            if (names.Length is 0)
            {
                names = null;
            }

            effectClips = ArrayHelper.Remove(index, this.effectClips);
        }

        public EffectClip GetCopy(int index)
        {
            if (index < 0 || index >= effectClips.Length)
            {
                return null;
            }

            EffectClip original = effectClips[index];
            EffectClip clip = new EffectClip();
            clip = original;

            return clip;
        }

        public EffectClip GetClip(int index)
        {
            if (index < 0 || index >= effectClips.Length)
            {
                return null;
            }
            
            effectClips[index].PreLoad();
            return effectClips[index];
        }

        public override void Copy(int index)
        {
            names = ArrayHelper.Add(names[index], names);
            effectClips = ArrayHelper.Add(GetCopy(index), effectClips);
        }
    }
}