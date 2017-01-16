using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace StackClone
{
    public class Dosezek
    {
        public Dosezek() { }

        public Dosezek(int id, string naziv, int nagrada, string opis)
        {
            Id = id;
            Naziv = naziv;
            Nagrada = nagrada;
            Opis = opis;
        }

        public int Id { get; set; } = -1;

        public string Naziv { get; set; } = "";

        public int Nagrada { get; set; } = -1;

        public string Opis { get; set; } = "";

        /// <summary>
        /// Brskanje dosezkov za pregled
        /// </summary>
        /// <param name="naziv">brskanje po nazivu dosezka</param>
        /// <param name="nagradaMin">donja granica vrednosti nagrade</param>
        /// <param name="nagradaMax">zgornja granica vrednosti nagrade</param>
        /// <returns>seznam dosezkov</returns>
        public static List<Dosezek> Brskaj( string naziv = "", int Id = -1, int nagradaMin = -1, int nagradaMax = -1, int idUporabnika = -1, string uporabnisko = "" )
        {
            SqlConnection con = new SqlConnection( Nastavitve.GetConnectionString() );
            SqlCommand cmd = new SqlCommand();

            string where = "1 = 1 ";
            where += (Id != -1) ? ("AND (Id = " + Id + ") ") : ("");
            where += (naziv.Trim() != "") ? ("AND (Naziv = '" + naziv + "') ") : ("");
            where += (nagradaMin != -1) ? ("AND (Nagrada > " + nagradaMin + ") ") : ("");
            where += (nagradaMax != -1) ? ("AND (Nagrada < " + nagradaMax + ") ") : ("");
            where += (idUporabnika != -1) ? ("AND (UporabnikId = " + idUporabnika + ") ") : ("");
            where += (uporabnisko != "") ? ("AND (Uporabnik = '" + uporabnisko + "') ") : ("");

            string from = "[StackDB].[dbo].[tblDosezki]";
            //                 0    1    2       3      4
            string select = "SELECT Id, Naziv, Nagrada, opis FROM " + from + " WHERE " + where;


            // če se brska za nekaterog uporabnika
            if ( idUporabnika != -1 || uporabnisko != "" )
            {
                from = "[StackDB].[dbo].[viewDosezki]";
                //          0        1       2        3      4
                select = "SELECT DosezekId, Naziv, Nagrada, opis FROM " + from + " WHERE " + where;
            }

            cmd.CommandText = select;
            cmd.Connection = con;

            List<Dosezek> lista = new List<Dosezek>();

            try
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while ( reader.Read() )
                {
                    int id = reader.GetInt32( 0 );
                    string nazivDosezka = reader.IsDBNull( 1 ) ? "" : reader.GetString( 1 ).Trim();
                    int nagrada = reader.IsDBNull( 2 ) ? -1 : reader.GetInt32( 2 );
                    string Opis = reader.IsDBNull(3) ? "" : reader.GetString(3).Trim();

                    Dosezek enota = new Dosezek( id, nazivDosezka, nagrada, Opis );

                    lista.Add( enota );
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
        /// Dodavanje novih dosežkov
        /// </summary>
        /// <param name="dosezek">Objekt s dosežkom kateroga hočemo dodati v bazo</param>
        /// <returns></returns>
        public static bool Dodaj( Dosezek dosezek )
        {
            SqlConnection con = new SqlConnection( Nastavitve.GetConnectionString() );
            SqlCommand cmd = new SqlCommand();

            string into = "[StackDB].[dbo].[tblDosezki]";
            string insert = "INSERT INTO " + into + " (Naziv, Nagrada, opis) VALUES ('" +
                dosezek.Naziv + "', '" + dosezek.Nagrada + "', '" + dosezek.Opis + "');";

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
        /// Dodavanje novog dosežka za uporabnika
        /// </summary>
        public static bool DodajDosezekUporabnika( int idUporabnika, int idDosezka )
        {
            SqlConnection con = new SqlConnection( Nastavitve.GetConnectionString() );
            SqlCommand cmd = new SqlCommand();

            string into = "[StackDB].[dbo].[tblDosezkiUporabnikov]";
            string insert = "INSERT INTO " + into + " (IdUporabnika, IdDosezka) VALUES (" +
                idUporabnika + ", " + idDosezka + ");";

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
        /// Seznam dosezkov katere je uporabnik odljučal
        /// </summary>
        /// <returns>seznam dosezkov</returns>
        public static List<int> UporabnikMaDosezek( int idUporabnika )
        {
            SqlConnection con = new SqlConnection( Nastavitve.GetConnectionString() );
            SqlCommand cmd = new SqlCommand();

            string where = "1 = 1 ";
            where += (idUporabnika != -1) ? ("AND (IdUporabnika = " + idUporabnika + ") ") : ("");

            string from = "[StackDB].[dbo].[tblDosezkiUporabnikov]";
            //                           0             1
            string select = "SELECT IdUporabnika, idDosezka FROM " + from + " WHERE " + where;


            cmd.CommandText = select;
            cmd.Connection = con;

            List<int> lista = new List<int>();

            try
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while ( reader.Read() )
                {
                    int idDosezka = reader.GetInt32( 1 );

                    lista.Add( idDosezka );
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
        /// Brisanje dosežka iz baze
        /// </summary>
        /// <param name="idDosezka">Id dosežka kateroga brišemo</param>
        /// <returns></returns>
        public static bool Brisanje( int idDosezka )
        {
            SqlConnection con = new SqlConnection( Nastavitve.GetConnectionString() );
            SqlCommand cmd = new SqlCommand();

            string from = "[StackDB].[dbo].[tblDosezki]";
            string delete = "DELETE FROM " + from + " WHERE Id=" + idDosezka;

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
