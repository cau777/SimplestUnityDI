namespace Tests
{
    public class ClassDependencyLine1
    {
        
    }
    
    public class ClassDependencyLine2
    {
        public readonly ClassDependencyLine1 Line;

        public ClassDependencyLine2(ClassDependencyLine1 line)
        {
            Line = line;
        }
    }
    
    public class ClassDependencyLine3
    {
        public readonly ClassDependencyLine2 Line;

        public ClassDependencyLine3(ClassDependencyLine2 line)
        {
            Line = line;
        }
    }
}