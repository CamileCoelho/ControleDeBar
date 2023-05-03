namespace ControleDeBar.ConsoleApp.Compartilhado
{
    public abstract class TelaBase<T> where T : EntidadeBase
    {
        public string nomeEntidade { get; set; }
        public string sufixo { get; set; }

        public RepositorioBase<EntidadeBase> repositorioBase;

        public Validador validador;

        public void ExibirMensagem(string mensagem, ConsoleColor cor)
        {
            Console.ForegroundColor = cor;
            Console.WriteLine(mensagem);
            Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.Cyan;
        }
        protected string MostrarMenu()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Clear();
            Console.WriteLine("____________________________________________________________________________________________________");
            Console.WriteLine();
            Console.WriteLine($"                                     Gestão de {nomeEntidade}{sufixo}!                                    ");
            Console.WriteLine("____________________________________________________________________________________________________");
            Console.WriteLine();
            Console.WriteLine("   Digite:                                                                                          ");
            Console.WriteLine();
            Console.WriteLine($"   1  - Para cadastrar um {nomeEntidade}.                                                          ");
            Console.WriteLine($"   2  - Para visualizar seus {nomeEntidade}{sufixo} cadastrados.                                          ");
            Console.WriteLine($"   3  - Para editar o cadastro de um {nomeEntidade}.                                               ");
            Console.WriteLine($"   4  - Para excluir o cadastro de um {nomeEntidade}.                                              ");
            Console.WriteLine();
            Console.WriteLine("   5  - Para voltar ao menu principal.                                                              ");
            Console.WriteLine("____________________________________________________________________________________________________");
            Console.WriteLine();
            Console.Write("   Opção escolhida: ");
            string opcao = Console.ReadLine().ToUpper();
            bool opcaoInvalida = opcao != "1" && opcao != "2" && opcao != "3" && opcao != "4" && opcao != "5";
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

        public abstract int ObterId(RepositorioBase<T> repositorioBase);

    }
}
