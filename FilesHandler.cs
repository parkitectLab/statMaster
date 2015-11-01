using UnityEngine;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.IO;

namespace StatMaster
{
    class FilesHandler
    {
        private string path = Application.persistentDataPath + "/statMaster/";
        private string ext = ".json";
        public Dictionary<string, string> files = new Dictionary<string, string>();
        public Dictionary<string, string> contents = new Dictionary<string, string>();

        public bool errorOnLoad = false;
        public bool errorOnSave = false;

        public void add(string handle)
        {
            files.Add(handle, handle + ext);
        }

        public bool set(string handle, string content)
        {
            contents[handle] = content;
            return true; // todo add handle exists check
        }

        public string get(string handle)
        {
            return contents.ContainsKey(handle) ? contents[handle] : null;
        }

        public string calculateMD5Hash(string input)
        {
            MD5 md5 = MD5.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }

        public void deleteAll()
        {
            Directory.Delete(path, true);
        }

        public List<string> loadAll()
        {
            errorOnLoad = false;
            Debug.Log("load all ");
            List<string> messages = new List<string>();
            foreach (string handle in files.Keys)
            {
                UnityEngine.Debug.Log("try file by handle " + handle);
                try
                {
                    UnityEngine.Debug.Log("try to open/load file " + path + files[handle]);
                    if (File.Exists(path + files[handle]))
                    {
                        FileStream file = File.Open(path + files[handle], FileMode.Open);
                        try
                        {
                            try
                            {
                                StreamReader sr = new StreamReader(file);
                                contents[handle] = sr.ReadToEnd();
                            } catch
                            {
                                throw new System.Exception("Cannot read file with stream reader");
                            }
                            if (contents[handle] == "")
                            {
                                throw new System.Exception("Content cannot be empty");
                            }
                            messages.Add("Loaded " + handle + " data. " + contents[handle]);
                        }
                        catch (System.Exception e)
                        {
                            errorOnLoad = true;
                            messages.Add("Failed to load " + handle + " data. Error: " + e.Message);
                        }
                        finally
                        {
                            file.Close();
                        }
                    }
                }
                catch (IOException e)
                {
                    errorOnLoad = true;
                    messages.Add("Failed to handle file for " + handle + " data. Error: " + e.Message);
                }
            }
            return messages;
        }

        public List<string> saveAll()
        {
            errorOnLoad = false;
            List<string> messages = new List<string>();
            foreach (string handle in files.Keys)
            {
                try
                {
                    Directory.CreateDirectory(path);
                    FileStream file = File.Create(path + files[handle]);
                    try
                    {
                        if (contents[handle] == "")
                        {
                            throw new System.Exception("Content cannot be empty");
                        }
                        try
                        {
                            StreamWriter sw = new StreamWriter(file);
                            sw.Write(contents[handle]);
                            sw.Flush();
                            messages.Add("Saved " + handle + " data.");
                        }
                        catch
                        {
                            throw new System.Exception("Cannot save file with stream writer");
                        }                  
                    }
                    catch (System.Exception e)
                    {
                        errorOnSave = true;
                        messages.Add("Failed to save " + handle + " data. Error: " + e.Message);
                    }
                    finally
                    {
                        file.Close();
                    }
                }
                catch (IOException e)
                {
                    errorOnSave = true;
                    messages.Add("Failed to handle file for " + handle + " data. Error: " + e.Message);
                }
            }
            return messages;
        }
    }
}
