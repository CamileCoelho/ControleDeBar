using ControleDeBar.ConsoleApp.ModuloConta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleDeBar.ConsoleApp.Compartilhado
{
    public class Financeiro 
    {
        public decimal faturamento { get; set; }

        public Financeiro()
        {
            faturamento = 0;
        }

        public void ZerarFaturamento()
        {
            faturamento = 0;
        }
    }
}
