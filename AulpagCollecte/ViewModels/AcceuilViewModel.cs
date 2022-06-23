using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using AulpagCollecte.Parameters;
using AulpagCollecte.Services;
using GalaSoft.MvvmLight.Command;

namespace AulpagCollecte.ViewModels
{
    class AcceuilViewModel: INotifyPropertyChanged
    {
        public RelayCommand ValidateCommand { get; }
   
        private DateTime _startDate=DateTime.Now;
        public DateTime StartDate
        {
            get { 
                return _startDate; 
            }
            set { _startDate = value;
                OnPropertyChanged("StartDate"); }
        }

        private DateTime _endDate = DateTime.Now;
        public DateTime EndDate
        {
            get { return _endDate; }
            set { _endDate = value; OnPropertyChanged("StartDate"); }
        }


        public AcceuilViewModel()
        {
           ValidateCommand = new RelayCommand(() => {
               periodes.Debut = StartDate;
               periodes.Fin = EndDate;
               Demarre();
               MessageBox.Show("Traitement terminé");
           } );
        }


        void Demarre()
        {
            MessageBox.Show("Traitement en cours");
            // Nettoyer.Services2(); 

            CollecteZip.DownLoadApiSncf();
            CollecteZip.ExtractZip();
            CollecteZip.Import_Donnees();
            GestionDates.Traitement();
            Mise_a_jour_tables.Traitement();
            Commentaires.Get_commentaires();
            Alerte_changement.RecordHisto();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }     
    }
}
