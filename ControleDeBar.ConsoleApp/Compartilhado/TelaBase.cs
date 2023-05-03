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

        //public void ExibirAvisos(RepositorioRemedio repositorioRemedio)
        //{
        //    Console.ForegroundColor = ConsoleColor.DarkRed;
        //    foreach (Remedio r in repositorioRemedio.GetAll())
        //    {
        //        if (r != null && r.quantidadeDisponivel == 0 && r.quantidadeDisponivel <= r.quantidadeMinima
        //            && r.quantidadeDisponivel != 0)
        //        {
        //            Console.Write("\n   Você possuí remédios com o estoque zerado ou em pouca quantidade. " +
        //                "\n   Realize uma aquisição para repor seu estoque. ");
        //            Console.ReadLine();
        //            return;
        //        }
        //        else if (r != null && r.quantidadeDisponivel == 0)
        //        {
        //            Console.Write("\n   Você possuí remédios com o estoque zerado. " +
        //                "\n   Realize uma aquisição para repor seus remédios com o estoque zerado. ");
        //            Console.ReadLine();
        //            return;
        //        }
        //        else if (r != null && r.quantidadeDisponivel <= r.quantidadeMinima && r.quantidadeDisponivel != 0)
        //        {
        //            Console.Write("\n   Você possuí remédios em pouca quantidade em estoque. " +
        //                "\n   Realize uma aquisição para repor seus remédios em pouca quantidade. ");
        //            Console.ReadLine();
        //            return;
        //        }
        //    }            
        //    Console.ForegroundColor = ConsoleColor.Cyan;
        //}

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
