using System;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace StackClone
{
    public class TopRezultat
    {
        public TopRezultat() { }

        public TopRezultat( Uporabnik uporabnik, int rezultat )
        {
            Uporabnik = uporabnik;
            Rezultat = rezultat;
        }


        public int Id { get; set; } = -1;

        public Uporabnik Uporabnik { get; set; } = new Uporabnik();

        public int Rezultat { get; set; } = -1;

        /// <summary>
        /// Pridobivanje najboljših rezultatov
        /// </summary>
        /// <param name="velikostSeznama">Število rezultatov kateri se vrnejo</param>
        /// <returns></returns>
        public static List<TopRezultat> GetTopRezultati( int velikostSeznama )
        {
            SqlConnection con = new SqlConnection( Nastavitve.GetConnectionString() );
            SqlCommand cmd = new SqlCommand();

            if ( velikostSeznama < 1 )
                velikostSeznama = 1;

            string select = "SELECT TOP " + velikostSeznama +
                //     0         1      2       3        4         5   
                " IdUporabnika, Ime, Priimek, Email, Uporabnik, Rezultat " +
                "FROM [StackDB].[dbo].[viewRezultati] ORDER BY Rezultat DESC";

            cmd.CommandText = select;
            cmd.Connection = con;

            List<TopRezultat> lista = new List<TopRezultat>();

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
                    string uporabnisko = reader.IsDBNull( 4 ) ? "" : reader.GetString( 4 ).Trim();
                    int rezultat = reader.IsDBNull( 5 ) ? -1 : reader.GetInt32( 5 );

                    Uporabnik uporabnik = new Uporabnik( id, ime, priimek, email, uporabnisko, 0, 0 );
                    TopRezultat topRezultat = new TopRezultat( uporabnik, rezultat );

                    lista.Add( topRezultat );
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
        /// Dodavanje novega top rezultata
        /// </summary>
        /// <param name="rezultat">Objekt z rezultatom kateroga hočemo dodati v bazo</param>
        /// <returns></returns>
        public static bool Dodaj( TopRezultat rezultat )
        {
            SqlConnection con = new SqlConnection( Nastavitve.GetConnectionString() );
            SqlCommand cmd = new SqlCommand();

            string into = "[StackDB].[dbo].[tblTopRezultati]";
            string insert = "INSERT INTO " + into + " (IdUporabnika, Rezultat) VALUES (" +
                rezultat.Uporabnik + ", " + rezultat.Rezultat + ");";

            cmd.CommandText = insert;
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
        /// Brisanje rezultata iz baze
        /// </summary>
        /// <param name="idDosezka">Id rezultata kateroga brišemo</param>
        /// <returns></returns>
        public static bool Brisanje( int idRezultata )
        {
            SqlConnection con = new SqlConnection( Nastavitve.GetConnectionString() );
            SqlCommand cmd = new SqlCommand();

            string from = "[StackDB].[dbo].[tblTopRezultati]";
            string delete = "DELETE FROM " + from + " WHERE Id=" + idRezultata;

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
        /// Brisanje vsih rezultata kateri nisu med najboljšima
        /// </summary>
        /// <param name="steviloRezultata">Kolko najboljših rezultata želimo ostaviti</param>
        /// <returns></returns>
        public static bool BrisanjeSlabih( int steviloRezultata )
        {
            SqlConnection con = new SqlConnection( Nastavitve.GetConnectionString() );
            SqlCommand cmd = new SqlCommand();

            string from = "[StackDB].[dbo].[tblTopRezultati]";
            string delete = "DELETE FROM " + from + " WHERE Rezultat < " +
                "(SELECT MIN(Rezultat) FROM " +
                "(SELECT TOP " + steviloRezultata + " Rezultat " +
                "FROM " + from + " ORDER BY Rezultat DESC) AS Reze)";

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

    }

}
