using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using CsvHelper;
using SimpleETF.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleETF.Common
{
    static internal class CsvOperations
    {
        static internal List<DataRecord> ReadCsv(string filePath)
        {
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
            {
                var records = new List<DataRecord>();
                var duplicates = new List<DataRecord>();
                var seenRecords = new HashSet<DataRecord>();

                while (csv.Read())
                {
                    try
                    {
                        var record = csv.GetRecord<DataRecord>();

                        record.store_and_fwd_flag = record.store_and_fwd_flag?.Trim();
                        if (record.store_and_fwd_flag == "N")
                        {
                            record.store_and_fwd_flag = "No";
                        }
                        else if (record.store_and_fwd_flag == "Y")
                        {
                            record.store_and_fwd_flag = "Yes";
                        }

                        if (!seenRecords.Add(record))
                        {
                            duplicates.Add(record);
                            Console.WriteLine($"Duplicate record found and removed: {record.tpep_pickup_datetime}, {record.tpep_dropoff_datetime}, {record.passenger_count}");
                        }
                        else
                        {
                            records.Add(record);
                        }
                    }
                    catch (CsvHelperException ex)
                    {
                        if (ex.InnerException is TypeConverterException innerEx &&
                            innerEx.MemberMapData?.Member.Name == "passenger_count" &&
                            string.IsNullOrEmpty(csv.GetField("passenger_count")))
                        {
                            Console.WriteLine($"Warning: Skipping row with empty passenger_count at record.");
                        }
                        else
                        {
                            Console.WriteLine($"Error reading record : {ex.Message}");
                        }
                    }
                }
                WriteDuplicatesToCsv(duplicates, "duplicates.csv");

                return records;

            }
        }
        static internal void WriteDuplicatesToCsv(List<DataRecord> duplicates, string filePath)
        {
            using (var writer = new StreamWriter(filePath))
            using (var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)))
            {
                csv.WriteRecords(duplicates);
            }
            Console.WriteLine($"Duplicates written to {filePath}");
        }
    }
}
