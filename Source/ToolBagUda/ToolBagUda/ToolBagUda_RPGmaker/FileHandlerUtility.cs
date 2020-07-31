using System;
using System.Reflection;
using HarmonyLib;
using UnityEngine;
using Verse;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using RimWorld;

namespace ToolBagUda
{
    [StaticConstructorOnStartup]
    class FileHandlerUtility
    {
        static FileHandlerUtility()
        {

        }
        public string getReconSteamPath()
        {
            string path = "";

            char sp = Path.DirectorySeparatorChar;
            string pather = GenFilePaths.ModsFolderPath; Log.Message("Mods:  " + pather);
            string foot = "workshop" + sp + "content" + sp + "294100" + sp + "2180058143" + sp + "Defs" + sp + "Misc" + sp + "TrekData" + sp;
            string[] patherParts = pather.Split(Path.DirectorySeparatorChar);
            string cap = patherParts[0];
            cap = cap + '/';
            patherParts[0] = cap;

            int numParts = patherParts.Count() - 1;
            Array.Resize(ref patherParts, numParts - 2);
            path = Path.Combine(Path.Combine(patherParts), foot);

           
            return path;

        }
        public string getReconSteamPath(string fileName)
        {
            string path = "";

            char sp = Path.DirectorySeparatorChar;
            string pather = GenFilePaths.ModsFolderPath; Log.Message("Mods:  " + pather);
            string foot = "workshop" + sp + "content" + sp + "294100" + sp + "2180058143" + sp + "Defs" + sp + "Misc" + sp + "TrekData" + sp;
            string[] patherParts = pather.Split(Path.DirectorySeparatorChar);
            string cap = patherParts[0];
            cap = cap + '/';
            patherParts[0] = cap;

            int numParts = patherParts.Count() - 1;
            Array.Resize(ref patherParts, numParts - 2);
            path = Path.Combine(Path.Combine(patherParts), foot + fileName + ".rwship");

            
            return path;

        }
        public string getReconSteamPath(string fileName, string fileExt)
        {
            string path = "";

            char sp = Path.DirectorySeparatorChar;
            string pather = GenFilePaths.ModsFolderPath; Log.Message("Mods:  " + pather);
            string foot = "workshop" + sp + "content" + sp + "294100" + sp + "2180058143" + sp + "Defs" + sp + "Misc" + sp + "TrekData" + sp;
            string[] patherParts = pather.Split(Path.DirectorySeparatorChar);
            string cap = patherParts[0];
            cap = cap + '/';
            patherParts[0] = cap;

            int numParts = patherParts.Count() - 1;
            Array.Resize(ref patherParts, numParts - 2);
            path = Path.Combine(Path.Combine(patherParts), foot + fileName + fileExt);


            return path;

        }
        public bool loadPawnsFromFile(string filePath, bool destruct, out List<Pawn> listOut)
        {
            List<Pawn> loadedPawns = new List<Pawn>();
            try
            {
                Scribe.loader.InitLoading(filePath);
                Scribe_Collections.Look<Pawn>(ref loadedPawns, "pawns", LookMode.Deep, Array.Empty<object>());
                Scribe.loader.FinalizeLoading();
                if (destruct) File.Delete(filePath);
            }
            catch
            {
                listOut = null;
                return false;
            }

            listOut = loadedPawns;
            return true;

        }
        public bool savePawnsToFile(string filePath,List<Pawn> pawnToSave)
        {
            try
            {
                SafeSaver.Save(filePath, "RPG_Pawn", delegate
                {
                    Scribe_Collections.Look<Pawn>(ref pawnToSave, "pawns", LookMode.Deep, Array.Empty<object>());
                 

                }, false);
            }
            catch
            {
                return false;
            }

            return true;

        }
        public bool loadPawnFromFile(string filePath,bool destruct, out Pawn pawnOut)
        {
            Pawn loadedPawn = new Pawn();
            try
            {
                Scribe.loader.InitLoading(filePath);
                Scribe_Deep.Look<Pawn>(ref loadedPawn, "pawns", LookMode.Deep, Array.Empty<object>());
                Scribe.loader.FinalizeLoading();
                if (destruct) File.Delete(filePath);
            }
            catch
            {
                pawnOut = null;
                return false;
            }

            pawnOut = loadedPawn;
            return true;

        }
        public bool savePawnToFile(string filePath, Pawn pawnToSave)
        {
            try
            {
                SafeSaver.Save(filePath, "RPG_Pawn", delegate
                {
                    Scribe_Deep.Look<Pawn>(ref pawnToSave, "pawns", LookMode.Deep, Array.Empty<object>());
                }, false);
            }
            catch
            {
                return false;
            }

            return true;

        }
        public bool scanForDir(string path, out List<string> subDir, out int count)
        {
            subDir = Directory.GetDirectories(path).ToList<string>();
            count = subDir.Count();
            if (count == 0) return false;
            return true;

        }

        public bool scanForFiles(string path, out List<string> files, out int count)
        {
            files = Directory.GetFiles(path).ToList<string>();
            count = files.Count();
            if (count == 0) return false;
            return true;
        }

        public string createMirror(string oldPath)
        {

            List<string> oldpat = oldPath.Split(Path.DirectorySeparatorChar).ToList<string>();
            oldpat[0] = oldpat[0] + '/';
            oldpat.Remove("Something");// oldpat.RemoveLast<string>();
            string newPath = Path.Combine(oldpat.ToArray());
           // Log.Message("Mirror made: " + oldPath +" to:  " + newPath);
            return newPath;
        }
        public void copyToMirror(string pathToFile)
        {
          //  Log.Message("copying file to mirror found at: " + pathToFile);
            this.checkPath(pathToFile);
            File.Copy(pathToFile, this.createMirror(pathToFile) , true);
        }

        public void copyFilesToMirror(List<string> files)
        {
            foreach(string file in files)
            {
                copyToMirror(file);
            }
        }

        public void copyDirToMirror(string pathToDir)
        {
            List<string> subDir = new List<string>();
            int count = 0;
           // Log.Message("scanning for folders in:  " + pathToDir);
            if (this.scanForDir(pathToDir,out subDir,out count))
                {
                Log.Message(count.ToString() + " folders found in: " + pathToDir);
                    foreach (string Dir in subDir)
                {
                    if (filterHiddens(Dir))
                    {
                        copyDirToMirror(Dir);
                    }
                    
                }
                }
            List<string> filePaths = new List<string>();
            int count2 = 0;
           // Log.Message("scanning for files in:  " + pathToDir);
            if (this.scanForFiles(pathToDir, out filePaths, out count2))
            {
                Log.Message(count2.ToString() + " files found in: " + pathToDir);
                foreach (string path in filePaths)
                {
                    copyToMirror(path);
                }
            }
            
        }
        public bool filterHiddens(string path)
        {
            List<string> parts = path.Split(Path.DirectorySeparatorChar).ToList<string>();
            if (parts.Contains(".git")) return false;
            return true;
        }
        public void checkPath(string path)
        {
            List<string> parts = path.Split(Path.DirectorySeparatorChar).ToList<string>();
             parts.RemoveLast<string>();
            string dir = Path.Combine(parts.ToArray());// Log.Message("checking dir:  " + dir);
            if (Directory.Exists(dir)) return;
            Directory.CreateDirectory(dir);// Log.Warning("making dir:  " + dir);
        }

    }
}
