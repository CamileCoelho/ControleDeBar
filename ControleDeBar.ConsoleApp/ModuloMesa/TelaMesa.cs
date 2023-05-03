using ControleDeBar.ConsoleApp.Compartilhado;
using ControleDeBar.ConsoleApp.ModuloFuncionario;
using ControleDeBar.ConsoleApp.ModuloGarcon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleDeBar.ConsoleApp.ModuloMesa
{
    public class TelaMesa : TelaBase<Mesa>
    {
        RepositorioBase<Mesa> repositorioBase;
        RepositorioMesa repositorioMesa;
        RepositorioBase<Garcon> repositorioGarcon;
        TelaGarcon telaGarcon;

        public TelaMesa(RepositorioMesa repositorioMesa, RepositorioGarcon repositorioGarcon, TelaGarcon telaGarcon, Validador validador)
        {
            nomeEntidade = "mesa";
            sufixo = "s";
            this.repositorioMesa = repositorioMesa;
            repositorioBase = repositorioMesa;
            this.repositorioGarcon = repositorioGarcon;
            this.telaGarcon = telaGarcon;
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
            if (repositorioGarcon.GetAll().Count == 0)
            {
                ExibirMensagem("\n   Nenhum garçom cadastrado. " +
                    "\n   Você deve cadastrar um garçom para poder cadastrar uma mesa.", ConsoleColor.DarkRed);
                return;
            }

            Imput(out Garcon garcon, out string senhaImputada);

            Mesa toAdd = new (garcon);

            string valido = validador.ValidarMesa(toAdd, senhaImputada);

            if (valido == "REGISTRO_REALIZADO")
            {
                repositorioMesa.Insert(toAdd);
                ExibirMensagem("\n   Mesa cadastrada com sucesso!", ConsoleColor.DarkGreen);
            }
            else
            {
                ExibirMensagem("\n   Mesa Não Cadastrada: " + valido, ConsoleColor.DarkRed);
            }
        }

        private void Visualizar()
        {
            if (repositorioMesa.GetAll().Count == 0)
            {
                ExibirMensagem("\n   Nenhuma mesa cadastrada. " +
                    "\n   Você deve cadastrar uma mesa para poder visualizar seus cadastros.", ConsoleColor.DarkRed);
                return;
            }
            MostarListaMesas(repositorioMesa);
            Console.ReadLine();
        }

        private void Editar()
        {
            if (repositorioMesa.GetAll().Count == 0)
            {
                ExibirMensagem("\n   Nenhuma mesa cadastrada. " +
                    "\n   Você deve cadastrar uma mesa para poder editar uma mesa.", ConsoleColor.DarkRed);
                return;
            }

            Mesa toEdit = (Mesa)repositorioMesa.GetById(ObterId(repositorioMesa));

            if (toEdit == null)
            {
                ExibirMensagem("\n   Mesa não encontrada!", ConsoleColor.DarkRed);
            }
            else
            {
                Imput(out Garcon garcon, out string senhaImputada);

                Mesa imput = new(garcon);

                string valido = validador.ValidarMesa(imput, senhaImputada);

                if (valido == "REGISTRO_REALIZADO")
                {
                    repositorioMesa.Update(toEdit, imput);
                    ExibirMensagem("\n   Mesa editada com sucesso!", ConsoleColor.DarkGreen);
                }
                else
                {
                    ExibirMensagem("\n   Mesa Não Editada:" + valido, ConsoleColor.DarkRed);
                }
            }
        }

        private void Excluir()
        {
            if (repositorioMesa.GetAll().Count == 0)
            {
                ExibirMensagem("\n   Nenhuma mesa cadastrada. " +
                    "\n   Você deve cadastrar uma mesa para poder excluir um cadastro.", ConsoleColor.DarkRed);
                return;
            }

            Mesa toDelete = (Mesa)repositorioBase.GetById(ObterId(repositorioMesa));

            string valido = validador.PermitirExclusaoDaMesa(toDelete);

            if (valido == "SUCESSO!")
            {
                repositorioMesa.Delete(toDelete);
                ExibirMensagem("\n   Mesa excluida com sucesso! ", ConsoleColor.DarkGreen);
            }
            else
            {
                ExibirMensagem(valido, ConsoleColor.DarkRed);
            }
        }

        public void Imput(out Garcon garcon, out string senhaImputada)
        {
            Console.Clear();
            garcon = (Garcon)repositorioGarcon.GetById(telaGarcon.ObterId(repositorioGarcon));

            Console.Write("\n   Digite a senha: ");
            senhaImputada = Console.ReadLine();
        }

        public void MostarListaMesas(RepositorioMesa repositorioMesa)
        {
            Console.Clear();
            Console.WriteLine("___________________________________________________________");
            Console.WriteLine();
            Console.WriteLine("                  Lista de Mesas                           ");
            Console.WriteLine("___________________________________________________________");
            Console.WriteLine();
            Console.WriteLine("{0,-10}|{1,-25}|{2,-25}", "NÚMERO ", "  GARÇOM ", "  STATUS ");
            Console.WriteLine("___________________________________________________________");
            Console.WriteLine();

            foreach (Mesa print in repositorioMesa.GetAll())
            {
                if (print != null)
                {
                    Console.WriteLine("{0,-10}|{1,-25}|{2,-25}", print.id, print.garcon.informacoesPessoais.nome, print.status);
                }
            }
        }

        public override int ObterId(RepositorioBase<Mesa> repositorioBase)
        {
            Console.Clear();
            MostarListaMesas(repositorioMesa);

            Console.Write("\n   Digite o número da mesa: ");
            int id;
            while (!int.TryParse(Console.ReadLine(), out id))
            {
                ExibirMensagem("\n   Entrada inválida! Digite um número inteiro. ", ConsoleColor.DarkRed);
                Console.Write("\n   Digite o número da mesa: ");
            }
            return id;
        }
    }
}
