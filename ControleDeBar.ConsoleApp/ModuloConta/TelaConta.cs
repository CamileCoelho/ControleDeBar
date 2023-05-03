using ControleDeBar.ConsoleApp.Compartilhado;
using ControleDeBar.ConsoleApp.ModuloFuncionario;
using ControleDeBar.ConsoleApp.ModuloGarcon;
using ControleDeBar.ConsoleApp.ModuloMesa;
using ControleDeBar.ConsoleApp.ModuloProduto;
using System;
using System.Collections.Generic;
using System.Globalization;
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
                    case "8":
                        continuar = false;
                        Console.ResetColor();
                        break;
                    case "1":
                        Open();
                        continue;
                    case "2":
                        AddPedidos();
                        continue;
                    case "3":
                        RemovePedidos();
                        continue;
                    case "4":
                        End();
                        continue;
                    case "5":
                        VisualizarEmAberto();
                        continue;
                    case "6":
                        VisualizarEncerradas();
                        continue;
                    case "7":
                        Visualizar();
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
                Console.WriteLine("   2  - Para adicionar pedidos a uma conta.                                                   ");
                Console.WriteLine();
                Console.WriteLine("   3  - Para remover pedidos de uma conta.                                                    ");
                Console.WriteLine();
                Console.WriteLine("   4  - Para fechar uma conta.                                                                ");
                Console.WriteLine();
                Console.WriteLine("   5  - Para visualizar suas contas em aberto.                                                ");
                Console.WriteLine();
                Console.WriteLine("   6  - Para visualizar suas contas encerradas.                                               ");
                Console.WriteLine();
                Console.WriteLine("   7  - Para visualizar as suas contas diárias.                                               ");
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("   8  - Para voltar ao menu principal.                                                        ");
                Console.WriteLine("______________________________________________________________________________________________");
                Console.WriteLine();
                Console.Write("   Opção escolhida: ");
                string opcao = Console.ReadLine().ToUpper();
                bool opcaoInvalida = opcao != "1" && opcao != "2" && opcao != "3" && opcao != "4" && opcao != "5" && opcao != "6" && opcao != "7" && opcao != "8";
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
        
        private void Open()
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

            Imput(out Mesa mesa, out string senhaImputada);

            string valido = validador.ValidarContaOpen(mesa, senhaImputada);

            Conta toAdd = null;
            if (mesa == null)
            {
                ExibirMensagem("\n   Conta Não Aberta: MESA_INEXISTENTE " , ConsoleColor.DarkRed);
                return;
            }
            else
                toAdd = new(mesa);

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

        private void AddPedidos()
        {
            if (repositorioConta.GetAll().Count == 0)
            {
                ExibirMensagem("\n   Nenhuma conta aberta. " +
                    "\n   Você deve abrir uma conta para poder adicionar pedidos a uma conta.", ConsoleColor.DarkRed);
                return;
            }
            if (repositorioConta.GetAll().Any(x => x.status == "EM ABERTO"))
            {
                Conta toUpdate = (Conta)repositorioBase.GetById(ObterIdContaAberta(repositorioConta));

                List<Produto> produtos = new();

                string valido = validador.ValidarContaExistente(toUpdate);

                if (valido == "REGISTRO_REALIZADO")
                {
                    do
                    {
                        Produto produto = (Produto)repositorioProduto.GetById(telaProduto.ObterId(repositorioProduto));
                        string nome;

                        if (produto != null)
                            nome = produto.nome;
                        else
                        {
                            ExibirMensagem("\n   Pedido não adicionado:  PRODUTO_INEXISTENTE ", ConsoleColor.DarkRed);
                            return;
                        }

                        decimal valor = produto.preco;
                        int quantidade;
                        Console.Write("\n   Digite a quatidade de unidades que o cliente deseja desse produto: ");
                        while (!int.TryParse(Console.ReadLine(), out quantidade))
                        {
                            ExibirMensagem("\n   Entrada inválida! Digite um número inteiro. ", ConsoleColor.DarkRed);
                            Console.Write("\n   Digite a quatidade de unidades que o unidades que o cliente deseja desse produto: ");
                        }
                        Produto produtoPedido = new(nome, valor, quantidade);
                        produtos.Add(produtoPedido);

                        Console.WriteLine("\n   Você deseja mais algum produto? se sim, digite \"S\" para continuar," +
                            "\n   caso não, digite qualquer tecla para finalizar seu pedido atual. ");
                        Console.Write("   ");
                        string saida = Console.ReadLine().ToUpper();

                        if (saida != "S")
                            break;

                    } while (true);

                    Pedido pedido = new(produtos);

                    repositorioConta.AddPedido(toUpdate, pedido);

                    ExibirMensagem("\n   Pedido adicionado com sucesso!", ConsoleColor.DarkGreen);
                }
                else
                {
                    ExibirMensagem("\n   Pedido não adicionado: " + valido, ConsoleColor.DarkRed);
                }
            }
            else
            {
                ExibirMensagem("\n   Nenhuma conta em aberto. ", ConsoleColor.DarkRed);
                return;
            }
        }

        private void RemovePedidos()
        {
            if (repositorioConta.GetAll().Count == 0)
            {
                ExibirMensagem("\n   Nenhuma conta aberta. " +
                    "\n   Você deve abrir uma conta para poder remover pedidos de uma conta.", ConsoleColor.DarkRed);
                return;
            }
            if (repositorioConta.GetAll().Any(x => x.status == "EM ABERTO"))
            {
                Conta toEdit = (Conta)repositorioBase.GetById(ObterIdContaAberta(repositorioConta));

                int id = ObterIdPedido(toEdit);
                Pedido pedido = toEdit.listaPedidos.Find(pedido => pedido.id == id);
                if (pedido == null)
                {
                    ExibirMensagem("\n   Pedido não encontrado. ", ConsoleColor.DarkRed);
                    return;                    
                }
                string valido = validador.PermitirRemocaoDoPedido(pedido, toEdit);

                if (pedido != null && valido == "SUCESSO!")
                {
                    repositorioConta.RemovePedido(toEdit, pedido);
                    ExibirMensagem("\n   Pedido removido com sucesso! ", ConsoleColor.DarkGreen);
                }
                else
                {
                    ExibirMensagem("\n   Pedido não excluido:" + valido, ConsoleColor.DarkRed);
                }
            }
            else
            {
                ExibirMensagem("\n   Nenhuma conta em aberto. ", ConsoleColor.DarkRed);
                return;
            }
}

        public void MostarListaPedidos(Conta toEdit)
        {
            Console.Clear();
            Console.WriteLine("________________________________________________________________________________________________________");
            Console.WriteLine();
            Console.WriteLine("                                     Lista de Pedidos!                                                  ");
            Console.WriteLine("________________________________________________________________________________________________________");
            Console.WriteLine();
            Console.WriteLine("{0,-5}", "ID   |  PRODUTO - QUANTIDADE");
            Console.WriteLine("________________________________________________________________________________________________________");
            Console.WriteLine();

            foreach (Pedido print in toEdit.listaPedidos)
            {
                if (print != null)
                {
                    Console.Write("{0,-5}|", print.id);

                    foreach (Produto produto in print.produtos)
                    {
                        Console.Write(produto.nome + " - " + produto.quantidade +" ");
                    }
                }
                Console.WriteLine();                
            }
        }

        public int ObterIdPedido(Conta toEdit)
        {
            Console.Clear();
            MostarListaPedidos(toEdit);

            Console.Write("\n   Digite o id do pedido: ");
            int id;
            while (!int.TryParse(Console.ReadLine(), out id))
            {
                ExibirMensagem("\n   Entrada inválida! Digite um número inteiro. ", ConsoleColor.DarkRed);
                Console.Write("\n   Digite o id do pedido: ");
            }
            return id;
        }

        private void End()
        {
            if (repositorioConta.GetAll().Count == 0)
            {
                ExibirMensagem("\n   Nenhuma conta cadastrada. " +
                    "\n   Você deve cadastrar uma conta para poder encerrar uma conta.", ConsoleColor.DarkRed);
                return;
            }
            if (repositorioConta.GetAll().All(x => x.status != "EM ABERTO"))
            {
                ExibirMensagem("\n   Nenhuma conta em aberto. ", ConsoleColor.DarkRed);
                return;
            }

            Conta toEnd = (Conta)repositorioBase.GetById(ObterId(repositorioConta));

            Console.Write("\n   Digite sua senha: ");
            string senhaImputada = Console.ReadLine();

            string valido = validador.ValidarConta(toEnd, senhaImputada);

            if (valido == "REGISTRO_REALIZADO")
            {
                repositorioConta.End(toEnd);
                ExibirMensagem("\n   Conta encerrada com sucesso!", ConsoleColor.DarkGreen);
            }
            else
            {
                ExibirMensagem("\n   Conta Não Encerrada: " + valido, ConsoleColor.DarkRed);
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

        private void VisualizarEmAberto()
        {
            if (repositorioConta.GetAll().Count == 0)
            {
                ExibirMensagem("\n   Nenhuma conta encontrada. " +
                    "\n   Você deve abrir uma conta para poder visualizar suas contas.", ConsoleColor.DarkRed);
                return;
            }
            if (repositorioConta.GetAll().Any(x => x.status == "EM ABERTO"))
            {
                MostarListaContasEmAberto(repositorioConta);
                Console.ReadLine();
            }
            else
            {
                ExibirMensagem("\n   Nenhuma conta em aberto. ", ConsoleColor.DarkRed);
                return;
            }
        }

        private void VisualizarEncerradas()
        {
            if (repositorioConta.GetAll().Count == 0)
            {
                ExibirMensagem("\n   Nenhuma conta encontrada. " +
                    "\n   Você deve abrir uma conta para poder visualizar suas contas.", ConsoleColor.DarkRed);
                return;
            }
            if (repositorioConta.GetAll().Any(x => x.status == "ENCERRADO"))
            {
                MostarListaContasEncerradas(repositorioConta);
                Console.ReadLine();
            }
            else
            {
                ExibirMensagem("\n   Nenhuma conta encerrada. ", ConsoleColor.DarkRed);
                return;
            }
        }

        private void Imput(out Mesa mesa, out string senhaImputada)
        {
            Console.Clear();
            Mesa imput = (Mesa)repositorioMesa.GetById(telaMesa.ObterId(repositorioMesa));

            if (imput != null)
            {
                Console.Write("\n   Digite sua senha: ");
                senhaImputada = Console.ReadLine();
                mesa = imput;
            }
            else
            {
                senhaImputada = null;
                mesa = null;
            }
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

        public int ObterIdContaAberta(RepositorioBase<Conta> repositorioBase)
        {
            Console.Clear();
            MostarListaContasEmAberto(repositorioConta);

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
            Console.WriteLine("{0,-5}|{1,-20}|{2,-20}|{3,-20}   |{4,-20}|{5,-20}", "ID ", "  MESA ", "  GARÇOM ", "  VALOR TOTAL ", "  STATUS ", "  DATA ");
            Console.WriteLine("______________________________________________________________________________________________________________________");
            Console.WriteLine();

            foreach (Conta print in repositorioConta.GetAll())
            {
                if (print != null && print.status == "EM ABERTO")
                {
                    Console.WriteLine("{0,-5}|{1,-20}|{2,-20}|R$ {3,-20}|{4,-20}|{5,-20}", print.id, print.mesa.id, print.mesa.garcon.informacoesPessoais.nome, print.valorFinal, print.status, print.data.ToString("dd/MM/yyyy"));
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
            Console.WriteLine("{0,-5}|{1,-20}|{2,-20}|{3,-20}   |{4,-20}|{5,-20}", "ID ", "  MESA ", "  GARÇOM ", "  VALOR TOTAL ", "  STATUS ", "  DATA ");
            Console.WriteLine("______________________________________________________________________________________________________________________");
            Console.WriteLine();

            foreach (Conta print in repositorioConta.GetAll())
            {
                if (print != null && print.status != "EM ABERTO")
                {
                    Console.WriteLine("{0,-5}|{1,-20}|{2,-20}|R$ {3,-20}|{4,-20}|{5,-20}", print.id, print.mesa.id, print.mesa.garcon.informacoesPessoais.nome, print.valorFinal, print.status, print.data.ToString("dd/MM/yyyy"));
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
            Console.WriteLine("{0,-5}|{1,-20}|{2,-20}|{3,-20}   |{4,-20}|{5,-20}", "ID ", "  MESA ", "  GARÇOM ", "  VALOR TOTAL ", "  STATUS ", "  DATA ");
            Console.WriteLine("______________________________________________________________________________________________________________________");
            Console.WriteLine();

            foreach (Conta print in repositorioConta.GetAll())
            {
                if (print != null && print.data.Date == DateTime.Today.Date)
                {
                    Console.WriteLine("{0,-5}|{1,-20}|{2,-20}|R$ {3,-20}|{4,-20}|{5,-20}", print.id, print.mesa.id, print.mesa.garcon.informacoesPessoais.nome, print.valorFinal, print.status, print.data.ToString("dd/MM/yyyy"));
                }
            }
        }
    }
}
