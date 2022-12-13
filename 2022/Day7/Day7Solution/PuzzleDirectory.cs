using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day6Solution
{
    public class PuzzleDirectory
    {
        public string DirectoryName;
        public List<PuzzleFile> Files;
        public List<PuzzleDirectory> Subdirectories;
        public PuzzleDirectory? parentDirectory;

        public long DirectorySizeBytes 
        {
            get => 
                Files.Select(f => f.FileSizeBytes).Sum()
                +
                Subdirectories.Select(dir => dir.DirectorySizeBytes).Sum();
        }

        public PuzzleDirectory() : this(string.Empty, null) { }

        public PuzzleDirectory(string DirectoryName, PuzzleDirectory? parentDirectory)
        {
            this.parentDirectory = parentDirectory;
            this.DirectoryName = DirectoryName;
            Files = new();
            Subdirectories = new();
        }

    }
}
