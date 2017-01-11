using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;

namespace StackClone
{
    public class Uporabnik
    {
        public enum TipUporabnika
        {
            Uporabnik, Vip, Admin
        }

        public Uporabnik() { }

        public Uporabnik( int id, string ime, string priimek, string email, string uporabnisko, TipUporabnika tip, int kovanc )
        {
            Id = id;
            Ime = ime;
            Priimek = priimek;
            Email = email;
            Uporabnisko = uporabnisko;
            Tip = tip;
            Kovanc = kovanc;
        }

        public int Id { get; set; } = -1;

        public string Ime { get; set; } = "";

        public string Priimek { get; set; } = "";

        public string Email { get; set; } = "";

        public string Uporabnisko { get; set; } = "";

        public int Kovanc { get; set; } = 0;

        public TipUporabnika Tip { get; set; } = 0;

        public string PriimekIme
        {
            get
            {
                return Priimek.Trim() + " " + Ime.Trim();
            }
        }

        /// <summary>
        /// Brskanje baze po uporabniškem imenu
        /// </summary>
        /// <param name="uporabniskoIme">Brska se po parametru 'Uporabnik'</param>
        /// <returns>Eneg uporabnika ali prazno listo</returns>
        public static List<Uporabnik> Brskaj( int idUporabnika = -1, string uporabniskoIme = "")
        {
            SqlConnection con = new SqlConnection( Nastavitve.GetConnectionString());
            SqlCommand cmd = new SqlCommand();
            
            string where = " 1 = 1 ";
            if (uporabniskoIme != "")   
                where += " AND (Uporabnik = '" + uporabniskoIme + "')";
            if ( idUporabnika != -1 )
                where += " AND (Id = " + idUporabnika + ")";
            //                       0   1      2       3        4            5           6
            string select = "SELECT Id, Ime, Priimek, Email, Uporabnik, TipUporabnika, Kovanc " +
                "FROM [StackDB].[dbo].[tblUporabnik] WHERE" + where;

            cmd.CommandText = select;
            cmd.Connection = con;

            List<Uporabnik> lista = new List<Uporabnik>();

            try
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while ( reader.Read() )
                {
                    int id = reader.GetInt32( 0 );
                    string ime = reader.IsDBNull( 1 ) ? "" : reader.GetString( 1 ).Trim();
                    string priimek = reader.IsDBNull( 2 ) ? "" : reader.GetString( 2 ).Trim();
                    string email = reader.IsDBNull( 3 ) ? "" : reader.GetString( 3 ).Trim();
                    string uporabnisko = reader.GetString( 4 ).Trim();
                    TipUporabnika tip = (TipUporabnika)reader.GetInt32( 5 );
                    int kovanc = reader.GetInt32( 6 );

                    Uporabnik uporabnik = new Uporabnik( id, ime, priimek, email, uporabnisko, tip, kovanc );
                    lista.Add( uporabnik );
                }
                reader.Close();
                return lista;
            }
            catch ( TimeoutException tEx )
            {
                // Zapisivanje u log
                return null;
            }
            catch ( Exception ex )
            {
                // log
                return null;
            }
            finally
            {
                con.Close();
            }

        }

        /// <summary>
        /// Dodavanje novih uporabnikov
        /// </summary>
        /// <param name="uporabnik">Objekt s uporabnikom kateroga hočemo dodati v bazo</param>
        /// <returns>ID noveg uporabnika</returns>
        public static int Dodaj( Uporabnik uporabnik, string geslo )
        {
            using ( SqlConnection con = new SqlConnection( Nastavitve.GetConnectionString() ) )
            {
                try
                {
                    con.Open();
                    string into = "[StackDB].[dbo].[tblUporabnik]";
                    string insert = "INSERT INTO " + into + " (Ime, Priimek, Email, Uporabnik, Geslo, Salt, TipUporabnika, Kovanc) " +
                        "VALUES (@Ime, @Priimek, @Email, @Uporabnik, @Geslo, @Salt, @TipUporabnika, @Kovanc); " +
                        "SELECT CAST(scope_identity() as int)";//'" + uporabnik.Ime + "', '" + uporabnik.Priimek + "', '" + uporabnik.Email + "', '" +
                                                               //uporabnik.Uporabnisko + "', " + uporabnik.Tip + ", " + uporabnik.Kovanc;

                    byte[] gesloBytes = Encoding.UTF8.GetBytes( geslo );
                    byte[] saltBytes = Encoding.UTF8.GetBytes( CreateSalt( 20 ) );
                    byte[] gesloHash = GenerateSaltedHash( gesloBytes, saltBytes );

                    SqlCommand cmd = new SqlCommand( insert, con );
                    uporabnik.Parametriziraj( ref cmd, gesloHash, saltBytes );
                    cmd.CommandTimeout = 30;

                    int? id = -1;
                    id = (int?)cmd.ExecuteScalar();
                    return (id != null) ? (int)id : -1;
                }
                catch ( TimeoutException tEx )
                {
                    // Zapisivanje u log
                    return -1;
                }
                catch ( Exception ex )
                {
                    // log
                    return -1;
                }
            }
        }


        /// <summary>
        /// Update postoječih uporabnikov
        /// </summary>
        /// <param name="uporabnik">Objekt s uporabnikom kateroga hočemo spremeniti v bazi</param>
        /// <param name="salt">Opcionalni parameter kje se pohrani salt od uporabnikovog gesla</param>
        /// <param name="geslo">Opcionalni parameter z hashiranim geslom</param>
        /// <returns></returns>
        public static bool Update( Uporabnik uporabnik, byte[] salt = null, byte[] geslo = null )
        {
            SqlConnection con = new SqlConnection( Nastavitve.GetConnectionString() );
            SqlCommand cmd = new SqlCommand();

            string table = "[StackDB].[dbo].[tblUporabnik]";
            string where = " WHERE Id = " + uporabnik.Id;
            string update = "UPDATE " + table +
                " SET Ime = '" + uporabnik.Ime + "', " +
                "Priimek = '" + uporabnik.Priimek + "', " +
                "Email = '" + uporabnik.Email + "', " +
                "Uporabnik = '" + uporabnik.Uporabnisko + "', " +
                "TipUporabnika = " + (int)uporabnik.Tip +
                ", Kovanc = " + uporabnik.Kovanc;

            if ( salt != null && geslo != null )
            {
                update += ", Salt = @Salt, Geslo = @Geslo";

                cmd.Parameters.Add( "@Salt", SqlDbType.Binary );
                cmd.Parameters["@Salt"].Value = salt;

                cmd.Parameters.Add( "@Geslo", SqlDbType.Binary );
                cmd.Parameters["@Geslo"].Value = geslo;
            }

            cmd.CommandText = update + where;
            cmd.Connection = con;

            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                return true;
            }
            catch ( TimeoutException tEx )
            {
                // Zapisivanje u log
                return false;
            }
            catch ( Exception ex )
            {
                // log
                return false;
            }
            finally
            {
                con.Close();
            }
        }

        /// <summary>
        /// Brisanje uporabnika iz baze
        /// </summary>
        /// <param name="idUporabnika">Id dosežka kateroga brišemo</param>
        /// <returns></returns>
        public static bool Brisanje( int idUporabnika )
        {
            SqlConnection con = new SqlConnection( Nastavitve.GetConnectionString() );
            SqlCommand cmd = new SqlCommand();

            string from = "[StackDB].[dbo].[tblUporabnik]";
            string delete = "DELETE FROM " + from + " WHERE Id = " + idUporabnika;

            cmd.CommandText = delete;
            cmd.Connection = con;

            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                return true;
            }
            catch ( TimeoutException tEx )
            {
                // Zapisivanje u log
                return false;
            }
            catch ( Exception ex )
            {
                // log
                return false;
            }
            finally
            {
                con.Close();
            }
        }

        /// <summary>
        /// Metoda katera doda parametre v SqlCommand
        /// </summary>
        /// <param name="param">SqlParameterCollection kateri se polni z parametri</param>
        /// <param name="gesloHash">Hashirano geslo</param>
        /// <param name="saltHash">Hashirani salt</param>
        /// <returns></returns>
        private void Parametriziraj(ref SqlCommand cmd, byte[] gesloHash = null, byte[] saltHash = null)
        {
            if ( Id != -1 )
            {
                cmd.Parameters.Add( "@Id", SqlDbType.Int, 4 );
                cmd.Parameters["@Id"].Value = Id;
            }

            cmd.Parameters.Add( "@Ime", SqlDbType.NVarChar );
            cmd.Parameters["@Ime"].Value = Ime;

            cmd.Parameters.Add( "@Priimek", SqlDbType.NVarChar );
            cmd.Parameters["@Priimek"].Value = Priimek;

            cmd.Parameters.Add( "@Email", SqlDbType.NVarChar );
            cmd.Parameters["@Email"].Value = Email;

            cmd.Parameters.Add( "@Uporabnik", SqlDbType.NVarChar );
            cmd.Parameters["@Uporabnik"].Value = Uporabnisko;

            cmd.Parameters.Add( "@TipUporabnika", SqlDbType.Int );
            cmd.Parameters["@TipUporabnika"].Value = Tip;

            cmd.Parameters.Add( "@Kovanc", SqlDbType.Int );
            cmd.Parameters["@Kovanc"].Value = Kovanc;

            if ( gesloHash != null && saltHash != null )
            {
                cmd.Parameters.Add( "@Geslo", SqlDbType.VarBinary );
                cmd.Parameters["@Geslo"].Value = gesloHash;

                cmd.Parameters.Add( "@Salt", SqlDbType.VarBinary );
                cmd.Parameters["@Salt"].Value = saltHash;
            }
            
        }


        /// <summary>
        /// Postavljanje gesla za uporabnika
        /// </summary>
        /// <param name="geslo">Plain text geslo katero hashiramo in spremamo v bazo</param>
        /// <param name="idUporabnika">ID uporabnika za terog spremamo geslo</param>
        /// <returns></returns>
        public static bool HashirajGeslo (int idUporabnika, string geslo)
        {
            byte[] gesloBytes = Encoding.UTF8.GetBytes( geslo );
            byte[] saltBytes = Encoding.UTF8.GetBytes( CreateSalt( 20 ) );
            byte[] hash = GenerateSaltedHash( gesloBytes, saltBytes );

            List<Uporabnik> lista = Brskaj( idUporabnika );
            Uporabnik u = lista.Count > 0 ? lista[0] : null;
            if ( u != null )
            {
                if ( Update( u, saltBytes, hash ) )
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        /// <summary>
        /// Hashiranje gesla z generiranim saltom
        /// </summary>
        /// <param name="plainText">byte array plain text gesla</param>
        /// <param name="salt">generirani salt</param>
        /// <returns>hashirano geslo</returns>
        static byte[] GenerateSaltedHash( byte[] plainText, byte[] salt )
        {
            HashAlgorithm algorithm = new SHA256Managed();

            byte[] plainTextWithSaltBytes =
              new byte[plainText.Length + salt.Length];

            for ( int i = 0; i < plainText.Length; i++ )
                plainTextWithSaltBytes[i] = plainText[i];
            for ( int i = 0; i < salt.Length; i++ )
                plainTextWithSaltBytes[plainText.Length + i] = salt[i];

            return algorithm.ComputeHash( plainTextWithSaltBytes );
        }

        /// <summary>
        /// Poredba dva byte arraya
        /// </summary>
        /// <returns>Vrne se true če so arrayi isti</returns>
        public static bool CompareByteArrays( byte[] array1, byte[] array2 )
        {
            if ( array1.Length != array2.Length )
                return false;

            for ( int i = 0; i < array1.Length; i++ )
            {
                if ( array1[i] != array2[i] )
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Generator salt stringa
        /// </summary>
        /// <param name="size">Veličina salta</param>
        /// <returns>vrne byte array z random saltem</returns>
        private static string CreateSalt( int size )
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] buff = new byte[size];
            rng.GetBytes( buff );

            return Convert.ToBase64String( buff );
        }

        /// <summary>
        /// Avtorizacija uporabnika
        /// </summary>
        /// <param name="idUporabnika">ID uporabnika kateroga preverjamo</param>
        /// <param name="novoGesloString">Geslo katero preverjamo če velja</param>
        /// <returns></returns>
        internal static bool? PotrdiGeslo(int idUporabnika, string novoGesloString)
        {
            Uporabnik u = Brskaj( idUporabnika)[0];
            byte[] novoGeslo = Encoding.UTF8.GetBytes( novoGesloString );
            byte[] geslo = new byte[32];
            byte[] salt = new byte[28];

            SqlConnection con = new SqlConnection( Nastavitve.GetConnectionString() );
            SqlCommand cmd = new SqlCommand();
            
            string select = "SELECT Geslo, Salt FROM [StackDB].[dbo].[tblUporabnik] WHERE Id = " + idUporabnika;

            cmd.CommandText = select;
            cmd.Connection = con;

            try
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while ( reader.Read() )
                {
                    geslo = (byte[])reader["Geslo"];
                    salt = (byte[])reader["Salt"];
                }
                reader.Close();
                byte[] noviHash = GenerateSaltedHash( novoGeslo, salt );
                if ( CompareByteArrays( noviHash, geslo ) )
                    return true;
                else
                    return false;
            }
            catch ( TimeoutException tEx )
            {
                // Zapisivanje u log
                return null;
            }
            catch ( Exception ex )
            {
                // log
                return null;
            }
            finally
            {
                con.Close();
            }


        }

    }
}
