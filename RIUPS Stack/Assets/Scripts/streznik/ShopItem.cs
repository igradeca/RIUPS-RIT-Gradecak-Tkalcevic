using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace StackClone
{
    class ShopItem
    {
        public ShopItem() { }

        public ShopItem( int id, string naziv, int cijena )
        {
            Id = id;
            Naziv = naziv;
            Cijena = cijena;
        }

        public int Id { get; set; } = -1;
        public string Naziv { get; set; } = "";
        public int Cijena { get; set; } = 1;


        /// <summary>
        /// Dodavanje novih ship itema
        /// </summary>
        /// <param name="shop item">Objekt s shop itemom kateroga hočemo dodati v bazo</param>
        /// <returns></returns>
        public static bool Dodaj( ShopItem item )
        {
            SqlConnection con = new SqlConnection( Nastavitve.GetConnectionString() );
            SqlCommand cmd = new SqlCommand();

            string into = "[StackDB].[dbo].[tblShop]";
            string insert = "INSERT INTO " + into + " (Naziv, Cijena) VALUES ('"
                + item.Naziv + "', " + item.Cijena + ");";

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
        /// Dodavanje novog shop itema za uporabnika
        /// </summary>
        public static bool DodajShopItemZaUporabnika( int idUporabnika, int idShopItema )
        {
            SqlConnection con = new SqlConnection( Nastavitve.GetConnectionString() );
            SqlCommand cmd = new SqlCommand();

            string into = "[StackDB].[dbo].[tblKupljeniArtikli]";
            string insert = "INSERT INTO " + into + " (IdUporabnika, IdDosezka) VALUES (" +
                idUporabnika + ", " + idShopItema + ");";

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
        /// Seznam shop itema katere je uporabnik odljučal
        /// </summary>
        /// <returns>seznam shop itema</returns>
        public static List<ShopItem> Brskaj()
        {
            SqlConnection con = new SqlConnection( Nastavitve.GetConnectionString() );
            SqlCommand cmd = new SqlCommand();

            string from = "[StackDB].[dbo].[tblShop]";
            //                       0    1
            string select = "SELECT Id, Naziv, Cijena FROM " + from;

            cmd.CommandText = select;
            cmd.Connection = con;

            List<ShopItem> lista = new List<ShopItem>();

            try
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while ( reader.Read() )
                {
                    int idItema = reader.GetInt32( 0 );
                    string naziv = reader.GetString( 1 );
                    int cijena = reader.GetInt32( 2 );

                    lista.Add( new ShopItem(idItema, naziv.Trim(), cijena ));
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
        /// Seznam shop itema katere je uporabnik odljučal
        /// </summary>
        /// <returns>seznam shop itema</returns>
        public static List<int> BrskajZaUporabnika( int idUporabnika )
        {
            SqlConnection con = new SqlConnection( Nastavitve.GetConnectionString() );
            SqlCommand cmd = new SqlCommand();

            string where = "1 = 1 ";
            where += (idUporabnika != -1) ? ("AND (IdUporabnika = " + idUporabnika + ") ") : ("");

            string from = "[StackDB].[dbo].[tblKupljeniArtikli]";
            //                           0             1
            string select = "SELECT IdUporabnika, IdItema FROM " + from + " WHERE " + where;


            cmd.CommandText = select;
            cmd.Connection = con;

            List<int> lista = new List<int>();

            try
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while ( reader.Read() )
                {
                    int idItema = reader.GetInt32( 1 );

                    lista.Add( idItema );
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

    }
}
