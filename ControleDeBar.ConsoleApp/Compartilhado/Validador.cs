using ControleDeBar.ConsoleApp.ModuloConta;
using ControleDeBar.ConsoleApp.ModuloFuncionario;
using ControleDeBar.ConsoleApp.ModuloGarcon;
using ControleDeBar.ConsoleApp.ModuloMesa;
using ControleDeBar.ConsoleApp.ModuloProduto;
using System.Text.RegularExpressions;

namespace ControleDeBar.ConsoleApp.Compartilhado
{
    public class Validador
    {
        public RepositorioFuncionario? repositorioFuncionario;
        public RepositorioGarcon? repositorioGarcon;
        public RepositorioMesa? repositorioMesa;
        public RepositorioProduto? repositorioProduto;
        public RepositorioConta? repositorioConta;

        public Validador()
        {
            
        }

        public Validador(RepositorioFuncionario? repositorioFuncionario,
        RepositorioGarcon? repositorioGarcon, RepositorioMesa? repositorioMesa, 
        RepositorioProduto? repositorioProduto, RepositorioConta? repositorioConta)
        {
            this.repositorioFuncionario = repositorioFuncionario;
            this.repositorioGarcon = repositorioGarcon;
            this.repositorioMesa = repositorioMesa;
            this.repositorioConta = repositorioConta;
            this.repositorioProduto = repositorioProduto;
        }

        public bool ValidarString(string str)
        {
            if (string.IsNullOrEmpty(str) && string.IsNullOrWhiteSpace(str))
                return true;
            else
                return false;
        }

        public bool ValidarSenha(string senha)
        {
            if (string.IsNullOrEmpty(senha) && string.IsNullOrWhiteSpace(senha) && senha.ToCharArray().Length >= 4)
                return false;
            else
                return true;
        }

        public bool ValidaTelefone(string telefone)
        {
            // formato (XX)XXXXX-XXXX
            Regex Rgx = new(@"^\(\d{2}\)\d{5}-\d{4}$");

            if (Rgx.IsMatch(telefone))
                return false;
            else
                return true;
        }

        public bool ValidaCPF(string cpf)
        {
            // formato XXX.XXX.XXX-XX ou XXXXXXXXXXX
            Regex Rgx = new(@"^\d{3}\.\d{3}\.\d{3}-\d{2}$|^\d{11}$");

            if (Rgx.IsMatch(cpf))
                return false;
            else
                return true;
        }

        public string ValidarInfoPessoal(InformacoesPessoais informacoesPessoais)
        {
            Validador valida = new();
            string mensagem = "";

            if (valida.ValidarString(informacoesPessoais.nome))
                mensagem += " NOME_INVALIDO ";

            if (informacoesPessoais.telefone == null || valida.ValidaTelefone(informacoesPessoais.telefone))
                mensagem += " TELEFONE_INVALIDO ";

            if (valida.ValidarString(informacoesPessoais.endereco))
                mensagem += " ENDERECO_INVALIDO ";

            if (informacoesPessoais.cpf == null || valida.ValidaCPF(informacoesPessoais.cpf))
                mensagem += " CPF_INVALIDO ";

            if (mensagem != "")
                return mensagem;

            return "REGISTRO_REALIZADO";
        }

        public string ValidarProduto(Produto imput, Funcionario funcionario, decimal preco, string senhaImputada)
        {
            Validador valida = new();
            string mensagem = "";

            if (valida.ValidarString(imput.nome))
                mensagem += " NOME_INVALIDO ";

            if (valida.ValidarString(imput.descricao))
                mensagem += " DESCRICAO_INVALIDA ";

            if (preco <= 0)
                mensagem += " PREÇO_INVALIDO ";

            if (funcionario == null)
                mensagem += " FUNCIONARIO_INVALIDO ";
            else if (valida.ValidarString(senhaImputada) || funcionario.senha != senhaImputada)
                mensagem += " SENHA_ERRADA ";

            if (mensagem != "")
                return mensagem;

            return "REGISTRO_REALIZADO";
        }

        public string ValidarFunicionario(Funcionario imput)
        {
            Validador valida = new();
            string mensagem = "";
            string validarInfoPessoalCB = ValidarInfoPessoal(imput.informacoesPessoais);

            if (validarInfoPessoalCB != "REGISTRO_REALIZADO")
                mensagem += validarInfoPessoalCB;

            if (valida.ValidarString(imput.senha) || valida.ValidarSenha(imput.senha) == false)
                mensagem += " SENHA_INVALIDA ";

            if (mensagem != "")
                return mensagem;

            return "REGISTRO_REALIZADO";
        }

        public string ValidarMesa(Mesa imput, string senhaImputada)
        {
            Validador valida = new();
            string mensagem = "";

            if (imput.garcon == null)
                mensagem += " GARCOM_INVALIDO ";
            else if (valida.ValidarString(senhaImputada) || imput.garcon.senha != senhaImputada)
                mensagem += " SENHA_ERRADA ";                  

            if (mensagem != "")
                return mensagem;

            return "REGISTRO_REALIZADO";
        }

        public string ValidarGarcon(Garcon toAdd, string senhaImputada)
        {
            Validador valida = new();
            string mensagem = "";

            if (toAdd == null)
                mensagem += " FUNCIONARIO_INVALIDO ";
            else if (valida.ValidarString(senhaImputada) || toAdd.senha != senhaImputada)
                mensagem += " SENHA_ERRADA ";

            if (mensagem != "")
                return mensagem;

            return "REGISTRO_REALIZADO";
        }

        public string ValidarConta(Conta toAdd, string senhaImputada)
        {
            Validador valida = new();
            string mensagem = "";

            if (toAdd == null )
                mensagem += " MESA_INVALIDA ";

            if (toAdd != null && toAdd.mesa.garcon.senha == null)
                mensagem += " FUNCIONARIO_INVALIDO ";
            else if (toAdd != null && valida.ValidarString(senhaImputada) || toAdd != null && toAdd.mesa.garcon.senha != senhaImputada)
                mensagem += " SENHA_ERRADA ";

            if (mensagem != "")
                return mensagem;

            return "REGISTRO_REALIZADO";
        }
        public string ValidarContaOpen(Mesa mesa, string senhaImputada)
        {
            Validador valida = new();
            string mensagem = "";

            if (mesa == null)
                mensagem += " MESA_INVALIDA ";
            else if (mesa.status == "INDISPONIVEL")
                mensagem += " MESA_INDISPONIVEL ";

            if (mesa.garcon.senha == null)
                mensagem += " FUNCIONARIO_INVALIDO ";
            else if (valida.ValidarString(senhaImputada) || mesa.garcon.senha != senhaImputada)
                mensagem += " SENHA_ERRADA ";

            if (mensagem != "")
                return mensagem;

            return "REGISTRO_REALIZADO";
        }

        public string ValidarContaExistente(Conta conta)
        {
            Validador valida = new();
            string mensagem = "";

            if (conta == null)
                mensagem += " CONTA_INVALIDA ";
            else if (repositorioConta.GetAll().Any(x => x.id == conta.id))
                return "REGISTRO_REALIZADO";
            else
                return " CONTA_NAO_ENCONTRADA ";

            if (mensagem != "")
                return mensagem;

            return "REGISTRO_REALIZADO";
        }

        public string PermitirExclusaoDaMesa(Mesa toDelete)
        {
            if (toDelete == null)
                return " Mesa não encontrada!";
            if (repositorioConta.GetAll().Any(x => x.mesa.id == toDelete.id))
                return " Esta mesa possuí processo em andamento. ";
            else
                return "SUCESSO!";
        }

        public string PermitirExclusaoDoFuncionario(Funcionario toDelete)
        {
            if (toDelete == null)
                return " Funcionário não encontrado!";
            if (repositorioGarcon.GetAll().Any(x => x.id == toDelete.id) || repositorioMesa.GetAll().Any(x =>x.garcon.id == toDelete.id) || repositorioConta.GetAll().Any(x => x.id == toDelete.id))
                return " Este funcionário possuí processo em andamento. ";
            else
                return "SUCESSO!";
        }

        public string PermitirExclusaoDoGarcon(Funcionario toDelete)
        {
            if (toDelete == null)
                return " Garçom não encontrado!";
            if (repositorioMesa.GetAll().Any(x => x.garcon.id == toDelete.id) || repositorioConta.GetAll().Any(x => x.id == toDelete.id))
                return " Este Garçom possuí processo em andamento. ";
            else
                return "SUCESSO!";
        }

        public string PermitirExclusaoDoProduto(Produto toDelete)
        {
            if (toDelete == null)
                return " Produto não encontrado!";
            if (repositorioConta.GetAll().Any(x => x.id == toDelete.id))
                return " Este produto possuí processo em andamento. ";
            else
                return "SUCESSO!";
        }

        public string PermitirRemocaoDoPedido(Pedido toRemove, Conta conta)
        {
            if (toRemove == null)
                return " Pedido não encontrado!";
            else if (conta.listaPedidos.Any(x => x.id == toRemove.id))
                return "SUCESSO!";
            else
                return " Pedido não encontrado!";
        }

        public string PermitirExclusaoConta(Conta toDelete, string senhaImputada)
        {
            Validador valida = new();
            string mensagem = "";

            if (toDelete.mesa.garcon == null)
                mensagem += " GARCOM_INVALIDO ";
            else if (valida.ValidarString(senhaImputada) || toDelete.mesa.garcon.senha != senhaImputada)
                mensagem += " SENHA_ERRADA ";

            if (toDelete == null)
                return " CONTA_INEXISTENTE ";

            if (mensagem != "")
                return mensagem;

            return "REGISTRO_REALIZADO";
        }

    }
}
