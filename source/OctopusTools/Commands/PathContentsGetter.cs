using System;
using System.Collections.Generic;
using System.IO;
namespace OctopusTools.Commands
{
    public interface IPathContentsGetter
    {
        List<string> GetPathContents(string path);
    }
    public class PathContentsGetter : IPathContentsGetter
    {
        public List<string> GetPathContents(string path)
        {
            var startingPoint = new DirectoryInfo(path);
            var listOfFiles = new List<string>();
            CrawlRecursively(startingPoint, listOfFiles);
            return listOfFiles;
        }

        public void CrawlRecursively(DirectoryInfo source, List<string> listOfFiles)
        {
            foreach (FileInfo file in source.GetFiles())
            {
                listOfFiles.Add(file.FullName);
            }
            foreach (DirectoryInfo dir in source.GetDirectories())
            {
                listOfFiles.Add(dir.FullName + Path.DirectorySeparatorChar);
                CrawlRecursively(dir, listOfFiles);
            }
        }
    }

}
