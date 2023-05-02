using ControleDeBar.ConsoleApp.Compartilhado;
using ControleDeBar.ConsoleApp.ModuloFuncionario;
using ControleDeBar.ConsoleApp.ModuloGarcon;
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
        RepositorioBase<Garcon> repositorioGarcon;
        TelaGarcon telaGarcon;
        Validador validador;

        public TelaRequisicao(RepositorioRequisicao repositorioRequisicao, RepositorioPaciente repositorioPaciente, RepositorioRemedio repositorioRemedio,
            RepositorioFuncionario repositorioFuncionario, TelaPaciente telaPaciente, TelaRemedio telaRemedio, TelaFuncionario telaFuncionario, Validador validador)
        {
            this.repositorioRequisicao = repositorioRequisicao;
            repositorioBase = repositorioRequisicao;
            this.repositorioPaciente = repositorioPaciente;
            this.repositorioRemedio = repositorioRemedio;
            this.repositorioFuncionario = repositorioFuncionario;
            this.telaPaciente = telaPaciente;
            this.telaRemedio = telaRemedio;
            this.telaFuncionario = telaFuncionario;
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
                Console.WriteLine("   2  - Para visualizar todas as suas contas.                                                 ");
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
            if (repositorioRemedio.GetAll().Count == 0)
            {
                ExibirMensagem("\n   Nenhum remédio cadastrado. " +
                    "\n   Você deve cadastrar um remédio para poder realizar uma requisição.", ConsoleColor.DarkRed);
                return;
            }
            if (repositorioPaciente.GetAll().Count == 0)
            {
                ExibirMensagem("\n   Nenhum paciente cadastrado. " +
                    "\n   Você deve cadastrar um paciente para poder realizar uma requisição.", ConsoleColor.DarkRed);
                return;
            }
            if (repositorioFuncionario.GetAll().Count == 0)
            {
                ExibirMensagem("\n   Nenhum funcionário cadastrado. " +
                    "\n   Você deve cadastrar um funcionário para poder realizar uma requisição.", ConsoleColor.DarkRed);
                return;
            }

            Imput(out Paciente paciente, out InformacoesReposicao informacoesReposicao, out int quantidadeRequisitada, out string senhaImputada);

            Requisicao toAdd = new(paciente, informacoesReposicao, quantidadeRequisitada);

            string valido = validador.ValidarRequisicao(toAdd, senhaImputada);

            if (valido == "REGISTRO_REALIZADO")
            {
                repositorioRequisicao.Insert(toAdd);
                ExibirMensagem("\n   Requisição realizada com sucesso!", ConsoleColor.DarkGreen);
            }
            else
            {
                ExibirMensagem("\n   Requisição Não Realizada: " + valido, ConsoleColor.DarkRed);
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

        private void Imput(out Paciente paciente, out InformacoesReposicao informacoesReposicao, out int quantidadeRequisitada, out string senhaImputada)
        {
            Console.Clear();

            paciente = (Paciente)repositorioPaciente.GetById(telaPaciente.ObterId(repositorioPaciente));

            informacoesReposicao.remedio = (Remedio)repositorioRemedio.GetById(telaRemedio.ObterId(repositorioRemedio));
            informacoesReposicao.data = DateTime.Now.Date;

            Console.Write("\n   Digite a quatidade de unidades que o paciente deseja desse remédio: ");
            while (!int.TryParse(Console.ReadLine(), out quantidadeRequisitada))
            {
                ExibirMensagem("\n   Entrada inválida! Digite um número inteiro. ", ConsoleColor.DarkRed);
                Console.Write("\n   Digite a quatidade de unidades que o paciente deseja desse remédio: ");
            }
            informacoesReposicao.funcionario = (Funcionario)repositorioFuncionario.GetById(telaFuncionario.ObterId(repositorioFuncionario));
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
            Console.WriteLine("                                               Lista de Contas!                                                       ");
            Console.WriteLine("______________________________________________________________________________________________________________________");
            Console.WriteLine();
            Console.WriteLine("{0,-5}|{1,-25}|{2,-25}|{3,-25}|{4,-25}", "ID ", "  MESA ", "  GARÇOM ", "  VALOR TOTAL ", "  STATUS ");
            Console.WriteLine("______________________________________________________________________________________________________________________");
            Console.WriteLine();

            foreach (Conta print in repositorioConta.GetAll())
            {
                if (print != null)
                {
                    Console.WriteLine("{0,-5}|{1,-25}|{2,-25}|{3,-25}|{4,-25}", print.id, print.mesa.id, print.mesa.garcon.informacoesPessoais.nome, print.valorFinal, print.mesa.status);
                }
            }
        }
        public void MostarListaContasEncerradas(RepositorioConta repositorioConta)
        {
            Console.Clear();
            Console.WriteLine("______________________________________________________________________________________________________________________");
            Console.WriteLine();
            Console.WriteLine("                                               Lista de Contas!                                                       ");
            Console.WriteLine("______________________________________________________________________________________________________________________");
            Console.WriteLine();
            Console.WriteLine("{0,-5}|{1,-25}|{2,-25}|{3,-25}|{4,-25}", "ID ", "  MESA ", "  GARÇOM ", "  VALOR TOTAL ", "  STATUS ");
            Console.WriteLine("______________________________________________________________________________________________________________________");
            Console.WriteLine();

            foreach (Conta print in repositorioConta.GetAll())
            {
                if (print != null)
                {
                    Console.WriteLine("{0,-5}|{1,-25}|{2,-25}|{3,-25}|{4,-25}", print.id, print.mesa.id, print.mesa.garcon.informacoesPessoais.nome, print.valorFinal, print.mesa.status);
                }
            }
        }

        public void MostarListaContas(RepositorioConta repositorioConta)
        {
            Console.Clear();
            Console.WriteLine("______________________________________________________________________________________________________________________");
            Console.WriteLine();
            Console.WriteLine("                                               Lista de Contas!                                                       ");
            Console.WriteLine("______________________________________________________________________________________________________________________");
            Console.WriteLine();
            Console.WriteLine("{0,-5}|{1,-25}|{2,-25}|{3,-25}|{4,-25}", "ID ", "  MESA ", "  GARÇOM ", "  VALOR TOTAL ", "  STATUS ");
            Console.WriteLine("______________________________________________________________________________________________________________________");
            Console.WriteLine();

            foreach (Conta print in repositorioConta.GetAll())
            {
                if (print != null)
                {
                    Console.WriteLine("{0,-5}|{1,-25}|{2,-25}|{3,-25}|{4,-25}", print.id, print.mesa.id, print.mesa.garcon.informacoesPessoais.nome, print.valorFinal, print.mesa.status);
                }
            }
        }
    }
}
