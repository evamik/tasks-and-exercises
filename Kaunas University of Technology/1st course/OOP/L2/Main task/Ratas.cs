namespace Individuoli_uzduotis
{

    class Ratas
    {
        private Zaidejas[] Zaidejai;
        private int Kiekis;
        private int RatoNumeris;

        public int GautiKieki()
        {
            return Kiekis;
        }

        public Ratas(int dydis, int ratoNr)
        {
            Zaidejai = new Zaidejas[dydis];
            Kiekis = 0;
            RatoNumeris = ratoNr;
        }

        public void Prideti(Zaidejas zaidejas)
        {
            Zaidejai[Kiekis++] = zaidejas;
        }

        public int Numeris()
        {
            return RatoNumeris;
        }

        public Zaidejas Zaidejas(int i)
        {
            return Zaidejai[i];
        }
    }
}
