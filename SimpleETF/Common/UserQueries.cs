using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleETF.Common
{
    internal class UserQueries
    {
        //This queries made to test if program worked correctly
        static internal void ExecuteQueries(string connectionString)
        {

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // 1. Find out which `PULocationId` (Pick-up location ID) has the highest tip_amount on average.
                FindHighestAverageTip(connection);

                // 2. Find the top 100 longest fares in terms of `trip_distance`.
                FindTop100LongestTripsByDistance(connection);

                // 3. Find the top 100 longest fares in terms of time spent traveling.
                FindTop100LongestTripsByTime(connection);

                // 4. Search, where part of the conditions is `PULocationId`.
                SearchByPULocationId(connection, 238); // Example where PULocationId = 238
            }
        }

        static void FindHighestAverageTip(SqlConnection connection)
        {
            string sql = @"
                SELECT TOP 1 PULocationID, AVG(tip_amount) AS AverageTip
                FROM DataRecords
                GROUP BY PULocationID
                ORDER BY AverageTip DESC";

            using (SqlCommand command = new SqlCommand(sql, connection))
            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    Console.WriteLine($"Highest average tip PULocationID: {reader["PULocationID"]}, Average tip: {reader["AverageTip"]}");
                }
            }
        }

        static void FindTop100LongestTripsByDistance(SqlConnection connection)
        {
            string sql = @"
                SELECT TOP 100 *
                FROM DataRecords
                ORDER BY trip_distance DESC";

            using (SqlCommand command = new SqlCommand(sql, connection))
            using (SqlDataReader reader = command.ExecuteReader())
            {
                Console.WriteLine("Top 100 longest trips by distance:");
                while (reader.Read())
                {
                    Console.WriteLine($"Trip distance: {reader["trip_distance"]}, Pickup: {reader["tpep_pickup_datetime"]}, Dropoff: {reader["tpep_dropoff_datetime"]}");
                }
            }
        }

        static void FindTop100LongestTripsByTime(SqlConnection connection)
        {
            string sql = @"
                SELECT TOP 100 *, DATEDIFF(SECOND, tpep_pickup_datetime, tpep_dropoff_datetime) AS TripTime
                FROM DataRecords
                ORDER BY TripTime DESC";

            using (SqlCommand command = new SqlCommand(sql, connection))
            using (SqlDataReader reader = command.ExecuteReader())
            {
                Console.WriteLine("Top 100 longest trips by time:");
                while (reader.Read())
                {
                    Console.WriteLine($"Trip time (seconds): {reader["TripTime"]}, Pickup: {reader["tpep_pickup_datetime"]}, Dropoff: {reader["tpep_dropoff_datetime"]}");
                }
            }
        }

        static void SearchByPULocationId(SqlConnection connection, int locationId)
        {
            string sql = @"
                SELECT *
                FROM DataRecords
                WHERE PULocationID = @locationId";

            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@locationId", locationId);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    Console.WriteLine($"Search results for PULocationID {locationId}:");
                    while (reader.Read())
                    {
                        Console.WriteLine($"Pickup: {reader["tpep_pickup_datetime"]}, Dropoff: {reader["tpep_dropoff_datetime"]}, Trip distance: {reader["trip_distance"]}");
                    }
                }
            }
        }
    }
}
