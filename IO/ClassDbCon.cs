using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace IO
{
    public class ClassDbCon
    {

        private string _connectionString;
        protected SqlConnection con;
        private SqlCommand _command;

        public ClassDbCon()
        {
            // Server=myServerAddress;Database=myDataBase;Trusted_Connection=True;
            // Server=myServerAddress;Database=myDataBase;User Id=myUsername;Password=myPassword;

            //_connectionString = @"Server=(localdb)\MSSQLLocalDB;Database=FamilyDB;Trusted_Connection=True;";
            //con = new SqlConnection(_connectionString);
        }

        public ClassDbCon(string inConnectionString)
        {
            _connectionString = inConnectionString;
            con = new SqlConnection(_connectionString);
        }

        protected void SetCon(string inConnectionString)
        {
            _connectionString = inConnectionString;
            con = new SqlConnection(_connectionString);
        }

        /// <summary>
        /// Denne metode åbner forbindelsen til databasen.
        /// Den undersøger om de gængse betingelser er opfyldt for at åbne forbindelsen, inden den åbnes.
        /// Hvis betingelserne ikke er opfyldt, vil den prøve at håndtere de mest almindelige fejl og mangler.
        /// </summary>
        protected void OpenDB()
        {
            try
            {
                if (this.con != null && con.State == ConnectionState.Closed) // Undersøger om instansen con er initialiseret og at der ikke er en åben forbindelse i forvejen.
                {
                    con.Open(); // Åbner forbindelsen til DB
                }
                else  // Hvis betingelserne i 'if' ikke er opfyldt
                {
                    if (con.State == ConnectionState.Open)  // Undersøger om fejlen skyldes at der en åben forbindelse i forvejen
                    {
                        // Hvis ja - Lukker den forbindelsen og kalder 'sig selv' igen for at åbne forbindelsen.
                        CloseDB();
                        OpenDB();
                    }
                    else // Hvis det ikke var på grund af en åben forbindelse, må det være på grund af manglende initialisering af 'con'
                    {
                        con = new SqlConnection(_connectionString); // Initialisere 'con' med den angivne connectionstring
                        OpenDB(); // Kalder 'sig selv' igen for at åbne forbindelsen
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Denne metode lukker forbindelsen til DB
        /// </summary>
        protected void CloseDB()
        {
            try
            {
                con.Close(); // Lukker forbindelsen
            }
            catch (SqlException ex)  // Håndtere de exceptions (fejl) der måtte opstå under kommunikationen med databasen
            {
                throw ex;
            }
        }

        /// <summary>
        /// Denne metode har til formål, at udføre de handlinger i databasen, som ikke kræver at der returneres et resultatsæt.
        /// Metoden vil dog altid returnere en intiger værdi der angiver om handlingen gik godt eller skidt.
        /// Returneres: -1 er handlingen ikke blevet udført
        /// Returneres: Et tal fra 0 til N, indikerer det at udtrykket kunne eksekveres og angiver hvor mange datasæt der 
        /// blev påvirket
        /// </summary>
        /// <param name="sqlQuery">string</param>
        /// <returns>int</returns>
        protected int ExecuteNonQuery(string sqlQuery)
        {
            int res = 0;
            _command = new SqlCommand(sqlQuery, con); // Her initialiseres instansen af SqlCommand med parameterne string query og SqlConnection con

            try
            {
                OpenDB(); // Åbner forbindelsen til DB
                res = _command.ExecuteNonQuery(); // Her kaldes databasen og den givne query eksekveres
            }
            catch (SqlException ex) // Håndtere de exceptions (fejl) der måtte opståe under kommunikationen med databasen
            {
                throw ex;
            }
            finally  // Ved angivelse 'finally' sikre jeg, at det der står i 'finally' altid bliver udført, uanset om koden kunne eksekveres med eller uden fejl
            {
                CloseDB(); // Lukker forbindelsen til DB
            }

            return res;
        }

        protected int ExecuteNonQuery(SqlCommand inCommand)
        {
            int res = 0;

            try
            {
                OpenDB(); // Åbner forbindelsen til DB
                res = inCommand.ExecuteNonQuery(); // Her kaldes databasen og den givne query eksekveres, det nye ID returneres
            }
            catch (SqlException ex) // Håndtere de exceptions (fejl) der måtte opståe under kommunikationen med databasen
            {
                throw ex;
            }
            finally  // Ved angivelse 'finally' sikre jeg, at det der står i 'finally' altid bliver udført, uanset om koden kunne eksekveres med eller uden fejl
            {
                CloseDB(); // Lukker forbindelsen til DB
            }

            return res; // Returnere det nye ID i tabellen
        }

        protected int ExecuteScalarInDB(SqlCommand inCommand)
        {
            int res = 0;

            try
            {
                OpenDB(); // Åbner forbindelsen til DB
                res = Convert.ToInt32(inCommand.ExecuteScalar()); // Her kaldes databasen og den givne query eksekveres
            }
            catch (SqlException ex) // Håndtere de exceptions (fejl) der måtte opståe under kommunikationen med databasen
            {
                throw ex;
            }
            finally  // Ved angivelse 'finally' sikre jeg, at det der står i 'finally' altid bliver udført, uanset om koden kunne eksekveres med eller uden fejl
            {
                CloseDB(); // Lukker forbindelsen til DB
            }

            return res;
        }

        /// <summary>
        /// Denne metode skal håndtere forespørgelser til databasen som skal returnere et resultatsæt.
        /// Det resultatsæt der modtages fra DB, konverteres over i en collection af typen DataTable
        /// </summary>
        /// <param name="sqlQuery">string</param>
        /// <returns>DataTable</returns>
        protected DataTable DbReturnDataTable(string sqlQuery)
        {
            DataTable dtRes = new DataTable();
            try
            {
                OpenDB();
                using(_command = new SqlCommand(sqlQuery, con)) // Her initialiseres instansen af SqlCommand med parameterne string query og SqlConnection con
                {
                    using (var adapter  = new SqlDataAdapter(_command)) // Her foretages kaldet til databasen ved, at der oprettes en ny instans af en SqlDataAdapter. Resultatet overføres til en abstrakt datatype.
                    {
                        adapter.Fill(dtRes); // Her overføres data fra den abstrakte datatype til den DataTable metoden skal returnere
                    }
                }
            }
            catch (SqlException ex) // Håndtere de exceptions (fejl) der måtte opstå under kommunikationen med databasen
            {
                throw ex;
            }
            finally // Ved angivelse af 'finally' sikre jeg, at det der står i 'finally' altid bliver udført, uanset om koden kunne eksekveres med eller uden fejl
            {
                CloseDB();
            }

            return dtRes;
        }

        /// <summary>
        /// Denne metode skal håndtere forespørgelser til databasen som skal returnere en tekststreng.
        /// </summary>
        /// <param name="sqlQuery">string</param>
        /// <returns>string</returns>
        protected string DbReturnString(string sqlQuery)
        {
            string res = "";
            bool foundOne = false;

            try
            {
                OpenDB(); // Åbner forbindelsen til databasen
                using (SqlCommand cmd = new SqlCommand(sqlQuery, con)) // Opretter en ny instans af SqlCommand med parameterne sqlQuery og con, 
                                                                       // som indeholder henholdsvis min sql forspørgelse og information omkring 
                                                                       // hvilken database data skal hentes fra.
                {
                    SqlDataReader reader = cmd.ExecuteReader(); // Her eksekveres forespørgelsen på databasen og svaret gemmes i reader som er af datatypen
                                                                // SqlDataReader der har samme egenskaber som en StreamReader, altså egenskaber der gør den
                                                                // egnet til at modtage og holde en stream af tekst

                    while (reader.Read()) // Hvis reader har modtaget et resultat fra databasen, skal den udføre koden i while loopet
                    {
                        res = reader.GetString(0); // Læser teksten fra reader og indsætter den i res.
                        foundOne = true; // Bolsk værdi, der angiver at der er modtaget et resultat
                    }
                    if (!foundOne) // Hvis der ikke findes et resultat i databasen, skal der returneres en tom tekststreng.
                    {
                        res = "";
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                CloseDB();
            }

            return res;
        }

        /// <summary>
        /// Denne metode skal håndtere forespørgelser til databasen som skal returnere et resultatsæt.
        /// Forespørgelsen skal foretages gennem en StoredProcedure på SqlServeren.
        /// Det resultatsæt der modtages fra DB, konverteres over i en collection af typen DataTable
        /// </summary>
        /// <param name="inCommand">SqlCommand</param>
        /// <returns>DataTable</returns>
        protected DataTable MakeCallToStoredProcedure(SqlCommand inCommand)
        {
            DataTable dtRes = new DataTable();

            try
            {
                OpenDB(); // Åbner forbindelsen til databasen
                using (SqlDataAdapter adapter = new SqlDataAdapter(inCommand)) // Her intialiseres en instans af SqlDataAdapter med værdien i inCommand
                {
                    adapter.Fill(dtRes); // Her overføres data fra adapter til den DataTable, metoden skal returnere.
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                CloseDB(); // Lukker forbindelsen til databasen
            }

            return dtRes;
        }

    }
}
