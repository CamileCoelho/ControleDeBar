using ControleDeBar.ConsoleApp.ModuloProduto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleDeBar.ConsoleApp.Compartilhado
{
    public class Pedido : EntidadeBase
    {
        public List<Produto> produtos { get; set; }

        public Pedido()
        {

        }

        public Pedido(List<Produto> produtos)
        {
            this.produtos = produtos;
        }

        public override void UpdateInfo(EntidadeBase valid)
        {
            throw new NotImplementedException();
        }
    }
}
