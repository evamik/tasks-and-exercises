using System;
using System.IO;
using System.Text;
using System.Web.UI.WebControls;

namespace Lab1
{
    public partial class forma : System.Web.UI.Page
    {
        //private const string Duomenys = "U3.txt";
        private const string Duomenys = "U32.txt";
        private const string RezFailas = "Rezultatai.txt";
        private const int Plotis1 = 13;
        private const int Plotis2 = 15;

        private Virsunes kaimynai;
        private Virsunes kiti;

        private int geluonis;
        private int uodega;
        private int liemuo;
        private Virsunes kojos;

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            char[,] briaunos;
            Nuskaitymas(out briaunos, Duomenys);
            int kiekis = (int)Math.Sqrt(briaunos.Length);

            Skorpionas(briaunos, kiekis);
            File.Delete(Server.MapPath("~/" + RezFailas));
            IssaugotiDuomenis(briaunos, kiekis);
            IssaugotiRezultatus(Plotis1, Plotis2);
            RodytiDuomenis(briaunos, kiekis);
        }

        protected void Skorpionas(char[,] briaunos, int kiekis)
        {
            Table1.Rows.Clear();
            kaimynai = new Virsunes(kiekis);
            kiti = new Virsunes(kiekis);
            if (!Virsune(briaunos, kiekis))
            {
                Label1.Text = "Grafas nėra \"skorpionas\"";
            }
            else
            {
                Label1.Text = "Grafas yra \"skorpionas\"";
                IterptiIrasa("Geluonis: ", (geluonis + 1) + " viršunė");
                IterptiIrasa("Uodega: ", (uodega + 1) + " viršunė");
                IterptiIrasa("Liemuo: ", (liemuo + 1) + " viršunė");
                for (int i = 0; i < kojos.Kiekis; i++)
                {

                    IterptiIrasa((i+1) + " koja:", (kojos.Imti(i)+1) + " viršunė");
                }
            }
        }

        /// <summary>
        /// Užpildo Table2 lentelę duomenimis
        /// </summary>
        protected void RodytiDuomenis(char[,] briaunos, int kiekis)
        {
            IterptiIrasa("Duomenys:");
            IterptiIrasa(kiekis.ToString());
            for(int i = 0; i < kiekis; i++)
            {
                StringBuilder sb = new StringBuilder();
                for(int j = 0; j < kiekis; j++)
                {
                    sb.Append(briaunos[i, j]);
                    sb.Append("&nbsp;&nbsp;&nbsp;");
                }
                IterptiIrasa(sb.ToString());
            }
        }

        /// <summary>
        /// Išsaugo duomenis
        /// </summary>
        protected void IssaugotiDuomenis(char[,] briaunos, int kiekis)
        {
            using (var failas = File.AppendText(Server.MapPath("~/" + RezFailas)))
            {
                failas.WriteLine(kiekis.ToString());
                for (int i = 0; i < kiekis; i++)
                {
                    for (int j = 0; j < kiekis; j++)
                    {
                        failas.Write(briaunos[i, j] + " ");
                    }
                    failas.WriteLine();
                }
                failas.WriteLine();
            }
        }

        /// <summary>
        /// Išsaugo rezultatus į failą RezFailas
        /// </summary>
        /// <param name="plotis1"> pirmo stulpelio plotis simboliais </param>
        /// <param name="plotis2"> antro stulpelio plotis simboliais </param>
        protected void IssaugotiRezultatus(int plotis1, int plotis2)
        {
            int eiluciu = Table1.Rows.Count;

            using (var failas = File.AppendText(Server.MapPath("~/" + RezFailas)))
            {
                failas.WriteLine(Label1.Text);

                if (eiluciu > 0)
                {
                    failas.WriteLine("\n".PadRight(plotis1 + plotis2 + 3, '-'));

                    for (int i = 0; i < eiluciu; i++)
                    {
                        TableCellCollection cells = Table1.Rows[i].Cells;
                        failas.Write("| ");
                        failas.Write(cells[0].Text.PadRight(plotis1 - 2));
                        failas.Write(" | ");
                        failas.Write(cells[1].Text.PadLeft(plotis2 - 2));
                        failas.WriteLine(" |");

                        failas.WriteLine("".PadRight(plotis1 + plotis2 + 3, '-'));
                    }
                }
            }
        }

        /// <summary>
        /// Įterpia duomenis į Table2 lentelę
        /// </summary>
        /// <param name="tekstas"> eilutės tekstas </param>
        protected void IterptiIrasa(string tekstas)
        {
            TableCell cell = new TableCell();
            TableRow row = new TableRow();
            cell.Text = tekstas;
            row.Cells.Add(cell);
            Table2.Rows.Add(row);
        }

        /// <summary>
        /// Įterpia duomenis į Table1 lentelę
        /// </summary>
        /// <param name="tekstas"> pirmo stulpelio eilutės tekstas</param>
        /// <param name="tekstas2"> antro stulpelio eilutės tekstas</param>
        protected void IterptiIrasa(string tekstas, string tekstas2)
        {
            TableCell cell = new TableCell();
            TableRow row = new TableRow();
            cell.Text = tekstas;
            row.Cells.Add(cell);
            TableCell cell2 = new TableCell();
            cell2.Text = tekstas2;
            row.Cells.Add(cell2);
            Table1.Rows.Add(row);
        }

        /// <summary>
        /// Iškviečia Virsune metodą su pradiniu indeksu 0 ir pritaiko rekursiją
        /// </summary>
        /// <returns> Ar skorpionas </returns>
        protected bool Virsune(char[,] briaunos, int kiekis)
        {
            return Virsune(briaunos, kiekis, 0, true);
        }

        /// <summary>
        /// Nustato ar duomenų matrica apsako "skorpioną"
        /// </summary>
        /// <param name="i"> Viršūnės indeksas </param>
        /// <param name="kartoti"> Ar naudoti rekursiją </param>
        /// <returns> Ar skorpionas </returns>
        protected bool Virsune(char[,] briaunos, int kiekis, int i, bool kartoti = false)
        {
            // jei patikrintos visos viršūnės baigti
            if (i >= kiekis)
                return false;

            // išvalyti viršūnių sarašus
            kaimynai = new Virsunes(kiekis);
            kiti = new Virsunes(kiekis);

            // sudėti viršūnės į kaimynus/kitus sarašus
            for (int j = 0; j < kiekis; j++)
            {
                if (briaunos[i, j] == '+')
                    kaimynai.Prideti(j);
                else if (briaunos[i, j] == '-')
                    kiti.Prideti(j);
            }
            int n = kaimynai.Kiekis;

            // jei yra viršūnė su 0 jungčių arba yra viršūnė sujungta 
            // su visomis kitomis, tai ne skorpionas
            if (n == 0 || n == kiekis - 1)
                return false;

            // jei viršūnė turi kiekis-2 jungčių, tai yra liemuo ir galima rasti
            // kitas viršūnęs
            else if (n == kiekis - 2)
            {
                if (!kartoti)
                    return false;
                
                liemuo = i;
                if (kiti.Kiekis != 1)
                    return false;
                
                geluonis = kiti.Imti(0);
                Virsune(briaunos, kiekis, geluonis);
                if (kaimynai.Kiekis != 1)
                    return false;
                
                uodega = kaimynai.Imti(0);
                Virsune(briaunos, kiekis, uodega);
                if (kaimynai.Kiekis != 2)
                    return false;

                kojos = kiti;
                for (int j = 0; j < kojos.Kiekis; j++)
                {
                    Virsune(briaunos, kiekis, kojos.Imti(j));
                    if (kaimynai.Kiekis < 1 && kaimynai.Kiekis >= kiekis - 2)
                        return false;
                }
                return true;
            }

            if (kartoti)
                return Virsune(briaunos, kiekis, i + 1, kartoti);
            else return true;
        }


        /// <summary>
        /// Nuskaito viršūnių matricą iš duomenų failo
        /// </summary>
        /// <param name="briaunos"> briaunų matrica </param>
        /// <param name="failas"> duomenų failas </param>
        protected void Nuskaitymas(out char[,] briaunos, string failas)
        {
            string[] duomenys = File.ReadAllLines(Server.MapPath("~/"+failas));

            int kiekis = int.Parse(duomenys[0].Substring(0, 1));

            briaunos = new char[kiekis,kiekis];

            for(int i = 0; i < kiekis; i++)
                for(int j = 0; j < kiekis; j++)
                    briaunos[i, j] = duomenys[i+1][j];
        }
    }
}