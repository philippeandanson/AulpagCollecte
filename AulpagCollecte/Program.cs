using System;
using System.Windows;
using AulpagCollecte.Views;


namespace AulpagCollecte
{
    class Program
    {

        public static Application WinApp { get; private set; }
        public static Window MainWindow { get; private set; }

        [STAThread]
        static void Main(string[] args)
        {
            InitializeWindows();
        }

        static void InitializeWindows()
        {
            WinApp = new Application();
            WinApp.Run(MainWindow = new Acceuil()); //note: blocking call
        }
        
    }
   
}
