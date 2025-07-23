using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EletronicaGenetica
{
    public class Componente
    {
        public string Nome { get; }
        public double Custo { get; }
        public double ConsumoEnergia { get; } // Em Watts (W)
        public double Tamanho { get; }        // Em milímetros quadrados (mm²)

        public Componente(string nome, double custo, double consumoEnergia, double tamanho)
        {
            Nome = nome;
            Custo = custo;
            ConsumoEnergia = consumoEnergia;
            Tamanho = tamanho;
        }
    }
}