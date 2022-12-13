using Day6Solution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day6
{
    public class PuzzleDirectoryMaker
    {
        private readonly PuzzleDirectory baseDirectory;
        public PuzzleDirectory currentDirectory;

        public PuzzleDirectoryMaker()
        {
            currentDirectory = new PuzzleDirectory(@"/",null);
            baseDirectory = currentDirectory;
        }

        public void ChangeDirectory(string argument)
        {
            string directoryName = argument[5..];

            if (directoryName.Equals(".."))
            {
                currentDirectory = currentDirectory.parentDirectory;
                return;
            }

            if (directoryName.Equals(@"/"))
            {
                currentDirectory = baseDirectory;
                return;
            }

            currentDirectory = 
                currentDirectory
                .Subdirectories
                .Where(dir => dir.DirectoryName.Equals(directoryName)) // Expecting "$ cd <directory name>"
                .First();

        }

        public void ListCommand(List<string> arguments)
        {
            foreach(string argument in arguments)
            {
                if (argument.StartsWith('$')) // Skip first line
                {
                    continue;
                }

                if (argument.StartsWith("dir"))
                {
                    currentDirectory.Subdirectories.Add(new PuzzleDirectory(argument[4..], currentDirectory));
                    continue;
                }

                string[] argumentParts = argument.Split(' ',2);
                _ = long.TryParse(argumentParts[0], out long fileSize);
                string fileName = argumentParts[1];

                currentDirectory.Files.Add(new PuzzleFile(fileName, fileSize));
            }
        }

        public List<PuzzleDirectory> GetAllDirectories()
        {
            return GetAllDirectories(baseDirectory);
        }
        private static List<PuzzleDirectory> GetAllDirectories(PuzzleDirectory puzzleDirectory)
        {
            List<PuzzleDirectory> directories = new();
            directories.Add(puzzleDirectory);
            foreach(PuzzleDirectory subdirectory in puzzleDirectory.Subdirectories)
            {
                directories.AddRange(GetAllDirectories(subdirectory));
            }
            return directories;
        }

    }
}
