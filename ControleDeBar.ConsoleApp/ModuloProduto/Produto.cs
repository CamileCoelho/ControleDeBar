using ControleDeBar.ConsoleApp.Compartilhado;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleDeBar.ConsoleApp.ModuloProduto
{
    public class Produto : EntidadeBase
    {
        public string nome {  get; set; }
        public string descricao { get; set; }
        public decimal preco { get; set; }
        public int quantidade { get; set; }
        
        public Produto()
        {
            
        }

        public Produto(string nome, string descricao, decimal preco)
        {
            this.nome = nome;
            this.descricao = descricao;
            this.preco = preco;
        }

        public Produto(string nome, decimal preco, int quantidade)
        {
            this.nome = nome;
            this.preco = preco;
            this.quantidade = quantidade;
        }

        public override void UpdateInfo(EntidadeBase imput)
        {
            Produto toUpdate = (Produto)imput;

            nome = toUpdate.nome;
            descricao = toUpdate.descricao;
            preco = toUpdate.preco;
        }
    }
}
