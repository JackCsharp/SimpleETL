CREATE TABLE DataRecords (
    tpep_pickup_datetime DATETIME,
    tpep_dropoff_datetime DATETIME,
    passenger_count INT NULL,
    trip_distance FLOAT,
    store_and_fwd_flag NVARCHAR(3),
    PULocationID INT,
    DOLocationID INT,
    fare_amount FLOAT,
    tip_amount FLOAT
);