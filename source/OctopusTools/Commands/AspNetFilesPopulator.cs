using NuGet;
using System.Collections.Generic;
using System.IO;

namespace OctopusTools.Commands
{
    public interface IAspNetFilesPopulator
    {
        void PopulatePackage(PackageBuilder package, string basePath);
    }
    public class AspNetFilesPopulator : IAspNetFilesPopulator
    {
        readonly IPathContentsGetter pathContentsGetter;
        public AspNetFilesPopulator(IPathContentsGetter pathCrawler)
        {
            this.pathContentsGetter = pathCrawler;
        }

        public void PopulatePackage(PackageBuilder package, string basePath)
        {
            if (basePath.StartsWith(".\\"))
            {
                basePath = basePath.Substring(2);
            }

            var fullBasePath = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + basePath;
            foreach (var filePath in pathContentsGetter.GetPathContents(basePath))
            {
                var lowerCasedFilePath = filePath.ToLowerInvariant();
                if (lowerCasedFilePath.EndsWith(Path.DirectorySeparatorChar.ToString())) continue; // ignore folders
                if (lowerCasedFilePath.EndsWith(".cs")) continue; // ignore source files
                if (lowerCasedFilePath.EndsWith(".vb")) continue; // ignore source files
                if (lowerCasedFilePath.EndsWith(".pdb")) continue; // ignore debug files
                if (lowerCasedFilePath.EndsWith(".xml")) continue; // ignore debug files
                if (lowerCasedFilePath.Contains(".csproj")) continue; // ignore project files
                if (lowerCasedFilePath.Contains(".vbproj")) continue; // ignore project files
                if (lowerCasedFilePath.Contains(Path.DirectorySeparatorChar + "obj" + Path.DirectorySeparatorChar)) continue; // ignore build files
                if (lowerCasedFilePath.Contains("packages.config")) continue; // ignore nuget packages.config file

                var targetFolder = string.Empty;

                var directory = Path.GetDirectoryName(filePath);
                var offset = directory.IndexOf(fullBasePath) + fullBasePath.Length + 1;
                if (offset > 0 && offset <= directory.Length)
                {
                    targetFolder = directory.Substring(offset);
                }

                package.PopulateFiles(basePath, new List<ManifestFile> { new ManifestFile { Source = filePath, Target = targetFolder } });
            }
        }
    }
}
