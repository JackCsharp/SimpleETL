using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;
using System.Data.SqlClient;
using CsvHelper.TypeConversion;
using System.Data;
using Microsoft.Extensions.Configuration;
using SimpleETF.Model;
using SimpleETF.Common;

namespace SimpleEtl
{
    class Program
    {
        
        static void Main(string[] args)
        {
                var builder = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                IConfiguration configuration = builder.Build();
                string connectionString = configuration.GetConnectionString("DefaultConnection");

            string csvFilePath;
            if (args.Length != 1)
            {
                Console.WriteLine("csvFilePath was not found in args, so you need to write it here");
                do
                {
                    csvFilePath = Console.ReadLine();
                    if (!File.Exists(csvFilePath))
                    {
                        Console.WriteLine($"Error: File '{csvFilePath}' not found.");
                        Console.WriteLine("Try again.");
                    }
                    else
                    {
                        break;
                    }
                }
                while (true);
            }
            else
            {
                csvFilePath = args[0];
            }

            if (!File.Exists(csvFilePath))
            {
                Console.WriteLine($"Error: File '{csvFilePath}' not found.");
                return;
            }

            try
            {
                List<DataRecord> records = CsvOperations.ReadCsv(csvFilePath);
                SqlOperations.ProcessRecords(records, connectionString);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            Console.WriteLine("Do you want to run test queries? y - yes ");
            var userResponse = Console.ReadKey();
            if (userResponse.Key == ConsoleKey.Y)
                UserQueries.ExecuteQueries(connectionString);
            else
                Console.WriteLine("The program has stopped working");
        }

    }

}