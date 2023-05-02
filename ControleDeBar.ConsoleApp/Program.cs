using ControleDeBar.ConsoleApp.Compartilhado;
using ControleDeBar.ConsoleApp.ModuloConta;
using ControleDeBar.ConsoleApp.ModuloFuncionario;
using ControleDeBar.ConsoleApp.ModuloGarcon;
using ControleDeBar.ConsoleApp.ModuloMesa;
using ControleDeBar.ConsoleApp.ModuloProduto;

namespace ControleDeBar.ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            RepositorioFuncionario repositorioFuncionario = new();
            RepositorioGarcon repositorioGarcon = new();
            RepositorioMesa repositorioMesa = new();
            RepositorioProduto repositorioProduto = new();
            RepositorioConta repositorioConta = new();

            Validador validador = new Validador(repositorioFuncionario, repositorioGarcon, repositorioMesa, repositorioProduto, repositorioConta);

            TelaFuncionario telaFuncionario = new(repositorioFuncionario, validador);
            TelaGarcon telaGarcon = new(repositorioGarcon, repositorioFuncionario, telaFuncionario, validador);
            TelaMesa telaMesa = new(repositorioMesa, repositorioGarcon, telaGarcon, validador);
            TelaProduto telaProduto = new(repositorioProduto, repositorioFuncionario, telaFuncionario, validador);
            TelaConta telaConta = new(repositorioConta, repositorioGarcon, telaGarcon, validador);

            bool continuar = true;

            PopularCamposParaTeste(
            repositorioFuncionario,
            repositorioGarcon,
            repositorioMesa,
            repositorioProduto,
            repositorioConta);

            do
            {
                string opcao = MostrarMenuPrincipal();

                switch (opcao)
                {
                    case "S":
                        continuar = false;
                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                    case "1":
                        telaFuncionario.VisualizarTela();
                        break;
                    case "2":
                        telaProduto.VisualizarTela();
                        break;
                    case "3":
                        telaGarcon.VisualizarTela();
                        break;
                    case "4":
                        telaMesa.VisualizarTela();
                        break;
                    case "5":
                        telaConta.VisualizarTela();
                        break;
                }

            } while (continuar);

            string MostrarMenuPrincipal()
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Clear();
                Console.WriteLine("__________________________________________________________________________________");
                Console.WriteLine();
                Console.WriteLine("                          Bem-vindo ao Controle do Bar!                           ");
                Console.WriteLine("__________________________________________________________________________________");
                Console.WriteLine();
                Console.WriteLine("   Digite:                                                                        ");
                Console.WriteLine();
                Console.WriteLine("   1  - Para gestão de funcionarios.                                              ");
                Console.WriteLine("   2  - Para gestão de produtos.                                                  ");
                Console.WriteLine("   3  - Para gestão de garçons.                                                   ");
                Console.WriteLine("   4  - Para gestão de mesas.                                                     ");
                Console.WriteLine("   5  - Para gestão de contas.                                                    ");
                Console.WriteLine();
                Console.WriteLine("   S  - Para sair.                                                                ");
                Console.WriteLine("__________________________________________________________________________________");
                Console.WriteLine();
                Console.Write("   Opção escolhida: ");
                string opcao = Console.ReadLine().ToUpper();
                bool opcaoValida = opcao != "1" && opcao != "2" && opcao != "3" && opcao != "4" && opcao != "5" && opcao != "S";
                while (opcaoValida)
                {
                    if (opcaoValida)
                    {
                        Console.WriteLine("\n   Opção inválida, tente novamente. ");
                        break;
                    }
                }
                return opcao;
            }
        }

        public static void PopularCamposParaTeste(
        RepositorioFuncionario repositorioFuncionario,
        RepositorioGarcon repositorioGarcon,
        RepositorioMesa repositorioMesa,
        RepositorioProduto repositorioProduto,
        RepositorioConta repositorioConta)
        {

            InformacoesPessoais dados = new("Camile", "(49) 99999-9999", "Rua XXXX", "333.333.333-22");

            Funcionario camile = new Funcionario(dados, "1111");
            repositorioFuncionario.Insert(camile);
            Garcon camileGarcon = new Garcon(camile);
            repositorioGarcon.Insert(camileGarcon);
            Produto cerveja = new Produto("BRHAMA", "cerveja", 10);
            repositorioProduto.Insert(cerveja);
            Mesa mesa01 = new Mesa(camileGarcon);
        }
    }
}