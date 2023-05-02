﻿using ControleDeBar.ConsoleApp.ModuloConta;
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

            if (valida.ValidaTelefone(informacoesPessoais.telefone))
                mensagem += " TELEFONE_INVALIDO ";

            if (valida.ValidarString(informacoesPessoais.endereco))
                mensagem += " ENDERECO_INVALIDO ";

            if (valida.ValidaCPF(informacoesPessoais.cpf))
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

            if (valida.ValidarSenha(imput.senha))
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
            if (repositorioGarcon.GetAll().Any(x => x.id == toDelete.id) || repositorioMesa.GetAll().Any(x => .x == toDelete.id))
                return " Este funcionário possuí processo em andamento. ";
            else
                return "SUCESSO!";
        }

        public string PermitirExclusaoDoProduto(Produto toDelete)
        {
            if (toDelete == null)
                return " Produto não encontrado!";
            if (repositorioConta.GetAll().Any(x => x.produto.id == toDelete.id))//|| rep.GetAll().Any(x => x.informacoesReposicao.remedio.id == toDelete.id))
                return " Este produto possuí processo em andamento. ";
            else
                return "SUCESSO!";
        }

        public string PermitirExclusaoConta(Conta toDelete, string senhaImputada)
        {
            Validador valida = new();
            string mensagem = "";

            if (toDelete.informacoesReposicao.funcionario == null)
                mensagem += " FUNCIONARIO_INVALIDO ";
            else if (valida.ValidarString(senhaImputada) || toDelete.informacoesReposicao.funcionario.senha != senhaImputada)
                mensagem += " SENHA_ERRADA ";

            if (toDelete == null)
                return " AQUISICAO_INEXISTENTE ";

            if (mensagem != "")
                return mensagem;

            return "REGISTRO_REALIZADO";
        }

    }
}
