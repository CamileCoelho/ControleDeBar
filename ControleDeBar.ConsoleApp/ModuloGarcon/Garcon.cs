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
        public string gorgeta { get; set; }

        public Garcon()
        {

        }

        public Garcon(Funcionario funcionario)
        {
            
        }

        public void ReceberGorgeta(decimal gorgeta)
        {
            this.gorgeta += gorgeta; // 5% do valor total da compra
        }
    }
}
