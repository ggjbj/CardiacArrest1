using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;

namespace CardiacArrest
{
    [Serializable]
    class PlayerData : ISerializable
    {
        List<string> CluesCollected;
        List<string> CasesCollected;
        protected int Health;
        float TimePlayed;
        int xPos;
        int yPos;

        public PlayerData(int _Health, float _TimePlayed, int _xPos, int _yPos)
        {
            Health = _Health;
            TimePlayed = _TimePlayed;
            CluesCollected = new List<string>();
            CasesCollected = new List<string>();
            xPos = _xPos;
            yPos = _yPos;
        }

        public PlayerData(SerializationInfo info, StreamingContext context)
        {
            CluesCollected = (List<string>)info.GetValue("Clues", typeof(List<string>));
            CasesCollected = (List<string>)info.GetValue("Cases", typeof(List<string>));
            Health = (int)info.GetValue("Health", typeof(int));
            TimePlayed = (float)info.GetValue("TimePlayed", typeof(float));
            xPos = (int)info.GetValue("xPos", typeof(int));
            yPos = (int)info.GetValue("yPos", typeof(int));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Clues", CluesCollected);
            info.AddValue("Cases", CasesCollected);
            info.AddValue("Health", Health);
            info.AddValue("TimePlayed", TimePlayed);
            info.AddValue("xPos", xPos);
            info.AddValue("yPos", yPos);
        }
        public void AddClue(string key)
        {
            CluesCollected.Add(key);
        }
        public void AddCase(string key)
        {
            CasesCollected.Add(key);
        }
        public void SetHealth(int _health)
        {
            Health = _health;
        }
        public void SetTimePlayed(float _time)
        {
            TimePlayed = _time;
        }
        public int GetHealth()
        {
            return Health;
        }
        public float GetTimePlayed()
        {
            return TimePlayed;
        }
        public int GetX()
        {
            return xPos;
        }
        public int GetY()
        {
            return yPos;
        }
        public void SetX(int x)
        {
            xPos = x;
        }
        public void SetY(int y)
        {
            yPos = y;
        }
        public int GetCaseCount()
        {
            return CasesCollected.Count;
        }
        public List<string> GetClues()
        {
            return CluesCollected;
        }
        public bool Save()
        {
            if (!Directory.Exists("ApplicationData"))
            {// if we don't yet have application data, create the appropriate folder
                Directory.CreateDirectory("ApplicationData");
            }
            else
            {
                using (Stream stream = File.Open(Environment.SpecialFolder.ApplicationData + "/PlayerData.txt", FileMode.OpenOrCreate))
                {
                    try
                    {
                        BinaryFormatter bFormatter = new BinaryFormatter();
                        bFormatter.Serialize(stream, this);
                        return true;
                    }
                    catch (Exception e)
                    {
                        throw (e);
                    }
                }
            }
            return false;

        }
        public static PlayerData Load()
        {
            if (!Directory.Exists("ApplicationData"))
            {// if we don't yet have application data, create the appropriate folder
                Directory.CreateDirectory("ApplicationData");
            }
            else
            {
                if (File.Exists(Environment.SpecialFolder.ApplicationData + "/PlayerData.txt"))
                {
                    using (Stream stream = File.Open(Environment.SpecialFolder.ApplicationData + "/PlayerData.txt", FileMode.Open))
                    {
                        try
                        {
                            PlayerData objectToSerialize;
                            BinaryFormatter bFormatter = new BinaryFormatter();
                            objectToSerialize = (PlayerData)bFormatter.Deserialize(stream);
                            return objectToSerialize;
                        }
                        catch (Exception e)
                        {
                            throw (e);
                        }
                    }
                }
                else
                {
                    return null;
                }

            }
            return null;
        }
    }
}
