using ControleDeBar.ConsoleApp.Compartilhado;
using ControleDeBar.ConsoleApp.ModuloGarcon;
using ControleDeBar.ConsoleApp.ModuloMesa;
using ControleDeBar.ConsoleApp.ModuloPedido;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleDeBar.ConsoleApp.ModuloConta
{
    public class Conta : EntidadeBase
    {
        public Mesa mesa { get; set; }
        public decimal valorFinal { get; set; }
        public List<Pedido> listaPedidos { get; set; }

        public Conta()
        {
            
        }

        public Conta(Mesa mesa,decimal valorFinal, List<Pedido> listaPedidos)
        {
            this.mesa = mesa;
            this.valorFinal = valorFinal;
            this.listaPedidos = listaPedidos;
        }

        public override void UpdateInfo(EntidadeBase imput)
        {
            Conta valid = (Conta)imput;

            mesa = valid.mesa;
            valorFinal = valid.valorFinal;
            listaPedidos = valid.listaPedidos;
        }
    }
}
