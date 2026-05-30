namespace SistemaControleDeNivel
{
    public partial class Form1 : Form
    {
        bool valvulaEntrada = false;
        bool bombaEntrada = false;

        bool valvulaSaida = false;
        bool bombaSaida = false;

        bool emergenciaAtiva = false;

        double nivelTanque = 0;

        ModoSistema modoAtual = ModoSistema.Simulacao;

        public Form1()
        {
            InitializeComponent();
            timer1.Start();
        }
        public enum ModoSistema
        {
            Manual,
            Automatico,
            Simulacao,
            Emergencia,
            Manutencao
        }

        private void btnManual_Click(object sender, EventArgs e)
        {
            modoAtual = ModoSistema.Manual;

            if (modoAtual != ModoSistema.Manual)
            {
                lblModo.Text = "...";
                lblModo.ForeColor = Color.Black;
            }
            else
            {
                lblModo.Text = "MANUAL";
                lblModo.ForeColor = Color.DarkGreen;
            }

            AtualizarControles();
        }
        private void AtualizarControles()
        {
            bool manual = modoAtual == ModoSistema.Manual;

            btnAbValEntrada.Enabled = manual;
            btnAbValSaida.Enabled = manual;

            btnFcValEntrada.Enabled = manual;
            btnFcValSaida.Enabled = manual;

            btnAbBmbEntrada.Enabled = manual;
            btnAbBmbSaida.Enabled = manual;

            btnFcBmbEntrada.Enabled = manual;
            btnFcBmbSaida.Enabled = manual;
        }

        private void AtualizarImagemTanque()
        {
            if (nivelTanque <= 0)
            {
                pictureBoxTanque.Image =
                    Properties.Resources._01_tanque_vazio;
            }
            else if (nivelTanque <= 25)
            {
                pictureBoxTanque.Image =
                    Properties.Resources._02_enchimento_25;
            }
            else if (nivelTanque <= 50)
            {
                pictureBoxTanque.Image =
                    Properties.Resources._03_enchimento_50;
            }
            else if (nivelTanque <= 75)
            {
                pictureBoxTanque.Image =
                    Properties.Resources._04_enchimento_75;
            }
            else
            {
                pictureBoxTanque.Image =
                    Properties.Resources._05_tanque_cheio_100;
            }
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            if (valvulaEntrada && !bombaEntrada)
            {
                nivelTanque += 2;
            }

            if (valvulaEntrada && bombaEntrada)
            {
                nivelTanque += 5;
            }

            if (valvulaSaida && !bombaSaida)
            {
                nivelTanque -= 2;
            }

            if (valvulaSaida && bombaSaida)
            {
                nivelTanque -= 5;
            }

            if (nivelTanque > 100)
                nivelTanque = 100;

            if (nivelTanque < 0)
                nivelTanque = 0;

            AtualizarImagemTanque();
        }

        private void btnAbValEntrada_Click(object sender, EventArgs e)
        {
            if (modoAtual != ModoSistema.Manual)
                return;

            valvulaEntrada = true;
        }

        private void btnFcValEntrada_Click(object sender, EventArgs e)
        {
            if (modoAtual != ModoSistema.Manual)
                return;

            valvulaEntrada = false;
        }

        private void btnAbBmbEntrada_Click(object sender, EventArgs e)
        {
            if (modoAtual != ModoSistema.Manual)
                return;

            bombaEntrada = true;
        }

        private void btnFcBmbEntrada_Click(object sender, EventArgs e)
        {
            if (modoAtual != ModoSistema.Manual)
                return;

            bombaEntrada = false;
        }

        private void btnAbValSaida_Click(object sender, EventArgs e)
        {
            if (modoAtual != ModoSistema.Manual)
                return;

            valvulaSaida = true;
        }

        private void btnFcValSaida_Click(object sender, EventArgs e)
        {
            if (modoAtual != ModoSistema.Manual)
                return;

            valvulaSaida = false;
        }

        private void btnAbBmbSaida_Click(object sender, EventArgs e)
        {
            if (modoAtual != ModoSistema.Manual)
                return;

            bombaSaida = true;
        }

        private void btnFcBmbSaida_Click(object sender, EventArgs e)
        {
            if (modoAtual != ModoSistema.Manual)
                return;

            bombaSaida = false;
        }

        private void DesabilitarSistema()
        {
            // Modos
            btnManual.Enabled = false;
            btnAutomatico.Enabled = false;
            btnManutencao.Enabled = false;
            btnSimulacao.Enabled = false;

            // Válvulas
            btnAbValEntrada.Enabled = false;
            btnFcValEntrada.Enabled = false;

            btnAbValSaida.Enabled = false;
            btnFcValSaida.Enabled = false;

            // Bombas
            btnAbBmbEntrada.Enabled = false;
            btnFcBmbEntrada.Enabled = false;

            btnAbBmbSaida.Enabled = false;
            btnFcBmbSaida.Enabled = false;
        }

        private void btnEmergencia_Click(object sender, EventArgs e)
        {
            emergenciaAtiva = !emergenciaAtiva;

            if (emergenciaAtiva)
            {
                pictureBtnEm.Image =
                    Properties.Resources.btnEmFechado2;

                lblModo.Text = "EMERGÊNCIA";
                lblModo.ForeColor = Color.Red;

                timer1.Stop();

                valvulaEntrada = false;
                bombaEntrada = false;

                valvulaSaida = false;
                bombaSaida = false;

                DesabilitarSistema();

                btnEmergencia.BackColor = Color.Red;
            }

            else
            {
                pictureBtnEm.Image =
                    Properties.Resources.btnEmAberto2;

                lblModo.Text = "...";
                lblModo.ForeColor = Color.Black;

                timer1.Start();

                AtualizarControles();

                btnManual.Enabled = true;
                btnAutomatico.Enabled = true;
                btnManutencao.Enabled = true;
                btnSimulacao.Enabled = true;

                btnEmergencia.BackColor = Color.White;
            }
        }
    }
}
