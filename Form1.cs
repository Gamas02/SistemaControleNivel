namespace SistemaControleDeNivel
{
    public partial class Form1 : Form
    {
        bool valvulaEntrada = false;
        bool bombaEntrada = false;

        bool valvulaSaida = false;
        bool bombaSaida = false;

        bool emergenciaAtiva = false;
        bool enchendoAutomatico = false;
        bool simulacaoEnchendo = true;

        double nivelTanque = 0;

        ModoSistema modoAtual = ModoSistema.Simulacao;

        public Form1()
        {
            InitializeComponent();

            modoAtual = ModoSistema.Simulacao;

            lblModo.Text = "SIMULAÇÃO";
            lblModo.ForeColor = Color.Blue;

            AtualizarImagemTanque();
            AtualizarControles();

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

            AdicionarHistorico("Modo MANUAL ativado.");

            AtualizarControles();
        }
        private void AtualizarControles()
        {
            bool manual = modoAtual == ModoSistema.Manual;

            bool podeOperar =
                modoAtual == ModoSistema.Manual ||
                modoAtual == ModoSistema.Manutencao;

            btnAbValEntrada.Enabled = podeOperar;
            btnFcValEntrada.Enabled = podeOperar;

            btnAbValSaida.Enabled = podeOperar;
            btnFcValSaida.Enabled = podeOperar;

            btnAbBmbEntrada.Enabled = podeOperar;
            btnFcBmbEntrada.Enabled = podeOperar;

            btnAbBmbSaida.Enabled = podeOperar;
            btnFcBmbSaida.Enabled = podeOperar;
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

            if (modoAtual == ModoSistema.Automatico)
            {
                // Liga enchimento abaixo de 20%
                if (nivelTanque <= 20)
                {
                    enchendoAutomatico = true;
                }

                // Desliga enchimento em 80%
                if (nivelTanque >= 80)
                {
                    enchendoAutomatico = false;
                }

                // Enquanto estiver enchendo
                if (enchendoAutomatico)
                {
                    nivelTanque += 2;
                }
            }

            if (nivelTanque <= 20 && !enchendoAutomatico)
            {
                enchendoAutomatico = true;

                AdicionarHistorico(
                    "Nível abaixo de 20%. Iniciando enchimento automático."
                );
            }

            if (nivelTanque >= 75 && enchendoAutomatico)
            {
                enchendoAutomatico = false;

                AdicionarHistorico(
                    "Nível atingiu 75%. Enchimento automático encerrado."
                );
            }

            if (modoAtual == ModoSistema.Simulacao)
            {
                // Se estiver enchendo
                if (simulacaoEnchendo)
                {
                    nivelTanque += 2;

                    // Chegou ao máximo
                    if (nivelTanque >= 100)
                    {
                        nivelTanque = 100;

                        simulacaoEnchendo = false;

                        AdicionarHistorico(
                            "Simulação: tanque cheio. Iniciando esvaziamento."
                        );
                    }
                }
                else
                {
                    // Esvaziando
                    nivelTanque -= 2;

                    // Chegou ao mínimo
                    if (nivelTanque <= 0)
                    {
                        nivelTanque = 0;

                        simulacaoEnchendo = true;

                        AdicionarHistorico(
                            "Simulação: tanque vazio. Iniciando enchimento."
                        );
                    }
                }
            }

            if (nivelTanque < 0)
                nivelTanque = 0;

            AtualizarImagemTanque();
        }

        private void AdicionarHistorico(string mensagem)
        {
            lstHistorico.Items.Insert(
                0,
                $"[{DateTime.Now:HH:mm:ss}] {mensagem}"
            );
        }

        private void btnAbValEntrada_Click(object sender, EventArgs e)
        {
            if (modoAtual != ModoSistema.Manual && modoAtual != ModoSistema.Manutencao)
                return;

            valvulaEntrada = true;

            AdicionarHistorico("Válvula de entrada aberta.");
        }

        private void btnFcValEntrada_Click(object sender, EventArgs e)
        {
            if (modoAtual != ModoSistema.Manual && modoAtual != ModoSistema.Manutencao)
                return;

            valvulaEntrada = false;

            AdicionarHistorico("Válvula de entrada fechada.");
        }

        private void btnAbBmbEntrada_Click(object sender, EventArgs e)
        {
            if (modoAtual != ModoSistema.Manual && modoAtual != ModoSistema.Manutencao)
                return;

            bombaEntrada = true;

            AdicionarHistorico("Bomba de entrada ligada.");

            lblBombaEntrada.Text = "Ligado";
        }

        private void btnFcBmbEntrada_Click(object sender, EventArgs e)
        {
            if (modoAtual != ModoSistema.Manual && modoAtual != ModoSistema.Manutencao)
                return;

            bombaEntrada = false;

            AdicionarHistorico("Bomba de entrada desligada.");

            lblBombaEntrada.Text = "Desligado";
        }

        private void btnAbValSaida_Click(object sender, EventArgs e)
        {
            if (modoAtual != ModoSistema.Manual && modoAtual != ModoSistema.Manutencao)
                return;

            valvulaSaida = true;

            AdicionarHistorico("Valvula de saida ligada.");
        }

        private void btnFcValSaida_Click(object sender, EventArgs e)
        {
            if (modoAtual != ModoSistema.Manual && modoAtual != ModoSistema.Manutencao)
                return;

            valvulaSaida = false;

            AdicionarHistorico("Valvula de saida desligada.");
        }

        private void btnAbBmbSaida_Click(object sender, EventArgs e)
        {
            if (modoAtual != ModoSistema.Manual && modoAtual != ModoSistema.Manutencao)
                return;

            bombaSaida = true;

            AdicionarHistorico("Bomba de saida ligada.");

            lblBombaSaida.Text = "Ligado";
        }

        private void btnFcBmbSaida_Click(object sender, EventArgs e)
        {
            if (modoAtual != ModoSistema.Manual && modoAtual != ModoSistema.Manutencao)
                return;

            bombaSaida = false;

            AdicionarHistorico("Bomba de saida desligada.");

            lblBombaSaida.Text = "Desligado";
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

                AdicionarHistorico("PARADA DE EMERGÊNCIA acionada.");
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

                AdicionarHistorico("PARADA DE EMERGÊNCIA liberada.");
            }
        }

        private void btnAutomatico_Click(object sender, EventArgs e)
        {
            modoAtual = ModoSistema.Automatico;

            if (modoAtual != ModoSistema.Automatico)
            {
                lblModo.Text = "...";
                lblModo.ForeColor = Color.Black;
            }
            else
            {
                lblModo.Text = "AUTOMATICO";
                lblModo.ForeColor = Color.DarkGreen;
            }

        }

        private void btnSimulacao_Click(object sender, EventArgs e)
        {
            modoAtual = ModoSistema.Simulacao;

            lblModo.Text = "SIMULAÇÃO";
            lblModo.ForeColor = Color.Blue;

            AdicionarHistorico("Modo SIMULAÇÃO ativado.");

            AtualizarControles();
        }

        private void btnManutencao_Click(object sender, EventArgs e)
        {
            modoAtual = ModoSistema.Manutencao;

            lblModo.Text = "MANUTENÇÃO";
            lblModo.ForeColor = Color.Orange;

            MessageBox.Show(
                "Sistema em manutenção",
                "Manutenção",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );

            AdicionarHistorico("Modo MANUTENÇÃO ativado.");

            AtualizarControles();
        }
    }
}
