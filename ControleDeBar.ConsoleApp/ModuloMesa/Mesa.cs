using ControleDeBar.ConsoleApp.Compartilhado;
using ControleDeBar.ConsoleApp.ModuloConta;
using ControleDeBar.ConsoleApp.ModuloGarcon;

namespace ControleDeBar.ConsoleApp.ModuloMesa
{
    public class Mesa : EntidadeBase
    {
        public Garcon garcon {  get; set; }
        public string status { get; set; }

        public Mesa()
        {
            
        }

        public Mesa(Garcon garcon)
        {
            this.garcon = garcon;
            status = "DISPONIVEL";
        }

        public override void UpdateInfo(EntidadeBase imput)
        {
            Mesa toUpdate = (Mesa)imput;

            garcon = toUpdate.garcon;
            status = toUpdate.status;
        }
    }
}
