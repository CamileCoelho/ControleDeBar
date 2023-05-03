using ControleDeBar.ConsoleApp.Compartilhado;
using ControleDeBar.ConsoleApp.ModuloFuncionario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleDeBar.ConsoleApp.ModuloProduto
{
    public class TelaProduto : TelaBase<Produto>
    {
        RepositorioBase<Produto> repositorioBase;
        RepositorioProduto repositorioProduto;
        RepositorioBase<Funcionario> repositorioFuncionario;
        TelaFuncionario telaFuncionario;
        Validador validador;

        public TelaProduto(RepositorioProduto repositorioProduto, RepositorioFuncionario repositorioFuncionario, TelaFuncionario telaFuncionario, Validador validador)
        {
            nomeEntidade = "produto";
            sufixo = "s";
            this.repositorioProduto = repositorioProduto;
            repositorioBase = repositorioProduto;
            this.repositorioFuncionario = repositorioFuncionario;
            this.telaFuncionario = telaFuncionario;
            this.validador = validador;
        }

        public void VisualizarTela()
        {
            bool continuar = true;

            do
            {
                string opcao = MostrarMenu();

                switch (opcao)
                {
                    case "5":
                        continuar = false;
                        Console.ResetColor();
                        break;
                    case "1":
                        Cadastrar();
                        continue;
                    case "2":
                        Visualizar();
                        continue;
                    case "3":
                        Editar();
                        continue;
                    case "4":
                        Excluir();
                        continue;
                }
            } while (continuar);
        }

        private void Cadastrar()
        {
            if (repositorioFuncionario.GetAll().Count == 0)
            {
                ExibirMensagem("\n   Nenhum funcionário cadastrado. " +
                    "\n   Você deve cadastrar um funcionário para poder cadastrar um produto.", ConsoleColor.DarkRed);
                return;
            }

            Imput(out string nome, out string descricao, out decimal preco, out Funcionario funcionario, out string senhaImputada);

            Produto toAdd = new(nome, descricao, preco);

            string valido = validador.ValidarProduto(toAdd, funcionario, preco, senhaImputada);

            if (valido == "REGISTRO_REALIZADO")
            {
                repositorioProduto.Insert(toAdd);
                ExibirMensagem("\n   Produto cadastrado com sucesso!", ConsoleColor.DarkGreen);
            }
            else
            {
                ExibirMensagem("\n   Produto Não Cadastrado: " + valido, ConsoleColor.DarkRed);
            }
        }

        private void Visualizar()
        {
            if (repositorioProduto.GetAll().Count == 0)
            {
                ExibirMensagem("\n   Nenhum produto cadastrado. " +
                    "\n   Você deve cadastrar um produto para poder visualizar seus cadastros.", ConsoleColor.DarkRed);
                return;
            }
            MostarListaProdutos(repositorioProduto);
            Console.ReadLine();
        }

        private void Editar()
        {
            if (repositorioProduto.GetAll().Count == 0)
            {
                ExibirMensagem("\n   Nenhum produto cadastrado. " +
                    "\n   Você deve cadastrar um produto para poder editar seus produtos.", ConsoleColor.DarkRed);
                return;
            }

            Produto toEdit = (Produto)repositorioBase.GetById(ObterId(repositorioProduto));

            if (toEdit == null)
            {
                ExibirMensagem("\n   Produto não encontrado!", ConsoleColor.DarkRed);
            }
            else
            {
                Imput(out string nome, out string descricao, out decimal preco, out Funcionario funcionario, out string senhaImputada);

                Produto imput = new(nome, descricao, preco);

                string valido = validador.ValidarProduto(imput, funcionario, preco, senhaImputada);

                if (valido == "REGISTRO_REALIZADO")
                {
                    repositorioBase.Update(toEdit, imput);
                    ExibirMensagem("\n   Produto editado com sucesso!", ConsoleColor.DarkGreen);
                }
                else
                {
                    ExibirMensagem("\n   Produto Não Editado:" + valido, ConsoleColor.DarkRed);
                }
            }
        }

        private void Excluir()
        {
            if (repositorioProduto.GetAll().Count == 0)
            {
                ExibirMensagem("\n   Nenhum produto cadastrado. " +
                    "\n   Você deve cadastrar um produto para poder excluir seu cadastro.", ConsoleColor.DarkRed);
                return;
            }

            Produto toDelete = (Produto)repositorioBase.GetById(ObterId(repositorioProduto));

            string valido = validador.PermitirExclusaoDoProduto(toDelete);

            if (toDelete != null && valido == "SUCESSO!")
            {
                repositorioProduto.Delete(toDelete);
                ExibirMensagem("\n   Produto excluido com sucesso! ", ConsoleColor.DarkGreen);
            }
            else
            {
                ExibirMensagem("\n   Produto não excluido:" + valido, ConsoleColor.DarkRed);
            }
        }

        public void Imput(out string nome, out string descricao, out decimal preco, out Funcionario funcionario, out string senhaImputada)
        {
            Console.Clear();
            Console.Write("\n   Digite seu nome : ");
            nome = Console.ReadLine();
            Console.Write("\n   Digite sua descrição: ");
            descricao = Console.ReadLine();
            Console.Write("\n   Digite o preço desse produto: ");
            while (!decimal.TryParse(Console.ReadLine(), out preco))
            {
                ExibirMensagem("\n   Entrada inválida! Digite um número. ", ConsoleColor.DarkRed);
                Console.Write("\n   Digite o preço desse produto: ");
            }

            funcionario = (Funcionario)repositorioFuncionario.GetById(telaFuncionario.ObterId(repositorioFuncionario));
            Console.Write("\n   Digite a senha: ");
            senhaImputada = Console.ReadLine();
        }

        public override int ObterId(RepositorioBase<Produto> repositorioBase)
        {
            Console.Clear();
            MostarListaProdutos(repositorioProduto);

            Console.Write("\n   Digite o id do produto: ");
            int id;
            while (!int.TryParse(Console.ReadLine(), out id))
            {
                ExibirMensagem("\n   Entrada inválida! Digite um número inteiro. ", ConsoleColor.DarkRed);
                Console.Write("\n   Digite o id do produto: ");
            }
            return id;
        }

        public void MostarListaProdutos(RepositorioProduto repositorioProduto)
        {
            Console.Clear();
            Console.WriteLine("_____________________________________________________________________________________________");
            Console.WriteLine();
            Console.WriteLine("                                     Lista de Produtos                                       ");
            Console.WriteLine("_____________________________________________________________________________________________");
            Console.WriteLine();
            Console.WriteLine("{0,-5}|{1,-25}|{2,-35}|{3,-25}", "ID ", "  NOME ", "  DESCRIÇÃO ", "  PREÇO  ");
            Console.WriteLine("_____________________________________________________________________________________________");
            Console.WriteLine();

            foreach (Produto print in repositorioProduto.GetAll())
            {
                if (print != null)
                {
                    Console.WriteLine("{0,-5}|{1,-25}|{2,-35}|R$ {3,-25}", print.id, print.nome, print.descricao, print.preco);
                }
            }
        }
    }
}
