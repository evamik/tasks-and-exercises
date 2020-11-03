namespace Lab1
{
    /// <summary>
    /// Viršūnių pozicijų masyvo konteinerinė klasė
    /// </summary>
    public class Virsunes
    {
        private int[] virsunes;
        public int Kiekis { get; private set; }

        public Virsunes(int dydis)
        {
            virsunes = new int[dydis];
            Kiekis = 0;
        }

        public void Prideti(int virsune)
        {
            virsunes[Kiekis++] = virsune;
        }

        public int Imti(int i)
        {
            return virsunes[i];
        }
    }
}