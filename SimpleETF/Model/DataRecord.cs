using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleETF.Model
{
    internal class DataRecord
    {
        public DateTime tpep_pickup_datetime { get; set; }
        public DateTime tpep_dropoff_datetime { get; set; }
        public int? passenger_count { get; set; }
        public double trip_distance { get; set; }
        public string store_and_fwd_flag { get; set; }
        public int PULocationID { get; set; }
        public int DOLocationID { get; set; }
        public double fare_amount { get; set; }
        public double tip_amount { get; set; }
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            DataRecord other = (DataRecord)obj;
            return tpep_pickup_datetime == other.tpep_pickup_datetime &&
                   tpep_dropoff_datetime == other.tpep_dropoff_datetime &&
                   passenger_count == other.passenger_count;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(tpep_pickup_datetime, tpep_dropoff_datetime, passenger_count);
        }
    }
}
