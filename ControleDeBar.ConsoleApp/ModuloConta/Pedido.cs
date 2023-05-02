using ControleDeBar.ConsoleApp.ModuloProduto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleDeBar.ConsoleApp.ModuloConta
{
    public class Pedido
    {
        public Produto produto { get; }
        public int quantidadeProduto { get; }

        public Pedido()
        {
            
        }

        public Pedido(Produto produto, int quantidadeProduto)
        {
            this.produto = produto;
            this.quantidadeProduto = quantidadeProduto;
        }
    }
}
