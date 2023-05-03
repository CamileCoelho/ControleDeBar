using ControleDeBar.ConsoleApp.Compartilhado;
using ControleDeBar.ConsoleApp.ModuloProduto;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleDeBar.ConsoleApp.ModuloConta
{
    public class RepositorioConta : RepositorioBase<Conta> 
    {
        public Financeiro financeiro;
        int idCounterPedido = 1;

        public RepositorioConta(Financeiro financeiro)
        {
            this.financeiro = financeiro;
        }

        public void AddPedido(Conta toAdd, Pedido pedido)
        {
            pedido.id = idCounterPedido++;
            toAdd.listaPedidos.Add(pedido);
            decimal total = ObterValorPedido(pedido);
            toAdd.valorFinal += total;
        }


        public void RemovePedido(Conta toRemove, Pedido pedido)
        {
            toRemove.listaPedidos.Remove(pedido);
            decimal total = ObterValorPedido(pedido);
            toRemove.valorFinal -= total;
        }

        private decimal ObterValorPedido(Pedido pedido)
        {
            decimal total = 0;

            foreach (Produto produto in pedido.produtos)
            {
                if (produto != null)
                {
                    total += produto.preco * produto.quantidade;
                }
            }

            return total;
        }

        public void End(Conta toEnd)
        {
            toEnd.mesa.status = "DISPONIVEL";
            toEnd.status = "ENCERRADO";
            financeiro.faturamento += toEnd.valorFinal;
        }
    }
}
