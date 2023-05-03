using ControleDeBar.ConsoleApp.Compartilhado;
using ControleDeBar.ConsoleApp.ModuloFuncionario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleDeBar.ConsoleApp.ModuloGarcon
{
    public class Garcon : Funcionario
    {
        public Garcon()
        {

        }

        public Garcon(Funcionario funcionario)
        {
            this.id = funcionario.id;
            this.senha = funcionario.senha;
            this.informacoesPessoais = funcionario.informacoesPessoais;
        }
    }
}
