using UnityEngine;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.IO.IsolatedStorage;

namespace StatMaster
{
    class FilesHandler
    {
        private string path = Application.persistentDataPath + "/statMaster/";
        private string subFolder = "";
        private string ext = ".json";
        public Dictionary<string, string> files = new Dictionary<string, string>();
        public Dictionary<string, string> contents = new Dictionary<string, string>();

        public bool errorOnLoad = false;
        public bool errorOnSave = false;

        public bool invalidPath = false;
        public string invalidPathMessage = "";

        public FilesHandler()
        {
            try
            {
                Directory.CreateDirectory(path);
            }
            catch (IOException e)
            {
                invalidPath = true;
                invalidPathMessage = e.Message;
            }
        }

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

        public void setSubFolder(string newSubFolder)
        {
            subFolder = newSubFolder + "/";

            string[] subFolderParts = subFolder.Split('/');
            string currentSubFolder = "";
            for (var i = 0; i < subFolderParts.Length; i++)
            {
                if (subFolderParts[i].Length > 0)
                {
                    currentSubFolder += subFolderParts[i] + "/";
                    try
                    {
                        Directory.CreateDirectory(path + currentSubFolder);
                    }
                    catch (IOException e)
                    {
                        invalidPath = true;
                        invalidPathMessage = e.Message;
                        return;
                    }
                }
            }
            
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
            if (Directory.Exists(path))
                Directory.Delete(path, true);
        }

        public void backupAll(uint dataVersion)
        {
            string destPath = path + "backup_dv_" + dataVersion + "/";
            if (!Directory.Exists(destPath))
            {
                Directory.CreateDirectory(destPath);
            }
            DirectoryCopy(path, destPath, true);
        }

        private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = System.IO.Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, false);
            }

            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = System.IO.Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }

        public void delete(string handle)
        {
            if (files.ContainsKey(handle) && File.Exists(path + files[handle]))
                File.Delete(path + files[handle]);
        }

        public List<string> loadAll()
        {
            errorOnLoad = false;
            List<string> messages = new List<string>();
            foreach (string handle in files.Keys)
            {
                try
                {
                    if (File.Exists(path + subFolder + files[handle]))
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
                            messages.Add("Loaded " + subFolder + " " + handle + " data. " + contents[handle]);
                        }
                        catch (System.Exception e)
                        {
                            errorOnLoad = true;
                            messages.Add("Failed to load " + subFolder + " " + handle + " data. Error: " + e.Message);
                        }
                        finally
                        {
                            file.Close();
                        }
                    }
                }
                catch (IsolatedStorageException e)
                {
                    errorOnLoad = true;
                    messages.Add("Failed to get file from path / sub folder " + subFolder + " " + handle + " data. Error: " + e.Message);
                }
                catch (IOException e)
                {
                    errorOnLoad = true;
                    messages.Add("Failed to handle file for " + subFolder + " " + handle + " data. Error: " + e.Message);
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
                    FileStream file = File.Create(path + subFolder + files[handle]);
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
                            messages.Add("Saved " + subFolder + " " + handle + " data.");
                        }
                        catch
                        {
                            throw new System.Exception("Cannot save file with stream writer");
                        }                  
                    }
                    catch (System.Exception e)
                    {
                        errorOnSave = true;
                        messages.Add("Failed to save " + subFolder + " " + handle + " data. Error: " + e.Message);
                    }
                    finally
                    {
                        file.Close();
                    }
                }
                catch (IsolatedStorageException e)
                {
                    errorOnLoad = true;
                    messages.Add("Failed to set file to path / sub folder " + subFolder + " " + handle + " data. Error: " + e.Message);
                }
                catch (IOException e)
                {
                    errorOnSave = true;
                    messages.Add("Failed to handle file for " + subFolder + " " + handle + " data. Error: " + e.Message);
                }
            }
            return messages;
        }
    }
}
