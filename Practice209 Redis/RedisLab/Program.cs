using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisLab
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Do you want to read by a key(R), write(W), search by keys(S), delete(D) or exit(E)?");
                string line = Console.ReadLine();

                if(line.ToUpper() == "R")
                {
                    Console.WriteLine("Please enter a key");
                    string key = Console.ReadLine();
                    Console.WriteLine(ReadData("localhost", key));
                }

                else if (line.ToUpper() == "W")
                {
                    Console.WriteLine("Please enter a key");
                    string key = Console.ReadLine();
                    Console.WriteLine("Please enter a value");
                    string value = Console.ReadLine();

                    if (SaveData("localhost", key, value))
                        Console.WriteLine("Write successful");
                    else
                        Console.WriteLine("Error: key already exists");
                }

                else if (line.ToUpper() == "S")
                {
                    List<string> keys = new List<string>();

                    while (true)
                    {
                        Console.WriteLine("Enter a key. To continue enter C");
                        string input = Console.ReadLine();
                        if (input.ToUpper() == "C")
                            break;
                        else
                            keys.Add(input);
                    }

                    foreach (string key in keys)
                    {
                        Console.WriteLine(key + " " + ReadData("localhost", key));
                    }
                }

                else if (line.ToUpper() == "D")
                {
                    Console.WriteLine("Please enter a key");
                    string key = Console.ReadLine();
                    if (DeleteData("localhost", key))
                        Console.WriteLine("Deletion successful");
                    else
                        Console.WriteLine("Error: key not found");
                }

                else if (line.ToUpper() == "E")
                    break;
            }
        }

        private static bool SaveData(string host, string key, string value)
        {
            using (RedisClient client = new RedisClient(host))
            {
                if (client.Get<string>(key) == null)
                    client.Set(key, value, DateTime.Now.AddMinutes(1));
                else
                    return false;
                return true;
            }
        }

        private static string ReadData(string host, string key)
        {
            using (RedisClient client = new RedisClient(host))
            {
                if (client.Get<string>(key) == null)
                    return "Key not found";
                else
                    return client.Get<string>(key);
            }
        }

        private static bool DeleteData(string host, string key)
        {
            using (RedisClient client = new RedisClient(host))
            {
                if (client.Get<string>(key) == null)
                    return false;
                else
                    client.Remove(key);
                return true;
            }
        }
    }
}
