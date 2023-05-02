namespace ControleDeBar.ConsoleApp.Compartilhado
{
    public class InformacoesPessoais 
    {
        public string nome { get; set; }
        public string telefone { get; set; }
        public string endereco { get; set; }
        public string cpf { get; set; }

        public InformacoesPessoais()
        {
            
        }
        public InformacoesPessoais(string nome, string telefone, string endereco, string cpf)
        {
            this.nome = nome;
            this.telefone = telefone;
            this.endereco = endereco;
            this.cpf = cpf;
        }        
    }
}
