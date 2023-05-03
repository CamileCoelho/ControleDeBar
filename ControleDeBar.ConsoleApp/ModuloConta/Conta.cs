using ControleDeBar.ConsoleApp.Compartilhado;
using ControleDeBar.ConsoleApp.ModuloMesa;

namespace ControleDeBar.ConsoleApp.ModuloConta
{
    public class Conta : EntidadeBase
    {
        public Mesa mesa { get; set; }
        public List<Pedido> listaPedidos { get; set; }
        public decimal valorFinal { get; set; }
        public DateTime data { get; set; }
        public string status { get; set; }

        public Conta()
        {
            
        }

        public Conta(Mesa mesa)
        {
            this.mesa = mesa;
            mesa.status = "INDISPONIVEL";
            listaPedidos = new();
            data = DateTime.Now.Date;
            valorFinal = 0;
            status = "EM ABERTO";
        }

        public override void UpdateInfo(EntidadeBase imput)
        {
            Conta valid = (Conta)imput;

            mesa = valid.mesa;
        }

        public void ObterValorTotalDia()
        {

        }
    }
}
