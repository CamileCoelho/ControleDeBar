using ControleDeBar.ConsoleApp.Compartilhado;

namespace ControleDeBar.ConsoleApp.ModuloFuncionario
{
    public class TelaFuncionario : TelaBase<Funcionario>
    {
        protected RepositorioBase<Funcionario> repositorioBase;
        protected RepositorioFuncionario repositorioFuncionario;

        public TelaFuncionario(RepositorioFuncionario repositorioFuncionario, Validador validador)
        {
            nomeEntidade = "funcionário";
            sufixo = "s";
            this.repositorioFuncionario = repositorioFuncionario;
            repositorioBase = repositorioFuncionario;
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
            Imput(out InformacoesPessoais informacoesPessoais, out string senha);

            Funcionario toAdd = new(informacoesPessoais, senha);

            string valido = validador.ValidarFunicionario(toAdd);

            if (valido == "REGISTRO_REALIZADO" )
            {
                repositorioFuncionario.Insert(toAdd);
                ExibirMensagem("\n   Funcionário cadastrado com sucesso!" , ConsoleColor.DarkGreen);
            }
            else
            {
                ExibirMensagem("\n   Funcionário Não Cadastrado: " + valido, ConsoleColor.DarkRed);
            }
        }

        private void Visualizar()
        {
            if (repositorioFuncionario.GetAll().Count == 0)
            {
                ExibirMensagem("\n   Nenhum funcionário cadastrado. " +
                    "\n   Você deve cadastrar um funcionário para poder visualizar seus cadastros.", ConsoleColor.DarkRed);
                return;
            }
            MostarListaFuncionarios(repositorioFuncionario);
            Console.ReadLine();
        }

        private void Editar()
        {
            if (repositorioFuncionario.GetAll().Count == 0)
            {
                ExibirMensagem("\n   Nenhum funcionário cadastrado. " +
                    "\n   Você deve cadastrar um funcionário para poder editar seus cadastros.", ConsoleColor.DarkRed);
                return;
            }

            Funcionario toEdit = (Funcionario)repositorioBase.GetById(ObterId(repositorioFuncionario));

            if (toEdit == null)
            {
                ExibirMensagem("\n   Funcionário não encontrado!", ConsoleColor.DarkRed);
            }
            else
            {
                Imput(out InformacoesPessoais informacoesPessoais, out string senha);

                Funcionario imput = new(informacoesPessoais, senha);

                string valido = validador.ValidarFunicionario(imput);

                if (valido == "REGISTRO_REALIZADO")
                {
                    repositorioBase.Update(toEdit, imput);
                    ExibirMensagem("\n   Funcionario editado com sucesso!", ConsoleColor.DarkGreen);
                }
                else
                {
                    ExibirMensagem("\n   Funcionário Não Editado:" + valido, ConsoleColor.DarkRed);
                }
            }
        }

        private void Excluir()
        {
            if (repositorioFuncionario.GetAll().Count == 0)
            {
                ExibirMensagem("\n   Nenhum funcionário cadastrado. " +
                    "\n   Você deve cadastrar um funcionário para poder excluir seu cadastro.", ConsoleColor.DarkRed);
                return;
            }

            Funcionario toDelete = (Funcionario)repositorioBase.GetById(ObterId(repositorioFuncionario));

            string valido = validador.PermitirExclusaoDoFuncionario(toDelete);

            if (toDelete != null && valido == "SUCESSO!")
            {
                repositorioFuncionario.Delete(toDelete);
                ExibirMensagem("\n   Funcionário excluido com sucesso! ", ConsoleColor.DarkGreen);
            }
            else
            {
                ExibirMensagem("\n   Funcionario não excluido:" + valido, ConsoleColor.DarkRed);
            }
        }

        public void Imput(out InformacoesPessoais informacoesPessoais, out string senha)
        {
            informacoesPessoais = new InformacoesPessoais();
            Console.Clear();
            Console.Write("\n   Digite seu nome : ");
            informacoesPessoais.nome = Console.ReadLine();
            Console.Write("\n   Digite seu telefone (XX)XXXXX-XXXX: ");
            informacoesPessoais.telefone = Console.ReadLine();
            Console.Write("\n   Digite seu endereco: ");
            informacoesPessoais.endereco = Console.ReadLine();
            Console.Write("\n   Digite seu CPF (XXX.XXX.XXX-XX ou XXXXXXXXXXX): ");
            informacoesPessoais.cpf = Console.ReadLine();
            Console.Write("\n   Digite sua senha XXXX : ");
            senha = Console.ReadLine();
        }

        public override int ObterId(RepositorioBase<Funcionario> repositorioBase)
        {
            Console.Clear();
            MostarListaFuncionarios(repositorioFuncionario);

            Console.Write("\n   Digite o id do funcionario: ");
            int id;
            while (!int.TryParse(Console.ReadLine(), out id))
            {
                ExibirMensagem("\n   Entrada inválida! Digite um número inteiro. ", ConsoleColor.DarkRed);
                Console.Write("\n   Digite o id do funcionario: ");
            }
            return id;
        }

        public void MostarListaFuncionarios(RepositorioFuncionario repositorioFuncionario)
        {
            Console.Clear();
            Console.WriteLine("_____________________________________________________________________________________________");
            Console.WriteLine();
            Console.WriteLine("                                   Lista de Funcionarios                                     ");
            Console.WriteLine("_____________________________________________________________________________________________");
            Console.WriteLine();
            Console.WriteLine("{0,-5}|{1,-25}|{2,-25}|{3,-25}", "ID ", "  NOME ", "  TELEFONE ", "  CPF "); 
            Console.WriteLine("_____________________________________________________________________________________________");
            Console.WriteLine();

            foreach (Funcionario print in repositorioFuncionario.GetAll())
            {
                if (print != null)
                {
                    Console.WriteLine("{0,-5}|{1,-25}|{2,-25}|{3,-25}" , print.id, print.informacoesPessoais.nome, print.informacoesPessoais.telefone, print.informacoesPessoais.cpf);
                }
            }
        }
    }
}
