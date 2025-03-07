using System.Collections.Generic;

namespace Entities.Data
{
    public class EntityPath
    {
        public bool IsTargetPathValid => TargetPath is { Count: > 0 };
        
        public int PathLenght => TargetPath is { Count: > 0 } ? TargetPath.Count : 0;

        public List<PathRotationGroup> TargetPath { get; private set; }

        public void SetTargetPath(List<PathRotationGroup> targetPath) => TargetPath = targetPath;
    }
}