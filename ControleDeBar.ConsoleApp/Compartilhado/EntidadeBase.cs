namespace ControleDeBar.ConsoleApp.Compartilhado
{
    public abstract class EntidadeBase
    {
        public int id { get; set; }

        public abstract void UpdateInfo(EntidadeBase valid);
    }
}
