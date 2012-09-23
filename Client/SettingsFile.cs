using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace FTLOverdrive.Client
{
    public class SettingsFile
    {
        private struct Setting
        {
            public string Category;
            public string Key;
            public string Value;
        }

        private sealed class StringReader
        {
            private string strBuffer;
            private int iPos;
            public StringReader(string str)
            {
                strBuffer = str;
                iPos = 0;
            }

            public char Peek()
            {
                if (iPos >= strBuffer.Length) return '\0';
                return strBuffer[iPos];
            }

            public char Read()
            {
                if (iPos >= strBuffer.Length) return '\0';
                char c = strBuffer[iPos];
                iPos++;
                return c;
            }

            public string ReadSoFar()
            {
                return strBuffer.Substring(0, iPos);
            }

        }

        private List<Setting> lstSettings;
        private List<string> lstCats;
        private string strFilename;

        private bool bPopulate;

        private const string ptnCategoryStart = "[\\[]";
        private const string ptnCategoryEnd = "[\\]]";
        private const string ptnKey = "[a-zA-Z0-9:_]";
        private const string ptnValueIndicator = "[=]";
        private const string ptnValue = "[a-zA-Z0-9./ _]";
        private const string ptnValueStart = "[a-zA-Z0-9]";
        private const string ptnCommentStart = "[/]";
        private const string ptnNewLine = "[\\n]";

        public SettingsFile(string filename)
        {
            strFilename = filename;
            bPopulate = false;
            lstSettings = new List<Setting>();
            lstCats = new List<string>();
            if (!File.Exists(filename)) return;
            Load();
        }

        public void SetPopulate(bool b)
        {
            bPopulate = b;
        }

        public void Load()
        {
            // Clear existing data
            lstSettings.Clear();
            lstCats.Clear();

            // Check it exists
            if (!File.Exists(strFilename)) return;

            // Read the file
            byte[] data = File.ReadAllBytes(strFilename);
            string content = ASCIIEncoding.ASCII.GetString(data);
            content = content.Replace(Environment.NewLine, "\n");

            // Set the current category
            string curCat = "Default";

            // Create a reader
            StringReader rdr = new StringReader(content);
            string parsed = "";

            // Loop
            for (char c = rdr.Peek(); c != '\0'; c = rdr.Peek())
            {
                parsed += c;

                // See what the character represents
                if (Regex.IsMatch(c.ToString(), ptnCategoryStart))
                {
                    rdr.Read();
                    curCat = rdrReadCategory(rdr);
                    lstCats.Add(curCat);
                    continue;
                }
                if (Regex.IsMatch(c.ToString(), ptnKey))
                {
                    Setting s = rdrReadSetting(rdr);
                    if (s.Value != null)
                    {
                        s.Category = curCat;
                        lstSettings.Add(s);
                    }
                    continue;
                }
                if (Regex.IsMatch(c.ToString(), ptnCommentStart))
                {
                    rdrReadComment(rdr);
                    continue;
                }

                // Must be whitespace, ignore it
                rdr.Read();
            }
        }
        public void Save()
        {
            StringBuilder str = new StringBuilder();
            foreach (string cat in lstCats)
            {
                str.AppendLine("[" + cat + "]");
                foreach (Setting s in lstSettings)
                    if (s.Category == cat)
                        str.AppendLine(s.Key + " = " + s.Value);
                str.AppendLine("");
            }
            File.WriteAllBytes(strFilename, ASCIIEncoding.ASCII.GetBytes(str.ToString()));
        }

        private void rdrReadComment(StringReader rdr)
        {
            for (char c = rdr.Peek(); !Regex.IsMatch(c.ToString(), ptnNewLine); c = rdr.Peek())
            {
                if (c == '\0')
                {
                    throw new Exception("SettingsFile: rdrReadSetting: Unexpected eof when reading key-value pair");
                }
                rdr.Read();
            }
            rdr.Read();
        }
        private string rdrReadCategory(StringReader rdr)
        {
            string tmp = "";
            for (char c = rdr.Peek(); !Regex.IsMatch(c.ToString(), ptnCategoryEnd); c = rdr.Peek())
            {
                c = rdr.Read();
                if (c == '\0')
                {
                    throw new Exception("SettingsFile: rdrReadCategory: Unexpected eof when reading category");
                }
                tmp += c;
            }
            rdr.Read();
            return tmp;
        }
        private Setting rdrReadSetting(StringReader rdr)
        {
            char c;
            string key = "";
            for (c = rdr.Peek(); Regex.IsMatch(c.ToString(), ptnKey); c = rdr.Peek())
                key += rdr.Read();
            for (c = rdr.Peek(); !Regex.IsMatch(c.ToString(), ptnValueIndicator); c = rdr.Peek())
            {
                if (c == '\0')
                {
                    throw new Exception("SettingsFile: rdrReadSetting: Unexpected eof when reading key-value pair");
                }
                rdr.Read();
            }
            rdr.Read();
            for (c = rdr.Peek(); (!Regex.IsMatch(c.ToString(), ptnValueStart)) && (!Regex.IsMatch(c.ToString(), ptnCategoryStart)); c = rdr.Peek())
                rdr.Read();
            //rdr.Read();
            c = rdr.Peek();
            char n;
            if (Regex.IsMatch(c.ToString(), ptnCategoryStart))
            {
                rdr.Read();
                n = rdr.Peek();
                if (Regex.IsMatch(n.ToString(), ptnCategoryStart))
                {
                    rdr.Read();
                    string value = "";
                    for (c = rdr.Peek(); c != '\0'; c = rdr.Peek())
                    {
                        c = rdr.Read();
                        if (Regex.IsMatch(c.ToString(), ptnCategoryEnd))
                        {
                            n = rdr.Peek();
                            if (Regex.IsMatch(n.ToString(), ptnCategoryEnd))
                            {
                                break;
                            }
                        }
                        value += c;
                    }
                    Setting s = new Setting();
                    s.Key = key;
                    s.Value = value;
                    return s;
                }
            }
            string val = "";
            for (c = rdr.Peek(); Regex.IsMatch(c.ToString(), ptnValue); c = rdr.Peek())
                val += rdr.Read();
            Setting s2 = new Setting();
            s2.Key = key;
            s2.Value = val;
            return s2;
        }

        public string ReadString(string category, string key, string def)
        {
            foreach (Setting s in lstSettings)
                if ((s.Key.ToLower() == key.ToLower()) && (s.Category.ToLower() == category.ToLower()))
                    return s.Value;
            if (bPopulate) WriteString(category, key, def);
            return def;
        }
        public string ReadString(string category, string key)
        {
            return ReadString(category, key, "");
        }
        public void WriteString(string category, string key, string value)
        {
            Setting s;
            for (int i = 0; i < lstSettings.Count; i++)
            {
                s = lstSettings[i];
                if ((s.Key.ToLower() == key.ToLower()) && (s.Category.ToLower() == category.ToLower()))
                {
                    s.Value = value;
                    lstSettings[i] = s;
                    return;
                }
            }
            s = new Setting();
            s.Category = category;
            s.Key = key;
            s.Value = value;
            lstSettings.Add(s);
            if (!lstCats.Contains(category)) lstCats.Add(category);
        }

        public List<string> ReadKeys(string category)
        {
            var result = new List<string>();
            foreach (Setting s in lstSettings)
                if (s.Category.ToLower() == category.ToLower())
                    result.Add(s.Key);
            return result;
        }

        public int ReadInt(string category, string key, int def)
        {
            int result;
            if (!int.TryParse(ReadString(category, key, def.ToString()), out result)) return def;
            return result;
        }
        public int ReadInt(string category, string key)
        {
            return ReadInt(category, key, 0);
        }
        public void WriteInt(string category, string key, int value)
        {
            WriteString(category, key, value.ToString());
        }

        public float ReadFloat(string category, string key, float def)
        {
            float result;
            if (!float.TryParse(ReadString(category, key, def.ToString()), out result)) return def;
            return result;
        }
        public float ReadFloat(string category, string key)
        {
            return ReadFloat(category, key, 0f);
        }
        public void WriteFloat(string category, string key, float value)
        {
            WriteString(category, key, value.ToString());
        }

        public void RemoveSetting(string category, string key)
        {
            foreach (Setting s in lstSettings)
                if ((s.Key.ToLower() == key.ToLower()) && (s.Category.ToLower() == category.ToLower()))
                {
                    lstSettings.Remove(s);
                    return;
                }
        }

    }
}