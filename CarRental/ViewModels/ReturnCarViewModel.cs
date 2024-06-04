using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Data;

namespace CarRental.ViewModels
{
    public class ReturnCarViewModel : INotifyPropertyChanged
    {
        public void OnStopRentingClicked(object sender)
        {
            if (!string.IsNullOrEmpty(Matarställning) && !string.IsNullOrEmpty(BokNr))
            {
                int intMatarställning;
                int intBokNr;

                if (int.TryParse(Matarställning, out intMatarställning) && int. TryParse(BokNr, out intBokNr))
                {
                    DateTime nu = DateTime.Now;
                    DataRow dataRow = Database.ReadWriteToDatabase.StopRenting(intBokNr, nu); 

                    if (dataRow == null) ResultText = "Bokningsnummret finns inte i systemet";
                    else if (DateTime.Compare(nu, (DateTime)dataRow["RentingStoppedTime"]) != 0) ResultText = "Den här bilen har redan returnerats";
                    else
                    {
                        int oldMatarställning = int.Parse(dataRow["Matarställning"].ToString());
                        int diffMatarställning = intMatarställning - oldMatarställning;  //funkar fint att ha negativ diff, tillochmed negativ mätarställning. Går obviusly att fixa
                        int days = (int)Math.Ceiling(((DateTime)dataRow["RentingStoppedTime"] - (DateTime)dataRow["RentingStartedTime"]).TotalDays);
                        double price = 0;
                        string bilKat = dataRow["BilKat"].ToString();
                        switch (bilKat)
                        {
                            case "småbil":
                                price = GlobalValues.basDygnsHyra * days;
                                break;
                            case "kombi":
                                price = GlobalValues.basDygnsHyra * days*1.3 + GlobalValues.baskmPris*diffMatarställning;
                                break;
                            case "lastbil":
                                price = GlobalValues.basDygnsHyra * days *1.5 + GlobalValues.baskmPris * diffMatarställning*1.5;
                                break;
                            default:
                                ResultText = "Bilkategorin känns inte igen";
                                break;
                        }
                        if (price != 0) ResultText = "Bilen har nu returnerats, kostnaden är " + price.ToString() + " kronor.";

                    }
                }
                else
                {
                    ResultText = "Båda fälten behöver vara siffror, utan kommatecken";
                }
            }
            else
            {
                ResultText = "Fyll i värden i båda fälten";
            }
        }

        private string mMatarställning;
        public string Matarställning
        {
            get { return mMatarställning; }
            set
            {
                if (mMatarställning != value)
                {
                    mMatarställning = value;
                    OnPropertyChanged("Matarställning");
                }
            }
        }

        private string mBokNr;
        public string BokNr
        {
            get { return mBokNr; }
            set
            {
                if (mBokNr != value)
                {
                    mBokNr = value;
                    OnPropertyChanged("BokNr");
                }
            }
        }
        private string mResultText;
        public string ResultText
        {
            get { return mResultText; }
            set
            {
                if (mResultText != value)
                {
                    mResultText = value;
                    OnPropertyChanged("ResultText");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string property)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
