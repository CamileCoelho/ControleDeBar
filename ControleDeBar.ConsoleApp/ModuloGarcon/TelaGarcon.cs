using ControleDeBar.ConsoleApp.Compartilhado;
using ControleDeBar.ConsoleApp.ModuloFuncionario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleDeBar.ConsoleApp.ModuloGarcon
{
    public class TelaGarcon : TelaBase<Garcon>
    {
        RepositorioBase<Garcon> repositorioBase;
        RepositorioGarcon repositorioGarcon;
        RepositorioBase<Funcionario> repositorioFuncionario;
        TelaFuncionario telaFuncionario;

        public TelaGarcon(RepositorioGarcon repositorioGarcon, RepositorioFuncionario repositorioFuncionario, 
            TelaFuncionario telaFuncionario, Validador validador)
        {
            this.repositorioGarcon = repositorioGarcon;
            repositorioBase = repositorioGarcon;
            this.repositorioFuncionario = repositorioFuncionario;
            this.telaFuncionario = telaFuncionario;
            this.validador = validador;
        }

        public void VisualizarTela()
        {
            bool continuar = true;

            do
            {
                string opcao = MostrarMenuGarcom();

                switch (opcao)
                {
                    case "4":
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
                }
            } while (continuar);
            string MostrarMenuGarcom()
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Clear();
                Console.WriteLine("__________________________________________________________________________________");
                Console.WriteLine();
                Console.WriteLine("                                Gestão de Garçons!                                ");
                Console.WriteLine("__________________________________________________________________________________");
                Console.WriteLine();
                Console.WriteLine("   Digite:                                                                        ");
                Console.WriteLine();
                Console.WriteLine("   1  - Para cadastrar um garçom.                                                 ");
                Console.WriteLine();
                Console.WriteLine("   2  - Para visualizar seus garçons.                                             ");
                Console.WriteLine(); 
                Console.WriteLine("   3  - Para editar um garçom.                                                    ");
                Console.WriteLine();
                Console.WriteLine(); 
                Console.WriteLine("   4  - Para voltar ao menu principal.                                            ");
                Console.WriteLine("__________________________________________________________________________________");
                Console.WriteLine();
                Console.Write("   Opção escolhida: ");
                string opcao = Console.ReadLine().ToUpper();
                bool opcaoInvalida = opcao != "1" && opcao != "2" && opcao != "3" && opcao != "4";
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

        private void Cadastrar()
        {
            if (repositorioFuncionario.GetAll().Count == 0)
            {
                ExibirMensagem("\n   Nenhum funcionário cadastrado. " +
                    "\n   Você deve cadastrar um funcionário para tipá-lo como garçon.", ConsoleColor.DarkRed);
                return;
            }

            Imput(out Funcionario funcionario, out string senhaImputada);

            Garcon toAdd = new(funcionario);

            string valido = validador.ValidarGarcon(toAdd, senhaImputada);

            if (valido == "REGISTRO_REALIZADO")
            {
                repositorioGarcon.Insert(toAdd);
                ExibirMensagem("\n   Funcionário promovido a garçon.", ConsoleColor.DarkGreen);
            }
            else
            {
                ExibirMensagem("\n   Funcionário não promovido a garçon: " + valido, ConsoleColor.DarkRed);
            }
        }

        private void Visualizar()
        {
            if (repositorioGarcon.GetAll().Count == 0)
            {
                ExibirMensagem("\n   Nenhum garçom encontrado. " +
                    "\n   Você deve realizar o cadastro de um garçom para poder visualizar seus garçons.", ConsoleColor.DarkRed);
                return;
            }
            MostarListaGarcom(repositorioGarcon);
            Console.ReadLine();
        }

        private void Editar()
        {
            if (repositorioGarcon.GetAll().Count == 0)
            {
                ExibirMensagem("\n   Nenhum garçom encontrado. " +
                    "\n   Você deve realizar o cadastro de um garçom para poder editar seus garçons.", ConsoleColor.DarkRed);
                return;
            }

            Garcon toEdit = (Garcon)repositorioBase.GetById(ObterId(repositorioGarcon));

            if (toEdit == null)
            {
                ExibirMensagem("\n   Garçom não encontrado!", ConsoleColor.DarkRed);
                return;
            }
            else
            {
                Imput(out Funcionario funcionario, out string senhaImputada);

                Garcon imput = new(funcionario);

                string valido = validador.ValidarGarcon(toEdit, senhaImputada);

                if (valido == "REGISTRO_REALIZADO")
                {
                    repositorioGarcon.Update(toEdit, imput);
                    ExibirMensagem("\n   Garçom editado com sucesso!", ConsoleColor.DarkGreen);
                }
                else
                {
                    ExibirMensagem("\n   Garçom Não Editado:" + valido, ConsoleColor.DarkRed);
                }
            }
        }

        private void Imput(out Funcionario funcionario, out string senhaImputada)
        {
            funcionario = (Funcionario)repositorioFuncionario.GetById(telaFuncionario.ObterId(repositorioFuncionario));
            Console.Write("\n   Digite a senha: ");
            senhaImputada = Console.ReadLine();
        }

        public override int ObterId(RepositorioBase<Garcon> repositorioBase)
        {
            Console.Clear();
            MostarListaGarcom(repositorioGarcon);

            Console.Write("\n   Digite o id do garçon: ");
            int id;
            while (!int.TryParse(Console.ReadLine(), out id))
            {
                ExibirMensagem("\n   Entrada inválida! Digite um número inteiro. ", ConsoleColor.DarkRed);
                Console.Write("\n   Digite o id do garçon ");
            }
            return id;
        }

        public void MostarListaGarcom(RepositorioGarcon repositorioGarcon)
        {
            Console.Clear();
            Console.WriteLine("_____________________________________________________________________________________________");
            Console.WriteLine(); 
            Console.WriteLine("                                     Lista de Garçons!                                       ");
            Console.WriteLine("_____________________________________________________________________________________________");
            Console.WriteLine();
            Console.WriteLine("{0,-5}|{1,-25}|{2,-25}|{3,-25}", "ID ", "  NOME ", "  TELEFONE ", "  CPF ");
            Console.WriteLine("_____________________________________________________________________________________________");
            Console.WriteLine();

            foreach (Garcon print in repositorioGarcon.GetAll())
            {
                if (print != null)
                
                    Console.WriteLine("{0,-5}|{1,-25}|{2,-25}|{3,-25}", print.id, print.informacoesPessoais.nome, print.informacoesPessoais.telefone, print.informacoesPessoais.cpf);
                }
            }
        
    }
}
