using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace StackClone
{
    public class RezultatiScript : MonoBehaviour
    {
        public Text h1;
        GameObject h2;
        GameObject h3;
        GameObject h4;
        GameObject h5;
        GameObject h6;
        GameObject h7;
        GameObject h8;
        GameObject h9;
        GameObject h10;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void PopulateHighscore()
        {
            List<TopRezultat> lista = new List<TopRezultat>();
            lista = TopRezultat.GetTopRezultati( 10 );
            
            if (lista.Count > 0) h1.text = lista[0].Uporabnik.Uporabnisko + " - " + lista[0].Rezultat;
            /*
            if ( lista.Count > 1 ) h2.Text = lista[1].Uporabnik.Uporabnisko + " - " + lista[1].Rezultat;
            if ( lista.Count > 2 ) h3.Text = lista[2].Uporabnik.Uporabnisko + " - " + lista[2].Rezultat;
            if ( lista.Count > 3 ) h4.Text = lista[3].Uporabnik.Uporabnisko + " - " + lista[3].Rezultat;
            if ( lista.Count > 4 ) h5.Text = lista[4].Uporabnik.Uporabnisko + " - " + lista[4].Rezultat;
            */
        }

    }
}