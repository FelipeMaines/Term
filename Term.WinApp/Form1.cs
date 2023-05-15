using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;

namespace Term.WinApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            btnEnter.Enabled = false;
        }

        private int inicial = 1;
        private int final = 5;
        public string palavraChutada = "";

        Jogo jogo = new Jogo();

        private void AcaoChute_Click(object sender, EventArgs e)
        {
            if (sender is Button btn)
            {
                string letra = btn.Text.ToLower();
               

                if (DivisivelPorCinco())
                {
                    EscreverNaCaixa(letra);
                    DesabilitarTeclado();

                }
                else
                    EscreverNaCaixa(letra);

            }

            jogo.contador++;
        }

       

        private void EscreverNaCaixa(string letra)
        {
            foreach (TextBox textBox in panel.Controls.OfType<TextBox>().OrderBy(tb => tb.Tag))
            {

                string strTag = textBox.Tag.ToString();

                int tag = Convert.ToInt32(strTag);

                if (tag == jogo.contador)
                {
                    textBox.Text = letra;
                    break;
                }
            }
        }

        private bool DivisivelPorCinco()
        {

            if (jogo.contador % 5 == 0 && jogo.contador != 0)
            {
                btnEnter.Enabled = true;
                return true;
            }

            else
                return false;
        }

        private void DesabilitarTeclado()
        {
            foreach (Control control in caixaTeclado.Controls)
            {
                if (control is Button button)
                    button.Enabled = false;
            }
        }

        private void LiberarTeclado()
        {
            foreach (Control control in caixaTeclado.Controls)
            {
                if (control is Button button)
                    button.Enabled = true;
            }
        }

        private void btnEnter_Click(object sender, EventArgs e)
        {

            VerificarSeAcertou();

            LiberarTeclado();
        }

        private void btnApagar_Click(object sender, EventArgs e)
        {
            foreach (TextBox textBox in panel.Controls.OfType<TextBox>().OrderBy(tb => tb.Tag))
            {
               

                string strTag = textBox.Tag.ToString();

                int tag = Convert.ToInt32(strTag);

                if (tag == jogo.contador - 1)
                {
                    if (textBox.WordWrap == false)
                    {
                        MessageBox.Show("Nao pode apagar um elemento ja verificado!");
                        return;
                    }

                    textBox.Text = "";
                    jogo.contador--;
                    break;
                }
            }
        }

        private void VerificarSeAcertou()
        {
            string palavra = jogo.palavra;

            palavraChutada = PegarPalavraCaixa();

            int contador = 0;

            foreach (TextBox textBox in panel.Controls.OfType<TextBox>().OrderBy(tb => tb.Tag))
            {
                string strTag = textBox.Tag.ToString();
                int tag = Convert.ToInt32(strTag);

                if (tag >= inicial && tag <= final)
                {
                    if (palavra.Contains(textBox.Text))
                    {
                        try
                        {
                            if (palavra[contador] == palavraChutada[contador])
                            {
                                textBox.BackColor = Color.Green;
                                textBox.WordWrap = false;
                                contador++;
                            }

                            else
                            {
                                textBox.BackColor = Color.Yellow;
                                textBox.WordWrap = false;
                                contador++;
                            }

                        }catch(IndexOutOfRangeException)
                        {
                           
                        }
                    }

                    else
                    {
                        textBox.BackColor = Color.DarkGray;
                        textBox.WordWrap = false;
                        contador++;

                    }
                }
            }

            if (VerificarVitoria(palavra))
                return;

            if (VerificarDerrota())
                return;

            inicial += 5;
            final += 5;
        }

        //private void VerificarSeAcertou()
        //{
        //    string palavra = jogo.palavra;

        //    palavraChutada = PegarPalavraCaixa();



        //    foreach (TextBox textBox in panel.Controls.OfType<TextBox>())
        //    {
        //        string strTag = textBox.Tag.ToString();
        //        int tag = Convert.ToInt32(strTag);

        //        if (tag >= inicial && tag <= final)
        //        {

        //            if (palavra.Contains(textBox.Text))
        //            {

        //                int index = palavra.IndexOf(textBox.Text);

        //                try
        //                {
        //                    if (palavra[index].ToString().ToLower() != palavraChutada[index].ToString())
        //                    {
        //                        textBox.BackColor = Color.Yellow;
        //                        textBox.WordWrap = false;

        //                        continue;
        //                    }

        //                    if (palavra[index].ToString().ToLower() == palavraChutada[index].ToString())
        //                    {
        //                        textBox.BackColor = Color.DarkGreen;
        //                        textBox.WordWrap = false;
        //                        continue;
        //                    }
        //                }
        //                catch (IndexOutOfRangeException)
        //                {
        //                    PegarPalavraCaixa();
        //                }
        //            }

        //            else
        //            {
        //                textBox.BackColor = Color.DarkGray;
        //                textBox.WordWrap = false;

        //            }
        //        }

        //        else
        //            continue;
        //    }

        //    if (VerificarVitoria(palavra))
        //        return;

        //    if (VerificarDerrota())
        //        return;

        //    inicial += 5;
        //    final += 5;
        //}



        private bool VerificarDerrota()
        {
            if(jogo.contador > 25)
            {
                MessageBox.Show($"Vc perdeu a palavra era: {jogo.palavra}");
                this.jogo = new Jogo();
                this.inicial = 1;
                this.final = 5;

                LimparQuadro();
                VoltarCorBotoes();
                return true;
            }
             return false;
        }

        private bool VerificarVitoria(string palavra)
        {
            if (palavraChutada.ToLower() == palavra.ToLower())
            {
                MessageBox.Show("Win");
                this.jogo = new Jogo();
                this.inicial = 1;
                this.final = 5;

                LimparQuadro();
                VoltarCorBotoes();
                return true;
            }

            return false;
        }

        private void VoltarCorBotoes()
        {
            foreach(Button btn in caixaTeclado.Controls)
            {
                btn.BackColor = Color.Teal;
            }
        }

        private void LimparQuadro()
        {
            foreach (TextBox textBox in panel.Controls.OfType<TextBox>())
            {
                textBox.Text = "";
                textBox.WordWrap = true;
                textBox.BackColor = SystemColors.Window;
            }
        }

        private string PegarPalavraCaixa()
        {
            string palavra = "";
            int ponto = inicial;

            foreach (TextBox textBox in panel.Controls.OfType<TextBox>().OrderBy(tb => tb.Tag))
            {
                string strTag = textBox.Tag.ToString();
                int tag = Convert.ToInt32(strTag);


                if (tag == ponto && tag <= final)
                {
                    if (palavra.Length == 5)
                        break;

                    palavra += textBox.Text;
                    ponto++;

                }
            }

            return palavra;
        }
    }

}