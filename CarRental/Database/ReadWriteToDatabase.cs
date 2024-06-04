using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.IO;
using Microsoft.VisualBasic.FileIO;
using System.Reflection;

namespace CarRental.Database
{
    class ReadWriteToDatabase
    {
        private static string csvPath
        {
            get 
            {
                string exePath = Assembly.GetExecutingAssembly().Location;
                string exeDirectory = Path.GetDirectoryName(exePath);
                string filePath = Path.Combine(exeDirectory, "RentsDatabase.csv");
                if (!File.Exists(filePath)) using (FileStream fs = File.Create(filePath)) { }
                return filePath; 
            }
        }
        
        public static int StartRenting(string regNr, string persNr, string bilKat, string matarställning, DateTime rentingStartedTime)
        {
            
            return AddRowToDataTable(ReadCsvIntoDataTable(csvPath), regNr, persNr, bilKat, matarställning, rentingStartedTime);
        }
        public static DataTable CreateCustomDataTable()
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Index", typeof(int));
            dataTable.Columns.Add("RegNr", typeof(string));
            dataTable.Columns.Add("PersNr", typeof(string));
            dataTable.Columns.Add("BilKat", typeof(string));
            dataTable.Columns.Add("Matarställning", typeof(string));
            dataTable.Columns.Add("RentingStartedTime", typeof(DateTime));
            dataTable.Columns.Add("RentingStoppedTime", typeof(DateTime));
            return dataTable;
        }

        public static DataTable ReadCsvIntoDataTable(string filePath)
        {

            if (!File.Exists(filePath)) return null;
            DataTable dataTable = CreateCustomDataTable();
            

            using (StreamReader sr = new StreamReader(filePath))
            {
                string line;
                bool isFirstRow = true;

                while ((line = sr.ReadLine()) != null)
                {
                    string[] values = line.Split(',');

                    if (isFirstRow)
                    {
                        // Skip header row
                        isFirstRow = false;
                        continue;
                    }

                    DataRow row = dataTable.NewRow();
                    row["Index"] = int.Parse(values[0].Trim());
                    row["RegNr"] = values[1].Trim();
                    row["PersNr"] = values[2].Trim();
                    row["BilKat"] = values[3].Trim();
                    row["Matarställning"] = values[4].Trim();
                    row["RentingStartedTime"] = DateTime.Parse(values[5].Trim());
                    row["RentingStoppedTime"] = DateTime.Parse(values[6].Trim());
                    dataTable.Rows.Add(row);
                }
            }
            if (dataTable == null || dataTable.Columns.Count == 0) return null;

            return dataTable;
        }
        public static void WriteDataTableToCsv(DataTable dataTable, string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                // Write the header line
                string[] columnNames = dataTable.Columns.Cast<DataColumn>()
                                        .Select(column => column.ColumnName)
                                        .ToArray();
                writer.WriteLine(string.Join(",", columnNames));

                // Write the data lines
                foreach (DataRow row in dataTable.Rows)
                {
                    string[] fields = row.ItemArray.Select(field => field.ToString()).ToArray();
                    writer.WriteLine(string.Join(",", fields));
                }
            }
        }
        public static int AddRowToDataTable(DataTable dataTable, string regNr, string persNr, string bilKat, string matarställning, DateTime rentingStartedTime)
        {
            // Create the DataTable if it is null
            if (dataTable == null)
            {
                dataTable = CreateCustomDataTable();
            }

            // Determine the index for the new row
            int newIndex = 1;
            if (dataTable.Rows.Count > 0)
            {
                newIndex = dataTable.AsEnumerable()
                                    .Where(row => row.Field<int?>("Index").HasValue)
                                    .Max(row => row.Field<int>("Index")) +1; 
            }

            // Create a new DataRow and add the input values
            DataRow newRow = dataTable.NewRow();
            newRow["Index"] = newIndex;
            newRow["RegNr"] = regNr;
            newRow["PersNr"] = persNr;
            newRow["BilKat"] = bilKat;
            newRow["Matarställning"] = matarställning;
            newRow["RentingStartedTime"] = rentingStartedTime;
            newRow["RentingStoppedTime"] = DateTime.MinValue;


            // Add the DataRow to the DataTable
            dataTable.Rows.Add(newRow);
            WriteDataTableToCsv(dataTable, csvPath);
            return newIndex;
        }

        public static DataRow GetRowByIndex(DataTable dataTable, int index)
        {
            // Check if the DataTable is null or empty
            if (dataTable == null || dataTable.Rows.Count == 0)
            {
                throw new ArgumentException("The DataTable is null or empty.");
            }

            // Find the row with the specified index
            DataRow foundRow = dataTable.AsEnumerable()
                                        .FirstOrDefault(row => row.Field<int>("Index") == index);

            // Check if the row was found
            if (foundRow == null)
            {
                throw new ArgumentException($"No row found with Index = {index}.");
            }

            return foundRow;
        }

        public static DataRow EditRowByIndex(DataTable dataTable, int index, DateTime rentingStoppedTime)
        {
            // Check if the DataTable is null or empty
            if (dataTable == null || dataTable.Rows.Count == 0)
            {
                throw new ArgumentException("The DataTable is null or empty.");
            }

            // Find the row with the specified index
            DataRow foundRow = dataTable.AsEnumerable()
                                        .FirstOrDefault(row => row.Field<int>("Index") == index);

            // Check if the row was found
            if (foundRow == null)
            {
                throw new ArgumentException($"No row found with Index = {index}.");
            }
            if (DateTime.Compare((DateTime)foundRow["RentingStoppedTime"], DateTime.MinValue) == 0)
            {
                // Update the values of the found row
                foundRow["RentingStoppedTime"] = rentingStoppedTime;
                WriteDataTableToCsv(dataTable, csvPath);
            }
            return foundRow;
        }

        public static DataRow StopRenting(int index, DateTime rentingStoppedTime)
        {
            return EditRowByIndex(ReadCsvIntoDataTable(csvPath), index, rentingStoppedTime);
        }


    }
}
