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
        const string rez = "Rezultatai.txt";
        const int lentele1Dydis = 79;
        const int lentele2Dydis = 34;

        private static MarsrutuSarasas galimiMarsrutai;
        private static MarsrutuSarasas rastiMarsrutai;
        private static int kiekis = 0;
        private static Miestai miestai;
        private static string PradinisMiestas;
        private static float MaziausiasAtstumas;
        private static int DaugiausiaiGyventoju;
        private static bool ieskota = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            DuomenuEilute.Visible = false;
            RezultatuEilute.Visible = false;
            if (galimiMarsrutai == null || miestai == null)
                return;
            galimiMarsrutai.Pradzia();
            miestai.Pradzia();

            // Jei nuskaityti duomenys
            if (galimiMarsrutai.Yra() && miestai.Yra())
            {
                DuomenuEilute.Visible = true;
                LabelDuom1_1.Text = "Kiekis: " + kiekis;
                LabelDuom1_2.Text = "Pradinis miestas: " + PradinisMiestas;
                RodytiMarsrutus(LenteleDuom1, galimiMarsrutai);
                RodytiMiestus(LenteleDuom2, miestai);
                IssaugotiDuomenis(rez, galimiMarsrutai, miestai);
                DuomenuEilute.Focus();
            }

            if (rastiMarsrutai == null)
                return;
            rastiMarsrutai.Pradzia();


            // Jei yra rezultatai
            if (rastiMarsrutai.Yra())
            {
                RezultatuEilute.Visible = true;
                RodytiMarsrutus(LenteleRez, rastiMarsrutai);
                IssaugotiRezultatus(rez, rastiMarsrutai);
                RezultatuEilute.Focus();
            }
            else if (ieskota) LabelNeraRastu.Visible = true;
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            if (!CustomValidator_Failas1.IsValid || !CustomValidator_Failas2.IsValid)
                return;

            galimiMarsrutai = new MarsrutuSarasas();

            rastiMarsrutai = null;
            miestai = new Miestai();

            // Duomenų nuskaitymas
            NuskaitytiMarsrutus(Failas1.FileContent, galimiMarsrutai, out PradinisMiestas);
            NuskaitytiMiestus(Failas2.FileContent, miestai);

            // Duomenų saugojimas į failą
            IssaugotiDuomenis(rez, galimiMarsrutai, miestai);

            // Kelionės tikslų apdorojimas
            ApdorotiTikslus(miestai, galimiMarsrutai);

            Response.Redirect(Request.RawUrl);
        }

        protected void ApdorotiTikslus(Miestai miestai, MarsrutuSarasas marsrutai)
        {
            // Kiekvienam miestui
            foreach(Miestas miestas in miestai)
            {
                // Kiekvienam maršrutui
                foreach(Marsrutas marsrutas in marsrutai)
                {
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
        protected void IssaugotiDuomenis(string failas, MarsrutuSarasas marsrutai, Miestai miestai)
        {
            File.Delete(Server.MapPath("~/" + failas));
            using (var fr = File.AppendText(Server.MapPath("~/" + failas)))
            {
                fr.WriteLine("Pradiniai duomenys:\n");
                fr.WriteLine(Failas1.FileName);
                fr.WriteLine(kiekis);
                fr.WriteLine(PradinisMiestas);
            }
            IssaugotiMarsrutus(failas, marsrutai);

            using (var fr = File.AppendText(Server.MapPath("~/" + failas)))
            {
                fr.WriteLine();
                fr.WriteLine(Failas2.FileName);
            }
            IssaugotiMiestus(failas, miestai);
        }

        /// <summary>
        /// Išsaugo visus rezultatus lentelės forma į failą
        /// </summary>
        /// <param name="failas"> failo pavadinimas </param>
        /// <param name="marsrutai"> maršrutų sąrašo klasė </param>
        protected void IssaugotiRezultatus(string failas, MarsrutuSarasas marsrutai)
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
        protected void NuskaitytiMarsrutus(Stream failas, MarsrutuSarasas marsrutai, out string pradzia)
        {
            using (StreamReader reader = new StreamReader(failas)) {
                kiekis = int.Parse(reader.ReadLine());
                pradzia = reader.ReadLine();

                string eilute;
                while ((eilute = reader.ReadLine()) != null)
                {
                    string[] duomenys = eilute.Split(',');
                    string miestas1 = duomenys[0].Trim();
                    string miestas2 = duomenys[1].Trim();
                    float atstumas = float.Parse(duomenys[2]);

                    Marsrutas marsrutas = new Marsrutas(miestas1);
                    marsrutas.Deti(miestas2, atstumas);
                    marsrutai.Deti(marsrutas);
                }
            }
        }

        /// <summary>
        /// Iš failo nuskaito miestus ir jų gyventojų skaičius, ir sudeda į sarašo klasę
        /// </summary>
        /// <param name="failas"> failo pavadinimas </param>
        /// <param name="miestai"> miestų sąrašo klasė </param>
        protected void NuskaitytiMiestus(Stream failas, Miestai miestai)
        {
            using (StreamReader reader = new StreamReader(failas))
            {
                string eilute;
                while ((eilute = reader.ReadLine()) != null)
                {
                    string[] duomenys = eilute.Split(',');
                    string pavadinimas = duomenys[0].Trim();
                    int gyventojuSk = int.Parse(duomenys[1]);

                    Miestas miestas = new Miestas(pavadinimas, gyventojuSk);
                    miestai.Deti(miestas);
                }
            }
        }

        /// <summary>
        /// Išsaugo maršrutus lentelės forma į failą
        /// </summary>
        /// <param name="failas"> failo pavadinimas </param>
        /// <param name="marsrutai"> maršrutų sąrašo klasė </param>
        protected void IssaugotiMarsrutus(string failas, MarsrutuSarasas marsrutai)
        {
            using(var fr = File.AppendText(Server.MapPath("~/" + failas)))
            {
                fr.WriteLine(Juosta1());
                fr.WriteLine(string.Format("| {0, -60} | Atstumas, km |", "Maršrutai"));
                fr.WriteLine(Juosta1());
                foreach (Marsrutas marsrutas in marsrutai)
                {
                    fr.WriteLine(marsrutas.ToString());
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
                foreach (Miestas miestas in miestai)
                {
                    fr.WriteLine(miestas.ToString());
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
        protected static void RodytiMarsrutus(Table lentele, MarsrutuSarasas marsrutai)
        {
            LentelėsAntraste(lentele, new string[] { "Maršrutai", "Atstumas, km" }, new int[] { 500, 100 });
            foreach(Marsrutas marsrutas in marsrutai)
            {
                TableRow row = new TableRow();

                TableCell cell = new TableCell();
                cell.Text = marsrutas.Kelias;
                cell.Attributes.Add("style", "width: " + 500 + "px;");
                row.Cells.Add(cell);

                TableCell cell2 = new TableCell();
                cell2.Text = marsrutas.Atstumas.ToString();
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
            foreach (Miestas miestas in miestai)
            {
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
            TableHeaderRow row = new TableHeaderRow();

            for (int i = 0; i < duomenys.Length; i++)
                PridetiStulpeli(row, duomenys[i], plociai[i]);

            row.Height = 30;
            lentele.Rows.AddAt(0, row);
        }

        /// <summary>
        /// Įterpia stulpelį į lentelės eilutę
        /// </summary>
        /// <param name="row"> lentelės eilutė </param>
        /// <param name="tekstas"> tekstas </param>
        /// <param name="width"> plotis </param>
        protected static void PridetiStulpeli(TableRow row, string tekstas, int width = 100)
        {
            TableHeaderCell cell = new TableHeaderCell();
            cell.Text = "" + tekstas + "";
            //cell.Attributes.Add("style", "width: " + width + "px; font-weight: bold");
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

            rastiMarsrutai = new MarsrutuSarasas();
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
            foreach(KelionesTikslas tikslas in tikslai)
            {
                //KelionesTikslas tikslas = tikslai.Imti();
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
            foreach(Marsrutas marsrutas in rastiMarsrutai)
            {
                if(marsrutas.Yra(nenorimas))
                    rastiMarsrutai.Pasalinti(marsrutas);
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

        protected void CustomValidator_Failas1_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (Failas1.HasFile && Failas1.FileName.EndsWith(".txt"))
            {
                args.IsValid = true;
            }
            else args.IsValid = false;
        }

        protected void CustomValidator_Failas2_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (Failas2.HasFile && Failas2.FileName.EndsWith(".txt"))
            {
                args.IsValid = true;
            }
            else args.IsValid = false;
        }
    }
}