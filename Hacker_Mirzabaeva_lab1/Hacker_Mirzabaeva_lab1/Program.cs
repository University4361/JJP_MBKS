namespace Hacker_Mirzabaeva_lab1
{
    public class Program
    {
        static FileObserver fileObserver;
        
        static void Main(string[] args)
        {
            fileObserver = new FileObserver("C:\\Users\\Public\\MirzabaevaHacker");
        }
    }
}
