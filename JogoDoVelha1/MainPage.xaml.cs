using System;
using System.Linq;

namespace JogoDoVelha1
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            if (!string.IsNullOrEmpty(btn.ClassId)) return;

            // 👤 Jogador (Sasuke)
            btn.ImageSource = "sasuke.png";
            btn.ClassId = "X";
            btn.IsEnabled = false;

            // Vitória X
            if (VerificarVitoria("X"))
            {
                await DisplayAlertAsync("Fim de Jogo", "Sasuke Ganhou!", "OK");
                Zerar();
                return;
            }

            // Velha antes da IA
            if (DeuVelha())
            {
                await DisplayAlertAsync("Fim de Jogo", "Deu velha!", "OK");
                Zerar();
                return;
            }

            // 🤖 IA (Itachi)
            JogadaIA();

            // Vitória O
            if (VerificarVitoria("O"))
            {
                await DisplayAlertAsync("Fim de Jogo", "Itachi Ganhou!", "OK");
                Zerar();
                return;
            }

            // Velha depois da IA
            if (DeuVelha())
            {
                await DisplayAlertAsync("Fim de Jogo", "Deu velha!", "OK");
                Zerar();
            }
        }

        // 🤖 IA
        void JogadaIA()
        {
            Button[] botoes = { btn10, btn11, btn12, btn20, btn21, btn22, btn30, btn31, btn32 };

            // Ganhar
            if (TentarJogada("O")) return;

            // Bloquear
            if (TentarJogada("X")) return;

            // Centro
            if (string.IsNullOrEmpty(btn21.ClassId))
            {
                btn21.ImageSource = "itachi.png";
                btn21.ClassId = "O";
                btn21.IsEnabled = false;
                return;
            }

            // Aleatório
            Random rnd = new Random();
            var vazios = botoes.Where(b => string.IsNullOrEmpty(b.ClassId)).ToList();

            if (vazios.Count > 0)
            {
                var escolha = vazios[rnd.Next(vazios.Count)];
                escolha.ImageSource = "itachi.png";
                escolha.ClassId = "O";
                escolha.IsEnabled = false;
            }
        }

        // 🧠 IA auxiliar
        bool TentarJogada(string jogador)
        {
            Button[][] combinacoes = new Button[][]
            {
                new Button[] { btn10, btn11, btn12 },
                new Button[] { btn20, btn21, btn22 },
                new Button[] { btn30, btn31, btn32 },

                new Button[] { btn10, btn20, btn30 },
                new Button[] { btn11, btn21, btn31 },
                new Button[] { btn12, btn22, btn32 },

                new Button[] { btn10, btn21, btn32 },
                new Button[] { btn12, btn21, btn30 }
            };

            foreach (var linha in combinacoes)
            {
                var vazios = linha.Where(b => string.IsNullOrEmpty(b.ClassId)).ToList();
                int count = linha.Count(b => b.ClassId == jogador);

                if (count == 2 && vazios.Count == 1)
                {
                    vazios[0].ImageSource = "itachi.png";
                    vazios[0].ClassId = "O";
                    vazios[0].IsEnabled = false;
                    return true;
                }
            }

            return false;
        }

        // ✅ Vitória
        bool VerificarVitoria(string jogador)
        {
            return
                (btn10.ClassId == jogador && btn11.ClassId == jogador && btn12.ClassId == jogador) ||
                (btn20.ClassId == jogador && btn21.ClassId == jogador && btn22.ClassId == jogador) ||
                (btn30.ClassId == jogador && btn31.ClassId == jogador && btn32.ClassId == jogador) ||

                (btn10.ClassId == jogador && btn20.ClassId == jogador && btn30.ClassId == jogador) ||
                (btn11.ClassId == jogador && btn21.ClassId == jogador && btn31.ClassId == jogador) ||
                (btn12.ClassId == jogador && btn22.ClassId == jogador && btn32.ClassId == jogador) ||

                (btn10.ClassId == jogador && btn21.ClassId == jogador && btn32.ClassId == jogador) ||
                (btn12.ClassId == jogador && btn21.ClassId == jogador && btn30.ClassId == jogador);
        }

        // ✅ Velha
        bool DeuVelha()
        {
            return
                !string.IsNullOrEmpty(btn10.ClassId) &&
                !string.IsNullOrEmpty(btn11.ClassId) &&
                !string.IsNullOrEmpty(btn12.ClassId) &&
                !string.IsNullOrEmpty(btn20.ClassId) &&
                !string.IsNullOrEmpty(btn21.ClassId) &&
                !string.IsNullOrEmpty(btn22.ClassId) &&
                !string.IsNullOrEmpty(btn30.ClassId) &&
                !string.IsNullOrEmpty(btn31.ClassId) &&
                !string.IsNullOrEmpty(btn32.ClassId);
        }

        // 🔄 Reset
        void Zerar()
        {
            Button[] botoes = { btn10, btn11, btn12, btn20, btn21, btn22, btn30, btn31, btn32 };

            foreach (var b in botoes)
            {
                b.ImageSource = null;
                b.ClassId = "";
                b.IsEnabled = true;
            }
        }
    }
}