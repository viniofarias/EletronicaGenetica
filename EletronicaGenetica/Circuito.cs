using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EletronicaGenetica
{
    public class Circuito : ICloneable
    {
        public bool[] Genes { get; set; } // O último gene representa a opção "multilayer"
        public double Fitness { get; set; }
        public double CustoTotal { get; private set; }
        public double ConsumoTotal { get; private set; }
        public double TamanhoTotal { get; private set; }
        public bool IsMultilayer { get; private set; }

        private static readonly Random rand = new Random();

        public Circuito(int numComponentesDisponiveis)
        {
            // O tamanho dos genes é o número de componentes + 1 (para o gene multilayer)
            Genes = new bool[numComponentesDisponiveis + 1];
            for (int i = 0; i < Genes.Length; i++)
            {
                // rand.Next(2) == 1 é a forma idiomática de gerar um booleano aleatório em C#
                Genes[i] = rand.Next(2) == 1;
            }
        }

        public Circuito(bool[] genes)
        {
            Genes = genes;
        }

        public void CalcularAtributos(List<Componente> componentesDisponiveis, double custoAdicionalMultilayer, double fatorReducaoTamanho)
        {
            CustoTotal = 0;
            ConsumoTotal = 0;

            double somaBrutaTamanhos = 0; // Variável para a soma antes da redução

            IsMultilayer = Genes[Genes.Length - 1];


            for (int i = 0; i < componentesDisponiveis.Count; i++)
            {
                if (Genes[i])
                {
                    Componente c = componentesDisponiveis[i];
                    CustoTotal += c.Custo;
                    ConsumoTotal += c.ConsumoEnergia;
                    somaBrutaTamanhos += c.Tamanho; // Acumula o tamanho bruto
                }
            }


            // Se a placa for multilayer, o tamanho total efetivo é a soma bruta reduzida.
            // Caso contrário, é apenas a soma bruta.
            if (IsMultilayer)
            {
                CustoTotal += custoAdicionalMultilayer;
                TamanhoTotal = somaBrutaTamanhos * fatorReducaoTamanho;
            }
            else
            {
                TamanhoTotal = somaBrutaTamanhos;
            }


        }

        /// <summary>
        /// Implementação do Design Pattern Prototype (Padrão de Projeto Protótipo).
        /// Em C#, o método Clone() retorna um object, necessitando de um cast.
        /// </summary>
        public object Clone()
        {
            // MemberwiseClone faz uma cópia superficial (shallow copy).
            Circuito clone = (Circuito)this.MemberwiseClone();
            // Precisamos fazer uma cópia profunda (deep copy) do array de genes.
            clone.Genes = (bool[])this.Genes.Clone();
            return clone;
        }

        /// <summary>
        /// Em C#, sobrescrevemos o método ToString() para formatação customizada.
        /// A interpolação de string ($"...") é a forma moderna de formatar strings.
        /// </summary>
        public override string ToString()
        {
            string tipoPlaca = IsMultilayer ? "Multilayer" : "Padrão";
            // Usando formatação de moeda (C2) e números (F2) para melhor visualização.
            return $"Placa: {tipoPlaca} | Custo: {CustoTotal:C2} | Consumo: {ConsumoTotal:F2}W | Tamanho: {TamanhoTotal:F2}mm² | Fitness: {Fitness:F5}";
        }
    }
}
