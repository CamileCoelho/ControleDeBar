using ControleDeBar.ConsoleApp.Compartilhado;
using ControleDeBar.ConsoleApp.ModuloFuncionario;
using ControleDeBar.ConsoleApp.ModuloGarcon;
using ControleDeBar.ConsoleApp.ModuloMesa;
using ControleDeBar.ConsoleApp.ModuloProduto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleDeBar.ConsoleApp.ModuloConta
{
    public class TelaConta : TelaBase<Conta>
    {
        RepositorioBase<Conta> repositorioBase;
        RepositorioConta repositorioConta;
        RepositorioBase<Mesa> repositorioMesa;
        TelaMesa telaMesa;
        RepositorioBase<Garcon> repositorioGarcon;
        TelaGarcon telaGarcon;
        RepositorioBase<Produto> repositorioProduto;
        TelaProduto telaProduto;
        Validador validador;

        public TelaConta(RepositorioConta repositorioConta, RepositorioMesa repositorioMesa, RepositorioProduto repositorioProduto, TelaProduto telaProduto, TelaMesa telaMesa, RepositorioGarcon repositorioGarcon, TelaGarcon telaGarcon, Validador validador)
        {
            this.repositorioConta = repositorioConta;
            repositorioBase = repositorioConta;
            this.repositorioMesa = repositorioMesa;
            this.telaMesa = telaMesa;
            this.repositorioGarcon = repositorioGarcon;
            this.telaGarcon = telaGarcon;
            this.repositorioProduto = repositorioProduto;
            this.telaProduto = telaProduto;
            this.validador = validador;
        }

        public void VisualizarTela()
        {
            bool continuar = true;

            do
            {
                string opcao = MostrarMenuConta();

                switch (opcao)
                {
                    case "6":
                        continuar = false;
                        Console.ResetColor();
                        break;
                    case "1":
                        Request();
                        continue;
                    case "2":
                        Visualizar();
                        continue;
                    case "3":
                        Editar();
                        continue;
                    case "4":
                        VisualizarEmAberto();
                        continue;
                    case "5":
                        VisualizarEncerradas();
                        continue;
                }
            } while (continuar);

            string MostrarMenuConta()
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Clear();
                Console.WriteLine("______________________________________________________________________________________________");
                Console.WriteLine();
                Console.WriteLine("                                      Gestão de Contas!                                       ");
                Console.WriteLine("______________________________________________________________________________________________");
                Console.WriteLine();
                Console.WriteLine("   Digite:                                                                                    ");
                Console.WriteLine();
                Console.WriteLine("   1  - Para abrir uma conta.                                                                 ");
                Console.WriteLine();
                Console.WriteLine("   2  - Para visualizar as suas contas diárias.                                               ");
                Console.WriteLine();
                Console.WriteLine("   3  - Para editar uma conta.                                                                ");
                Console.WriteLine();
                Console.WriteLine("   4  - Para visualizar suas contas em aberto.                                                ");
                Console.WriteLine();
                Console.WriteLine("   5  - Para visualizar suas contas encerradas.                                               ");
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("   6  - Para voltar ao menu principal.                                                        ");
                Console.WriteLine("______________________________________________________________________________________________");
                Console.WriteLine();
                Console.Write("   Opção escolhida: ");
                string opcao = Console.ReadLine().ToUpper();
                bool opcaoInvalida = opcao != "1" && opcao != "2" && opcao != "3" && opcao != "4" && opcao != "5" && opcao != "6";
                while (opcaoInvalida)
                {
                    if (opcaoInvalida)
                    {
                        ExibirMensagem("\n   Opção inválida, tente novamente. ", ConsoleColor.DarkRed);
                        break;
                    }
                }
                return opcao;
            }
        }

        private void Request()
        {
            if (repositorioGarcon.GetAll().Count == 0)
            {
                ExibirMensagem("\n   Nenhum garçom cadastrado. " +
                    "\n   Você deve cadastrar um garçom para poder abrir uma conta.", ConsoleColor.DarkRed);
                return;
            }
            if (repositorioMesa.GetAll().Count == 0)
            {
                ExibirMensagem("\n   Nenhuma mesa cadastrada. " +
                    "\n   Você deve cadastrar uma mesa para poder abrir uma conta.", ConsoleColor.DarkRed);
                return;
            }
            if (repositorioProduto.GetAll().Count == 0)
            {
                ExibirMensagem("\n   Nenhum produto cadastrado. " +
                    "\n   Você deve cadastrar um produto para poder abrir uma conta.", ConsoleColor.DarkRed);
                return;
            }

            Imput(out Mesa mesa, out Pedido pedido, out DateTime data, out string senhaImputada);

            Conta toAdd = new(mesa, pedido, data);

            string valido = validador.ValidarConta(toAdd, senhaImputada);

            if (valido == "REGISTRO_REALIZADO")
            {
                repositorioConta.Insert(toAdd);
                ExibirMensagem("\n   Conta aberta com sucesso!", ConsoleColor.DarkGreen);
            }
            else
            {
                ExibirMensagem("\n   Conta Não Aberta: " + valido, ConsoleColor.DarkRed);
            }
        }

        private void Visualizar()
        {
            if (repositorioConta.GetAll().Count == 0)
            {
                ExibirMensagem("\n   Nenhuma conta encontrada. " +
                    "\n   Você deve abrir uma conta para poder visualizar suas contas.", ConsoleColor.DarkRed);
                return;
            }
            MostarListaContas(repositorioConta);
            Console.ReadLine();
        }

        private void Editar()
        {
            if (repositorioRequisicao.GetAll().Count == 0)
            {
                ExibirMensagem("\n   Nenhuma requisição encontrada. " +
                    "\n   Você deve realizar uma requisição para poder visualizar suas requisições.", ConsoleColor.DarkRed);
                return;
            }

            Requisicao toEdit = (Requisicao)repositorioBase.GetById(ObterId(repositorioRequisicao));

            if (toEdit == null)
            {
                ExibirMensagem("\n   Requisição não encontrada!", ConsoleColor.DarkRed);
                return;
            }
            else
            {
                toEdit.informacoesReposicao.remedio.quantidadeDisponivel = toEdit.informacoesReposicao.remedio.quantidadeDisponivel + toEdit.quantidadeRequisitada;

                ImputEdit(toEdit, out int quantidadeRequisitada, out string senhaImputada);

                Requisicao imput = new(toEdit.paciente, toEdit.informacoesReposicao, toEdit.quantidadeRequisitada);

                string valido = validador.ValidarRequisicao(imput, senhaImputada);

                if (valido == "REGISTRO_REALIZADO")
                {
                    repositorioRequisicao.Update(toEdit, imput);
                    toEdit.informacoesReposicao.remedio.quantidadeDisponivel = toEdit.informacoesReposicao.remedio.quantidadeDisponivel - quantidadeRequisitada;
                    ExibirMensagem("\n   Requisição editada com sucesso!", ConsoleColor.DarkGreen);
                }
                else
                {
                    ExibirMensagem("\n   Requisição Não Editada:" + valido, ConsoleColor.DarkRed);
                }
            }
        }

        private void VisualizarEmAberto()
        {
            if (repositorioConta.GetAll().Count == 0)
            {
                ExibirMensagem("\n   Nenhuma conta encontrada. " +
                    "\n   Você deve abrir uma conta para poder visualizar suas contas.", ConsoleColor.DarkRed);
                return;
            }
            MostarListaContasEmAberto(repositorioConta);
            Console.ReadLine();
        }

        private void VisualizarEncerradas()
        {
            if (repositorioConta.GetAll().Count == 0)
            {
                ExibirMensagem("\n   Nenhuma conta encontrada. " +
                    "\n   Você deve abrir uma conta para poder visualizar suas contas.", ConsoleColor.DarkRed);
                return;
            }
            MostarListaContasEncerradas(repositorioConta);
            Console.ReadLine();
        }

        private void Imput(out Mesa mesa, out Pedido pedido, out DateTime data, out string senhaImputada)
        {
            Console.Clear();

            mesa = (Mesa)repositorioMesa.GetById(telaMesa.ObterId(repositorioMesa));

            data = DateTime.Now.Date;

            Produto produto = (Produto)repositorioProduto.GetById(telaProduto.ObterId(repositorioProduto));

            Console.Write("\n   Digite a quatidade de unidades que o cliente deseja desse produto: ");
            while (!int.TryParse(Console.ReadLine(), out pedido.quantidadeProduto))
            {
                ExibirMensagem("\n   Entrada inválida! Digite um número inteiro. ", ConsoleColor.DarkRed);
                Console.Write("\n   Digite a quatidade de unidades que o unidades que o cliente deseja desse produto: ");
            }
            mesa.garcon = (Garcon)repositorioGarcon.GetById(telaGarcon.ObterId(repositorioGarcon));
            Console.Write("\n   Digite a senha: ");
            senhaImputada = Console.ReadLine();
        }

        public override int ObterId(RepositorioBase<Conta> repositorioBase)
        {
            Console.Clear();
            MostarListaContas(repositorioConta);

            Console.Write("\n   Digite o id da conta: ");
            int id;
            while (!int.TryParse(Console.ReadLine(), out id))
            {
                ExibirMensagem("\n   Entrada inválida! Digite um número inteiro. ", ConsoleColor.DarkRed);
                Console.Write("\n   Digite o id da conta: ");
            }
            return id;
        }

        public void MostarListaContasEmAberto(RepositorioConta repositorioConta)
        {
            Console.Clear();
            Console.WriteLine("______________________________________________________________________________________________________________________");
            Console.WriteLine();
            Console.WriteLine("                                          Lista de Contas em Aberto!                                                  ");
            Console.WriteLine("______________________________________________________________________________________________________________________");
            Console.WriteLine();
            Console.WriteLine("{0,-5}|{1,-25}|{2,-25}|{3,-25}|{4,-25}|{5,-25}", "ID ", "  MESA ", "  GARÇOM ", "  VALOR TOTAL ", "  STATUS ", "  DATA ");
            Console.WriteLine("______________________________________________________________________________________________________________________");
            Console.WriteLine();

            foreach (Conta print in repositorioConta.GetAll())
            {
                if (print != null && print.mesa.status == "DISPONIVEL")
                {
                    Console.WriteLine("{0,-5}|{1,-25}|{2,-25}|{3,-25}|{4,-25}|{5,-25}", print.id, print.mesa.id, print.mesa.garcon.informacoesPessoais.nome, print.valorFinal, print.mesa.status, print.data.ToString("dd/MM/yyyy");
                }
            }
        }

        public void MostarListaContasEncerradas(RepositorioConta repositorioConta)
        {
            Console.Clear();
            Console.WriteLine("______________________________________________________________________________________________________________________");
            Console.WriteLine();
            Console.WriteLine("                                         Lista de Contas Encerradas!                                                  ");
            Console.WriteLine("______________________________________________________________________________________________________________________");
            Console.WriteLine();
            Console.WriteLine("{0,-5}|{1,-25}|{2,-25}|{3,-25}|{4,-25}|{5,-25}", "ID ", "  MESA ", "  GARÇOM ", "  VALOR TOTAL ", "  STATUS ", "  DATA ");
            Console.WriteLine("______________________________________________________________________________________________________________________");
            Console.WriteLine();

            foreach (Conta print in repositorioConta.GetAll())
            {
                if (print != null && print.mesa.status != "DISPONIVEL")
                {
                    Console.WriteLine("{0,-5}|{1,-25}|{2,-25}|{3,-25}|{4,-25|{5,-25}}", print.id, print.mesa.id, print.mesa.garcon.informacoesPessoais.nome, print.valorFinal, print.mesa.status, print.data.ToString("dd/MM/yyyy"));
                }
            }
        }

        public void MostarListaContas(RepositorioConta repositorioConta)
        {
            Console.Clear();
            Console.WriteLine("______________________________________________________________________________________________________________________");
            Console.WriteLine();
            Console.WriteLine("                                           Lista de Contas Diárias!                                                   ");
            Console.WriteLine("______________________________________________________________________________________________________________________");
            Console.WriteLine();
            Console.WriteLine("{0,-5}|{1,-25}|{2,-25}|{3,-25}|{4,-25}|{5,-25}", "ID ", "  MESA ", "  GARÇOM ", "  VALOR TOTAL ", "  STATUS ", "  DATA ");
            Console.WriteLine("______________________________________________________________________________________________________________________");
            Console.WriteLine();

            foreach (Conta print in repositorioConta.GetAll())
            {
                if (print != null && print.data == DateTime.Today)
                {
                    Console.WriteLine("{0,-5}|{1,-25}|{2,-25}|{3,-25}|{4,-25}|{5,-25}", print.id, print.mesa.id, print.mesa.garcon.informacoesPessoais.nome, print.valorFinal, print.data.ToString("dd/MM/yyyy"));
                }
            }
        }
    }
}
