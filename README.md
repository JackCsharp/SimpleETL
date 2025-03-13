SQL files are in folder Sql, 
number of rows in db: 29889, number of rows in file duplicates.csv 111
If the program were used to process a 10GB CSV file instead of storing all records in a List<DataRecord>, I would read and process rows in chunks, probably by using asynchronous streams.
