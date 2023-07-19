using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FileApplication
{
    public class Exception
    {
        public string userinput;
        public static string connectionString = "Server=192.168.0.30,1433;Initial Catalog=NikhithaEmp;Persist Security Info=False;User ID=User5;Password=CDev005#;MultipleActiveResultSets=False;Encrypt=False;TrustServerCertificate=False;";
        public void InValidFile()
        {
            string fpath = @"D:\test1.png";
            string type = ".txt";
            try
            {
                if (File.Exists(fpath))
                {
                    Console.WriteLine("File found");
                    if (Path.GetExtension(fpath) == type)
                    {
                        string data = File.ReadAllText(fpath);
                        Console.WriteLine(data);
                    }
                    else
                    {
                        throw new InvalidDataException("invalid file format");
                    }
                }
                else
                {
                    throw new FileNotFoundException("File not found", fpath);
                }
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
            catch (InvalidDataException ex)
            {
                Console.WriteLine("An Exception occurred in InValidFile method: " + ex.Message);
            }
        }
        public void DatabaseExceptions()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sqlQuery = "SELECT * FROM MSreplication_options";
                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            // Process each row
                            // ...
                        }

                        reader.Close();
                    }
                    connection.Close();
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("A SQL exception occurred: " + ex.Message);
                FallBackException();
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("An exception occurred: " + ex);
            }
        }
        public static void FallBackException()
        {
            Console.WriteLine("Check the Connection string! " + connectionString);
        }
        public void NetworkExceptions()
        {
            try
            {
                WebClient client = new WebClient();
                string response = client.DownloadString("https://api.example.com/data");
            }
            catch (WebException ex)
            {
                Console.WriteLine("A web exception occurred: " + ex.Message);
                if (ex.Status == WebExceptionStatus.Timeout)
                {
                    RetryOrPerformFallback();
                }

            }
            catch (System.Exception ex)
            {
                Console.WriteLine("An exception occurred: " + ex);
            }
            void RetryOrPerformFallback()
            {
                const int MaxRetries = 3;
                const int InitialDelayMs = 1000; // 1 second

                bool success = false;
                int retries = 0;
                int delayMs = InitialDelayMs;

                while (!success && retries < MaxRetries)
                {
                    try
                    {
                        WebClient client = new WebClient();
                        string response = client.DownloadString("https://api.example.com/data");
                        success = true; // Operation succeeded
                    }
                    catch (WebException ex)
                    {
                        Console.WriteLine("A web exception occurred: " + ex.Message);
                        if (IsRetryableException(ex))
                        {
                            retries++;
                            Console.WriteLine($"Retry attempt: {retries}");
                            delayMs *= 2;
                            System.Threading.Thread.Sleep(delayMs);
                        }

                    }
                    catch (System.Exception ex)
                    {
                        Console.WriteLine("An exception occurred: " + ex.Message);
                    }
                }
                if (!success)
                {
                    Console.WriteLine("Operation failed after maximum retries.");
                }

                bool IsRetryableException(WebException ex)
                {
                    return ex.Status == WebExceptionStatus.Timeout || ex.Status == WebExceptionStatus.ConnectionClosed ||
                        ex.Status == WebExceptionStatus.NameResolutionFailure || ex.Status == WebExceptionStatus.ProtocolError;
                }
            }
        }
        public void InputvalidationExceptions()
        {
            try
            {
                Console.WriteLine("Enter a numeric value:");
                userinput = Console.ReadLine();
                double numericValue = double.Parse(userinput);

            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid input! Please enter a valid numeric value. " + userinput);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("An exception occurred: " + ex.Message);
            }
        }
        public void MultithreadingExceptions()
        {
            WorkThread workThread = new WorkThread();
            Thread WorkerThread = new Thread(workThread.WorkerThread);
            WorkerThread.Start();
            for (int i = 0; i < 3; i++)
            {
                Console.WriteLine("Main thread: Performing operation " + i);
                Thread.Sleep(1500); 
            }
            WorkerThread.Join();
            Console.WriteLine("Main thread: Worker thread completed. Exiting the application.");
        }
    }
}
