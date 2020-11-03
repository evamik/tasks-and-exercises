using System;

namespace Individuoli_uzduotis
{
    class Zaidejas // Pervardinta iš "Žaidėjai" į "Žaidėjas"
    {
        public string Vardas { get; private set; }
        public string Pavarde { get; private set; }
        public string Pavadinimas { get; private set; }
        public string Pozicija { get; private set; }
        public string Cempionas { get; private set; }
        public int Sunaikinimai { get; private set; }
        public int Asistai { get; private set; }

        public Zaidejas()
        {
            Sunaikinimai = 0;
            Asistai = 0;
        }

        public Zaidejas(string vardas, string pavarde, string pavadinimas, string pozicija, string cempionas, int sunaikinimai, int asistai)
        {
            Vardas = vardas;
            Pavarde = pavarde;
            Pavadinimas = pavadinimas;
            Pozicija = pozicija;
            Cempionas = cempionas;
            Sunaikinimai = sunaikinimai;
            Asistai = asistai;
        }

        static public bool operator ==(Zaidejas zaidejas1, Zaidejas zaidejas2)
        {
            if (zaidejas1.Vardas == zaidejas2.Vardas && zaidejas1.Pavarde == zaidejas2.Pavarde)
                return true;
            return false;
        }

        static public bool operator !=(Zaidejas zaidejas1, Zaidejas zaidejas2)
        {
            if (zaidejas1.Vardas != zaidejas2.Vardas || zaidejas1.Pavarde != zaidejas2.Pavarde)
                return true;
            return false;
        }

        static public Zaidejas operator +(Zaidejas zaidejas1, Zaidejas zaidejas2)
        {
            Zaidejas zaidejas = zaidejas1;
            zaidejas.Sunaikinimai += zaidejas2.Sunaikinimai;
            zaidejas.Asistai += zaidejas2.Asistai;
            return zaidejas;
        }

        static public bool operator >(Zaidejas zaidejas1, Zaidejas zaidejas2)
        {
            int taskai1 = zaidejas1.Sunaikinimai + zaidejas1.Asistai;
            int taskai2 = zaidejas2.Sunaikinimai + zaidejas2.Asistai;
            if (taskai1 > taskai2)
                return true;
            return false;
        }

        static public bool operator <(Zaidejas zaidejas1, Zaidejas zaidejas2)
        {
            int taskai1 = zaidejas1.Sunaikinimai + zaidejas1.Asistai;
            int taskai2 = zaidejas2.Sunaikinimai + zaidejas2.Asistai;
            if (taskai1 < taskai2)
                return true;
            return false;
        }

        public override String ToString()
        {
            return String.Format("| {0, -12} | {1, -12} | {2, -10} | {3, -7} | {4, -10} | {5, 3} | {6, 3} |",
                Vardas, Pavarde, Pavadinimas, Pozicija, Cempionas, Sunaikinimai, Asistai);
        }

        public String ToStringShort()
        {
            return String.Format("{0} {1}",
                Vardas, Pavarde);
        }
    }
}
