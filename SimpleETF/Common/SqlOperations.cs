using SimpleETF.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleETF.Common
{
    internal class SqlOperations
    {
        static internal void ProcessRecords(List<DataRecord> records, string connectionString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                {
                    bulkCopy.DestinationTableName = "DataRecords";

                    DataTable dataTable = new DataTable();
                    dataTable.Columns.Add("tpep_pickup_datetime", typeof(DateTime));
                    dataTable.Columns.Add("tpep_dropoff_datetime", typeof(DateTime));
                    dataTable.Columns.Add("passenger_count", typeof(int));
                    dataTable.Columns.Add("trip_distance", typeof(double));
                    dataTable.Columns.Add("store_and_fwd_flag", typeof(string));
                    dataTable.Columns.Add("PULocationID", typeof(int));
                    dataTable.Columns.Add("DOLocationID", typeof(int));
                    dataTable.Columns.Add("fare_amount", typeof(double));
                    dataTable.Columns.Add("tip_amount", typeof(double));


                    TimeZoneInfo estZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"); // EST timezone
                    foreach (var record in records)
                    {
                        DateTime pickupUtc = TimeZoneInfo.ConvertTimeToUtc(record.tpep_pickup_datetime, estZone);
                        DateTime dropoffUtc = TimeZoneInfo.ConvertTimeToUtc(record.tpep_dropoff_datetime, estZone);

                        dataTable.Rows.Add(
                            pickupUtc,
                            dropoffUtc,
                            record.passenger_count,
                            record.trip_distance,
                            record.store_and_fwd_flag,
                            record.PULocationID,
                            record.DOLocationID,
                            record.fare_amount,
                            record.tip_amount
                        );
                    }

                    bulkCopy.WriteToServer(dataTable);
                }
            }

            Console.WriteLine("Data loaded into MS SQL using bulk insert.");
        }
    }
}
