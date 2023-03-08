using System.ComponentModel;

namespace Repository
{
    /// <summary>
    /// ClassNotify er den class, som alle de classes vi benytter i vores solution, skal nedarve for at kunne benytte PropertyChangedEventHandler.
    /// Dette gøres ved at ClassNotify implementere interfacet INotifyPropertyChanged, som sikre os adgang til System.ComponentModel og derved
    /// funktionaliteten der kan opdatere objekter på GUI med data fra et Property.
    /// </summary>
    public class ClassNotify : INotifyPropertyChanged
    {
        /// <summary>
        /// Når der i en class implenteres INotifyPropertyChanged, vil interfacet altid forlange at der generes en public event af datatypen
        /// PropertyChangedEventHandler som default navngives PropertyChanged.
        /// Det er gennem denne instans, at der skabes forbindelse til det bagved liggende kode-bibliotek i System.ComponentModel.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ClassNotify()
        {

        }

        /// <summary>
        /// Metoden Notify er erklæret protected, for at sikre at metoden KUN kan benyttes gennem nedarvning.
        /// Ved kald til metoden SKAL der parameteroverføres en string med navnet på den Property der er blevet opdateret.
        /// For ikke at risikere, at man overloader systemet, kontroleres der først for om der realt er foretaget en opdatering
        /// af et Property i den respektive class.
        /// Hvis der er registreret en opdatering i classen, udføres eventhandleren PropertyChanged.
        /// Eventhandleren PropertyChanged skal medsende to Parametere:
        ///      - Første parameter er en instans af den class eventet er foretaget i, dette gøres ved at benytte koden 'this'.
        ///      - Andet parameter er en ny instans af PropertyChangedEventArgs, som skal instantiseres med en textstreng som 
        ///        holder navnet på det Property der er blevet opdateret.
        ///        
        /// Herved overføres værdien der knytter sig til det nævnte property til det objekt på GUI som er binded via navnet på Property
        /// </summary>
        /// <param name="propertyName">string (Navnet på Property der er opdateret)</param>
        protected void Notify(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }
}
