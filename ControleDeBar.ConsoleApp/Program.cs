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
            Financeiro financeiro = new();

            RepositorioFuncionario repositorioFuncionario = new();
            RepositorioGarcon repositorioGarcon = new();
            RepositorioMesa repositorioMesa = new();
            RepositorioProduto repositorioProduto = new();
            RepositorioConta repositorioConta = new(financeiro);

            Validador validador = new Validador(repositorioFuncionario, repositorioGarcon, repositorioMesa, repositorioProduto, repositorioConta);

            TelaFuncionario telaFuncionario = new(repositorioFuncionario, validador);
            TelaGarcon telaGarcon = new(repositorioGarcon, repositorioFuncionario, telaFuncionario, validador);
            TelaMesa telaMesa = new(repositorioMesa, repositorioGarcon, telaGarcon, validador);
            TelaProduto telaProduto = new(repositorioProduto, repositorioFuncionario, telaFuncionario, validador);
            TelaConta telaConta = new(repositorioConta, repositorioMesa, repositorioProduto, telaProduto, telaMesa, repositorioGarcon, telaGarcon, validador);

            bool continuar = true;

            PopularCamposParaTeste(repositorioFuncionario, repositorioGarcon, repositorioMesa, repositorioProduto, repositorioConta);

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
                    case "6":
                        financeiro.ZerarFaturamento();
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
                Console.WriteLine($"   Faturamento do dia: R$ {financeiro.faturamento},00                            ");
                Console.WriteLine("__________________________________________________________________________________");
                Console.WriteLine();
                Console.WriteLine("   Digite:                                                                        ");
                Console.WriteLine();
                Console.WriteLine("   1  - Para gestão de funcionarios.                                              ");
                Console.WriteLine();
                Console.WriteLine("   2  - Para gestão de produtos.                                                  ");
                Console.WriteLine();
                Console.WriteLine("   3  - Para gestão de garçons.                                                   ");
                Console.WriteLine();
                Console.WriteLine("   4  - Para gestão de mesas.                                                     ");
                Console.WriteLine();
                Console.WriteLine("   5  - Para gestão de contas.                                                    ");
                Console.WriteLine();
                Console.WriteLine("   6  - Para zerar seu histórico de faturamento do dia.                           ");
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("   S  - Para sair.                                                                ");
                Console.WriteLine("__________________________________________________________________________________");
                Console.WriteLine();
                Console.Write("   Opção escolhida: ");
                string opcao = Console.ReadLine().ToUpper();
                bool opcaoValida = opcao != "1" && opcao != "2" && opcao != "3" && opcao != "4" && opcao != "5" && opcao != "6" && opcao != "S";
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
            InformacoesPessoais dados2 = new("Tiago", "(49) 99999-9999", "Rua YYYY", "111.222.333-44");
            InformacoesPessoais dados3= new("Rech", "(49) 99999-9999", "Rua ZZZZ", "333.222.333-22");

            Funcionario camile = new Funcionario(dados, "1111");
            repositorioFuncionario.Insert(camile);
            Funcionario tiago = new Funcionario(dados2, "2222");
            repositorioFuncionario.Insert(tiago);
            Funcionario rech = new Funcionario(dados3, "3333");
            repositorioFuncionario.Insert(rech);

            Garcon camileGarcon = new Garcon(camile);
            repositorioGarcon.Insert(camileGarcon);
            Garcon tiagog = new Garcon(tiago);
            repositorioGarcon.Insert(tiagog);
            Garcon rechg = new Garcon(rech);
            repositorioGarcon.Insert(rechg);

            Produto cerveja = new Produto("BRHAMA", "cerveja", 10);
            repositorioProduto.Insert(cerveja);
            Produto cerveja2 = new Produto("ORIGINAL", "cerveja", 10);
            repositorioProduto.Insert(cerveja2);
            Produto cerveja3 = new Produto("BUD", "cerveja", 12);
            repositorioProduto.Insert(cerveja3);
            Produto batata = new Produto("BATATA", "porção 500g", 35);
            repositorioProduto.Insert(batata);
            Produto polenta = new Produto("POLENTA", "porção 500g", 40);
            repositorioProduto.Insert(polenta);

            Mesa mesa01 = new Mesa(camileGarcon);
            repositorioMesa.Insert(mesa01);
            Mesa mesa02 = new Mesa(camileGarcon);
            repositorioMesa.Insert(mesa02);
            Mesa mesa03 = new Mesa(camileGarcon);
            repositorioMesa.Insert(mesa03);
            Mesa mesa04 = new Mesa(tiagog);
            repositorioMesa.Insert(mesa04);
            Mesa mesa05 = new Mesa(tiagog);
            repositorioMesa.Insert(mesa02);
            Mesa mesa06 = new Mesa(tiagog);
            repositorioMesa.Insert(mesa06);
            Mesa mesa07 = new Mesa(rechg);
            repositorioMesa.Insert(mesa07);
            Mesa mesa08 = new Mesa(rechg);
            repositorioMesa.Insert(mesa08);
        }
    }
}