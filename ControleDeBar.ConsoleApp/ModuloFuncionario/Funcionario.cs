using ControleDeBar.ConsoleApp.Compartilhado;

namespace ControleDeBar.ConsoleApp.ModuloFuncionario
{
    public class Funcionario : EntidadeBase
    {
        private static int idCounter = 1;
        public InformacoesPessoais informacoesPessoais { get; set; }
        public string senha { get; set; }

        public Funcionario()
        {

        }

        public Funcionario(InformacoesPessoais informacoesPessoais, string senha)
        {
            this.informacoesPessoais = informacoesPessoais;
            this.senha = senha;
        }

        public override void UpdateInfo(EntidadeBase imput)
        {
            Funcionario valid = (Funcionario)imput;

            informacoesPessoais = valid.informacoesPessoais;
            senha = valid.senha;
        }
    }
}
