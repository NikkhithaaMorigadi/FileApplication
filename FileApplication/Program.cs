using FileApplication;
public class Program
{
    public static void Main(string[] args)
    {
           FileApplication.Exception Exception = new FileApplication.Exception();
           {
                Exception.InValidFile();
                Exception.DatabaseExceptions();
                Exception.NetworkExceptions();
                Exception.InputvalidationExceptions();
                Exception.MultithreadingExceptions();

        }
    }
}