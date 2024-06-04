using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Reflection;

namespace CarRental.ViewModels
{
    class RentCarViewModel : INotifyPropertyChanged
    {
        public RentCarViewModel()
        {

        }

        private string mRegNr;
        public string RegNr
        {
            get { return mRegNr; }
            set
            {
                mRegNr = value;
                OnPropertyChanged("RegNr");
            }
        }

        private string mPersNr;
        public string PersNr
        {
            get { return mPersNr; }
            set
            {
                if (mPersNr != value)
                {
                    mPersNr = value;
                    OnPropertyChanged("PersNr");
                }
            }
        }

        private string mBilKat;
        public string BilKat
        {
            get { return mBilKat; }
            set
            {
                if (mBilKat != value && IsValidBilKat(value))
                {

                    mBilKat = value;
                    OnPropertyChanged("BilKat");
                }
            }
        }
        private static bool IsValidBilKat(string bilKat)
        {
            switch (bilKat)
            {
                case "småbil":
                case "kombi":
                case "lastbil":
                    return true;
                default:
                    return false;
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


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string property)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        public void OnStartRentingClicked(object sender)
        {
            string test = Assembly.GetExecutingAssembly().Location;
            ResultText = null;
            if (ResultText == null)RegNr = SanityCheckRegNr(RegNr);
            if (ResultText == null) PersNr = SanityCheckPersNr(PersNr);
            if (ResultText == null) if (!IsValidBilKat(BilKat)) ResultText = "Välj en giltig bilkategori" ;
            if (ResultText == null) Matarställning = SanityCheckMatarstallning(Matarställning);
            
            int UthyrningsID = 0;
            if (ResultText == null) UthyrningsID = Database.ReadWriteToDatabase.StartRenting(RegNr, PersNr, BilKat, Matarställning, DateTime.Now);
            if (UthyrningsID !=0 ) ResultText = "Uthyrningen påbörjades, uthyrnings ID är " + UthyrningsID.ToString();
            else if (ResultText == null) ResultText = "Något blev fel och uthyrningen påbörjades inte";
        }

        private DateTime mRentTime;
        public DateTime RentTime
        {
            get { return mRentTime; }
            set
            {
                if (mRentTime != value)
                {
                    mRentTime = value;
                    OnPropertyChanged("RentTime");
                }
            }
        }
        public List<string> BilKategorier
        {
            get { return new List<string> { "småbil", "kombi", "lastbil" }; }
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


        public string SanityCheckRegNr(string regNr)
        {
            if (regNr == null)
            {
                ResultText = "Registreringsnummer kan inte vara tomt.";
                return null;
            }

            if (regNr.Length == 6 && Regex.IsMatch(regNr, @"^[a-zA-Z0-9]{6}$"))
            {
                return regNr;
            }
            else if (regNr.Length == 7 && regNr[3] == ' ' && Regex.IsMatch(regNr.Replace(" ", ""), @"^[a-zA-Z0-9]{6}$"))
            {
                return regNr.Replace(" ", "");
            }
            else
            {
                ResultText = "Ogiltigt registreringsnummer. Det måste vara 6 bokstäver eller siffror utan mellanslag, eller 7 tecken med ett mellanslag i mitten.";
                return null;
            }
        }

        public string SanityCheckPersNr(string persNr)
        {
            if (persNr == null)
            {
                ResultText = "Personnummer kan inte vara tomt.";
                return null;
            }

            if (Regex.IsMatch(persNr, @"^\d{10}$"))
            {
                return persNr.Insert(6, "-");
            }
            else if (Regex.IsMatch(persNr, @"^\d{6}-\d{4}$"))
            {
                return persNr;
            }
            else
            {
                ResultText = "Ogiltigt personnummer. Det måste vara 10 siffror, eller 11 tecken där alla utom det sjunde är siffror och det sjunde är ett bindestreck.";
                return null;
            }
        }

        public string SanityCheckMatarstallning(string matarstallning)
        {
            if (matarstallning == null)
            {
                ResultText = "Mätarställning kan inte vara tomt.";
                return null;
            }

            if (int.TryParse(matarstallning, out int result) && result > 0)
            {
                return matarstallning;
            }
            else
            {
                ResultText = "Ogiltig mätarställning. Det måste vara ett positivt heltal.";
                return null;
            }
        }




    }
}
