using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace L2
{
    public partial class Forma : System.Web.UI.Page
    {
        const string duom1 = "U8a.txt";
        const string duom2 = "U8b.txt";
        const string rez = "Rezultatai.txt";
        const int lentele1Dydis = 79;
        const int lentele2Dydis = 34;

        private static Marsrutai galimiMarsrutai;
        private static Marsrutai rastiMarsrutai;
        private static int kiekis = 0;
        private static Miestai miestai;
        private static string PradinisMiestas;
        private static float MaziausiasAtstumas;
        private static int DaugiausiaiGyventoju;
        private static bool ieskota = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (galimiMarsrutai == null || miestai == null)
                return;
            galimiMarsrutai.Pradzia();
            miestai.Pradzia();

            if (galimiMarsrutai.Yra() && miestai.Yra())
            {
                LabelDuom1_1.Text = "Kiekis: " + kiekis;
                LabelDuom1_2.Text = "Pradinis miestas: " + PradinisMiestas;
                RodytiMarsrutus(LenteleDuom1, galimiMarsrutai);
                RodytiMiestus(LenteleDuom2, miestai);
                IssaugotiDuomenis(rez, galimiMarsrutai, miestai);
                
                PanelIeskoti.Visible = true;
            }

            if (rastiMarsrutai == null)
                return;
            rastiMarsrutai.Pradzia();

            if (rastiMarsrutai.Yra())
            {
                RodytiMarsrutus(LenteleRez, rastiMarsrutai);
                IssaugotiRezultatus(rez, rastiMarsrutai);

                PanelRasti.Visible = true;
            }
            else if (ieskota) LabelNeraRastu.Visible = true;
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            galimiMarsrutai = new Marsrutai();
            rastiMarsrutai = null;
            miestai = new Miestai();

            // Duomenų nuskaitymas
            NuskaitytiMarsrutus(duom1, galimiMarsrutai, out PradinisMiestas);
            NuskaitytiMiestus(duom2, miestai);

            // Duomenų saugojimas į failą
            IssaugotiDuomenis(rez, galimiMarsrutai, miestai);

            // Kelionės tikslų apdorojimas
            ApdorotiTikslus(miestai, galimiMarsrutai);

            Response.Redirect(Request.RawUrl);
        }

        protected void ApdorotiTikslus(Miestai miestai, Marsrutai marsrutai)
        {
            // Kiekvienam miestui
            for(miestai.Pradzia(); miestai.Yra(); miestai.Kitas())
            {
                Miestas miestas = miestai.Imti();
                // Kiekvienam maršrutui
                for(marsrutai.Pradzia(); marsrutai.Yra(); marsrutai.Kitas())
                {
                    Marsrutas marsrutas = marsrutai.Imti();
                    string[] pavadinimai = marsrutas.Kelias.Replace(", ", ",").Split(',');
                    
                    if(miestas.Pavadinimas == pavadinimai[0])
                    {
                        miestai.Yra(pavadinimai[1]);
                        KelionesTikslas tikslas = new KelionesTikslas(pavadinimai[1], miestai.Rastas().GyventojuSk, marsrutas.Atstumas);
                        miestas.Deti(tikslas);
                    }
                    else if (miestas.Pavadinimas == pavadinimai[1])
                    {
                        miestai.Yra(pavadinimai[0]);
                        KelionesTikslas tikslas = new KelionesTikslas(pavadinimai[0], miestai.Rastas().GyventojuSk, marsrutas.Atstumas);
                        miestas.Deti(tikslas);
                    }
                }
            }
        }

        /// <summary>
        /// Išsaugo visus duomenis lentelės forma į failą
        /// </summary>
        /// <param name="failas"> failo pavadinimas </param>
        /// <param name="marsrutai"> maršrutų sąrašo klasė </param>
        protected void IssaugotiDuomenis(string failas, Marsrutai marsrutai, Miestai miestai)
        {
            File.Delete(Server.MapPath("~/" + failas));
            using (var fr = File.AppendText(Server.MapPath("~/" + failas)))
            {
                fr.WriteLine("Pradiniai duomenys:\n");
                fr.WriteLine(duom1);
                fr.WriteLine(kiekis);
                fr.WriteLine(PradinisMiestas);
            }
            IssaugotiMarsrutus(failas, marsrutai);

            using (var fr = File.AppendText(Server.MapPath("~/" + failas)))
            {
                fr.WriteLine();
                fr.WriteLine(duom2);
            }
            IssaugotiMiestus(failas, miestai);
        }

        /// <summary>
        /// Išsaugo visus rezultatus lentelės forma į failą
        /// </summary>
        /// <param name="failas"> failo pavadinimas </param>
        /// <param name="marsrutai"> maršrutų sąrašo klasė </param>
        protected void IssaugotiRezultatus(string failas, Marsrutai marsrutai)
        {
            using (var fr = File.AppendText(Server.MapPath("~/" + failas)))
            {
                fr.WriteLine();
                fr.WriteLine("Rezultatai:\n");
                fr.WriteLine("Mažiausias atstumas: " + MaziausiasAtstumas);
                fr.WriteLine("Daugiausiai gyventojų: " + DaugiausiaiGyventoju);
            }
            IssaugotiMarsrutus(failas, marsrutai);
        }

        /// <summary>
        /// Iš failo nuskaito miestus ir atstumą tarp jų, ir sudeda į sąrašo klasę
        /// </summary>
        /// <param name="failas"> failo pavadinimas </param>
        /// <param name="marsrutai"> maršrutų sąrašo klasė </param>
        protected void NuskaitytiMarsrutus(string failas, Marsrutai marsrutai, out string pradzia)
        {
            string[] eilutes = File.ReadAllLines(Server.MapPath("~/" + failas));

            kiekis = int.Parse(eilutes[0]);
            pradzia = eilutes[1];

            for(int i = 0; i < kiekis; i++)
            {
                string[] duomenys = eilutes[i+2].Split(',');
                string miestas1 = duomenys[0].Trim();
                string miestas2 = duomenys[1].Trim();
                float atstumas = float.Parse(duomenys[2]);

                Marsrutas marsrutas = new Marsrutas(miestas1);
                marsrutas.Deti(miestas2, atstumas);
                marsrutai.Deti(marsrutas);
            }
        }

        /// <summary>
        /// Iš failo nuskaito miestus ir jų gyventojų skaičius, ir sudeda į sarašo klasę
        /// </summary>
        /// <param name="failas"> failo pavadinimas </param>
        /// <param name="miestai"> miestų sąrašo klasė </param>
        protected void NuskaitytiMiestus(string failas, Miestai miestai)
        {
            string[] eilutes = File.ReadAllLines(Server.MapPath("~/" + failas));

            for (int i = 0; i < eilutes.Length; i++)
            {
                string[] duomenys = eilutes[i].Split(',');
                string pavadinimas = duomenys[0].Trim();
                int gyventojuSk = int.Parse(duomenys[1]);

                Miestas miestas = new Miestas(pavadinimas, gyventojuSk);
                miestai.Deti(miestas);
            }
        }

        /// <summary>
        /// Išsaugo maršrutus lentelės forma į failą
        /// </summary>
        /// <param name="failas"> failo pavadinimas </param>
        /// <param name="marsrutai"> maršrutų sąrašo klasė </param>
        protected void IssaugotiMarsrutus(string failas, Marsrutai marsrutai)
        {
            using(var fr = File.AppendText(Server.MapPath("~/" + failas)))
            {
                fr.WriteLine(Juosta1());
                fr.WriteLine(string.Format("| {0, -60} | Atstumas, km |", "Maršrutai"));
                fr.WriteLine(Juosta1());
                for (marsrutai.Pradzia(); marsrutai.Yra(); marsrutai.Kitas())
                {
                    fr.WriteLine(marsrutai.Imti().ToString());
                    fr.WriteLine(Juosta1());
                }
            }
        }

        /// <summary>
        /// Išsaugo maršrutus lentelės forma į failą
        /// </summary>
        /// <param name="failas"> failo pavadinimas </param>
        /// <param name="marsrutai"> maršrutų sąrašo klasė </param>
        protected void IssaugotiMiestus(string failas, Miestai miestai)
        {
            using (var fr = File.AppendText(Server.MapPath("~/" + failas)))
            {
                fr.WriteLine(Juosta2());
                fr.WriteLine(string.Format("| {0, -14} | {1, 13} |", "Miestas", "Gyventojų sk."));
                fr.WriteLine(Juosta2());
                for (miestai.Pradzia(); miestai.Yra(); miestai.Kitas())
                {
                    fr.WriteLine(miestai.Imti().ToString());
                    fr.WriteLine(Juosta2());
                }
            }
        }

        /// <summary>
        /// Parodo maršrutų duomenis lentelėje
        /// </summary>
        /// <param name="lentele"> lentelė </param>
        /// <param name="uzrasas"> užrašas prie lentelės </param>
        /// <param name="marsrutai"> maršrutų sąrašo klasė </param>
        protected static void RodytiMarsrutus(Table lentele, Marsrutai marsrutai)
        {
            LentelėsAntraste(lentele, new string[] { "Maršrutai", "Atstumas, km" }, new int[] { 500, 100 });
            for(marsrutai.Pradzia(); marsrutai.Yra(); marsrutai.Kitas())
            {
                Marsrutas mar = marsrutai.Imti();

                TableRow row = new TableRow();

                TableCell cell = new TableCell();
                cell.Text = mar.Kelias;
                cell.Attributes.Add("style", "width: " + 500 + "px;");
                row.Cells.Add(cell);

                TableCell cell2 = new TableCell();
                cell2.Text =  mar.Atstumas.ToString();
                cell2.Attributes.Add("style", "width: " + 100 + "px; text-align: right");
                row.Cells.Add(cell2);

                row.Height = 25;
                lentele.Rows.Add(row);
            }
        }

        /// <summary>
        /// Parodo miestų duomenis lentelėje
        /// </summary>
        /// <param name="lentele"> lentelė </param>
        /// <param name="uzrasas"> užrašas prie lentelės </param>
        /// <param name="marsrutai"> miestų sąrašo klasė </param>
        protected static void RodytiMiestus(Table lentele, Miestai miestai)
        {
            LentelėsAntraste(lentele, new string[] { "Miestas", "Gyventojų sk." }, new int[] { 200, 100 });
            for (miestai.Pradzia(); miestai.Yra(); miestai.Kitas())
            {
                Miestas miestas = miestai.Imti();

                TableRow row = new TableRow();

                TableCell cell = new TableCell();
                cell.Text = miestas.Pavadinimas;
                cell.Attributes.Add("style", "width: " + 200 + "px;");
                row.Cells.Add(cell);

                TableCell cell2 = new TableCell();
                cell2.Text = miestas.GyventojuSk.ToString();
                cell2.Attributes.Add("style", "width: " + 100 + "px; text-align: right");
                row.Cells.Add(cell2);

                row.Height = 25;
                lentele.Rows.Add(row);
            }
        }

        /// <summary>
        /// Įterpia lentelės antraštę į lentelę
        /// </summary>
        /// <param name="lentele"> lentelė </param>
        /// <param name="duomenys"> antraštės pavadinimai </param>
        /// <param name="plociai"> lentelės stulpelių pločiai </param>
        protected static void LentelėsAntraste(Table lentele, string[] duomenys, int[] plociai)
        {
            TableRow row = new TableRow();

            for (int i = 0; i < duomenys.Length; i++)
                PridetiStulpeli(row, duomenys[i], plociai[i]);

            row.Height = 30;
            lentele.Rows.Add(row);
        }

        /// <summary>
        /// Įterpia stulpelį į lentelės eilutę
        /// </summary>
        /// <param name="row"> lentelės eilutė </param>
        /// <param name="tekstas"> tekstas </param>
        /// <param name="width"> plotis </param>
        protected static void PridetiStulpeli(TableRow row, string tekstas, int width = 100)
        {
            TableCell cell = new TableCell();
            cell.Text = "" + tekstas + "";
            cell.Attributes.Add("style", "width: " + width + "px; font-weight: bold");
            row.Cells.Add(cell);
        }

        /// <summary>
        /// Gražina lentelė1 minusų juostą
        /// </summary>
        /// <returns> minusų juostos eilutė </returns>
        protected static string Juosta1()
        {
            return "".PadRight(lentele1Dydis, '-');
        }

        /// <summary>
        /// Gražina lentelė2 dydžio minusų juostą
        /// </summary>
        /// <returns> minusų juostos eilutė </returns>
        protected static string Juosta2()
        {
            return "".PadRight(lentele2Dydis, '-');
        }

        protected void ButtonIeskoti_Click(object sender, EventArgs e)
        {
            Page.Validate("Ieskoti");
            if (!Page.IsValid)
                return;

            rastiMarsrutai = new Marsrutai();
            Marsrutas marsrutas = new Marsrutas(PradinisMiestas);
            if(miestai.Yra(PradinisMiestas))
                MarsrutuPaieska(marsrutas, miestai.Rastas().KelTikslai);
            rastiMarsrutai.Rikiuoti();
            ieskota = true;

            Response.Redirect(Request.RawUrl);
        }

        /// <summary>
        /// Rekursijos būdu ieško maršrutų tarp visų galimų kelių
        /// </summary>
        /// <param name="marsrutas"> maršruto klasė </param>
        /// <param name="tikslai"> kelionės tiklsų sąrašas </param>
        /// <returns></returns>
        protected bool MarsrutuPaieska(Marsrutas marsrutas, Tikslai tikslai)
        {
            bool rasta = false;
            for(tikslai.Pradzia(); tikslai.Yra(); tikslai.Kitas())
            {
                KelionesTikslas tikslas = tikslai.Imti();
                if(tikslas.GyventojuSk <= DaugiausiaiGyventoju && !marsrutas.Yra(tikslas.Pavadinimas))
                {
                    rasta = true;
                    Marsrutas naujasMarsrutas = new Marsrutas(marsrutas.Kelias, marsrutas.Atstumas);
                    naujasMarsrutas.Deti(tikslas.Pavadinimas, tikslas.Atstumas);
                    if (miestai.Yra(tikslas.Pavadinimas))
                        MarsrutuPaieska(naujasMarsrutas, miestai.Rastas().KelTikslai);
                }
            }
            if (!rasta && marsrutas.Atstumas >= MaziausiasAtstumas)
                rastiMarsrutai.Deti(marsrutas);

            return false;
        }

        protected void ButtonIsbraukti_Click(object sender, EventArgs e)
        {
            Page.Validate("Isbraukti");
            if (!Page.IsValid)
                return;

            string nenorimas = TextBoxIsbraukti.Text;
            for(rastiMarsrutai.Pradzia(); rastiMarsrutai.Yra(); rastiMarsrutai.Kitas())
            {
                while (rastiMarsrutai.Yra() && rastiMarsrutai.Imti().Yra(nenorimas))
                    rastiMarsrutai.Salinti();
            }


            Response.Redirect(Request.RawUrl);
        }

        protected void CustomValidatorAtstumas_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (float.TryParse(TextBoxMinimum.Text, out MaziausiasAtstumas))
                args.IsValid = true;
            else args.IsValid = false;
        }

        protected void CustomValidatorGyventojai_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (int.TryParse(TextBoxGyvSk.Text, out DaugiausiaiGyventoju))
                args.IsValid = true;
            else args.IsValid = false;
        }

        protected void CustomValidatorIsbraukti_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (miestai.Yra(args.Value))
                args.IsValid = true;
            else args.IsValid = false;
        }
    }
}