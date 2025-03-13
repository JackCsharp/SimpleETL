
CREATE INDEX IX_DataRecords_PULocationID ON DataRecords (PULocationID);

CREATE INDEX IX_DataRecords_tip_amount ON DataRecords (tip_amount);

CREATE INDEX IX_DataRecords_trip_distance ON DataRecords (trip_distance DESC);