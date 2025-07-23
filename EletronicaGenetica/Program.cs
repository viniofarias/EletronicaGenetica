namespace EletronicaGenetica
{
    public class Program
    {
        // --- PARÂMETROS DO ALGORITMO GENÉTICO ---
        private const int TamanhoPopulacao = 150;
        private const int NumeroGeracoes = 250;
        private const double TaxaMutacao = 0.04;
        private const int TamanhoTorneio = 3;
        private const bool Elitismo = false;

        // --- REQUISITOS DO PROBLEMA ---
        private const double TamanhoMaximoPlaca = 5000.0;
        private const double CustoAdicionalMultilayer = 120.00;
        private const double FatorOtimizacaoMultilayer = 0.7;
        private const int MINIMO_COMPONENTES = 10;
        private const double CUSTO_MAXIMO_ESPERADO = 600.0;  // Um custo alto, mas plausível
        private const double CONSUMO_MAXIMO_ESPERADO = 10.0;

        // Em C#, listas não podem ser 'const', mas 'static readonly' garante que a instância da lista não será trocada.
        private static readonly List<Componente> ComponentesDisponiveis = new List<Componente>();
        private const double PesoCusto = 1.0;
        private const double PesoEficiencia = 1.5;
        private const double PesoTamanho = 1.2;

        private static readonly Random rand = new Random();

        // O método Main é o ponto de entrada do programa em C#
        public static void Main(string[] args)
        {
            InicializarComponentes();
            List<Circuito> populacao = CriarPopulacaoInicial();
            Circuito melhorSolucaoGeral = null;

            Console.WriteLine("Iniciando processo evolutivo....");

            for (int geracao = 0; geracao < NumeroGeracoes; geracao++)
            {
                CalcularFitnessPopulacao(populacao);

                // Em C#, podemos usar o método Sort da lista com uma lambda expression.
                populacao.Sort((c1, c2) => c2.Fitness.CompareTo(c1.Fitness));

                if (melhorSolucaoGeral == null || populacao[0].Fitness > melhorSolucaoGeral.Fitness)
                {
                    // O método Clone() retorna 'object', então um cast é necessário.
                    melhorSolucaoGeral = (Circuito)populacao[0].Clone();
                }

                // Console.WriteLine é o equivalente a System.out.println
                Console.WriteLine($"Geração {geracao + 1} | Melhor Fitness: {populacao[0].Fitness:F5} | Solução: {populacao[0]}");

                populacao = EvoluirPopulacao(populacao);
            }

            Console.WriteLine("\n--- Processo Evolutivo Concluído! ---");
            Console.WriteLine("Melhor configuração de circuito encontrada:");
            ImprimirDetalhesSolucao(melhorSolucaoGeral);
        }

        private static void InicializarComponentes()
        {
            // O método Add é o equivalente ao 'add' do ArrayList do Java.
            ComponentesDisponiveis.Add(new Componente("Microcontrolador A (Padrão)", 25.50, 0.8, 150));
            ComponentesDisponiveis.Add(new Componente("Microcontrolador A (SMD)", 29.30, 0.64, 105));
            ComponentesDisponiveis.Add(new Componente("Microcontrolador B (Padrão)", 35.00, 0.5, 200));
            ComponentesDisponiveis.Add(new Componente("Microcontrolador B (SMD)", 40.25, 0.4, 140));
            ComponentesDisponiveis.Add(new Componente("Sensor de Proximidade (Padrão)", 12.00, 0.1, 50));
            ComponentesDisponiveis.Add(new Componente("Sensor de Proximidade (SMD)", 13.80, 0.08, 35));
            ComponentesDisponiveis.Add(new Componente("Módulo Wi-Fi (Padrão)", 45.00, 1.2, 300));
            ComponentesDisponiveis.Add(new Componente("Módulo Wi-Fi (SMD)", 51.75, 0.96, 210));
            ComponentesDisponiveis.Add(new Componente("Módulo Bluetooth 5.0 (Padrão)", 30.00, 0.4, 250));
            ComponentesDisponiveis.Add(new Componente("Módulo Bluetooth 5.0 (SMD)", 34.50, 0.32, 175));
            ComponentesDisponiveis.Add(new Componente("GPS (Padrão)", 55.00, 1.5, 400));
            ComponentesDisponiveis.Add(new Componente("GPS (SMD)", 63.25, 1.2, 280));
            ComponentesDisponiveis.Add(new Componente("Chip de Áudio (Padrão)", 22.00, 0.3, 120));
            ComponentesDisponiveis.Add(new Componente("Chip de Áudio (SMD)", 25.30, 0.24, 84));
            ComponentesDisponiveis.Add(new Componente("Regulador de Tensão (Padrão)", 5.00, 0.02, 30));
            ComponentesDisponiveis.Add(new Componente("Regulador de Tensão (SMD)", 5.75, 0.016, 21));
            ComponentesDisponiveis.Add(new Componente("Sensor de Luz", 8.50, 0.05, 40));
            ComponentesDisponiveis.Add(new Componente("Acelerômetro", 15.00, 0.2, 60));
            ComponentesDisponiveis.Add(new Componente("Giroscópio", 18.00, 0.25, 70));
            ComponentesDisponiveis.Add(new Componente("Bateria Lítio 2000mAh", 60.00, 2.0, 1000));
            ComponentesDisponiveis.Add(new Componente("Bateria Lítio 3000mAh", 85.00, 3.5, 1500));
            ComponentesDisponiveis.Add(new Componente("Memória Flash 16GB", 40.00, 0.6, 180));
        }

        private static List<Circuito> CriarPopulacaoInicial()
        {
            var populacao = new List<Circuito>();
            for (int i = 0; i < TamanhoPopulacao; i++)
            {
                populacao.Add(new Circuito(ComponentesDisponiveis.Count));
            }
            return populacao;
        }

        private static void CalcularFitnessPopulacao(List<Circuito> populacao)
        {
            foreach (var circuito in populacao)
            {
                circuito.CalcularAtributos(ComponentesDisponiveis, CustoAdicionalMultilayer, FatorOtimizacaoMultilayer);

                double fitness;
                int numeroDeComponentes = 0;
                bool temBateria = false;
                bool temMicrocontrolador = false;

                // 2. Itera sobre os genes para verificar os componentes incluídos e as regras de negócio
                for (int i = 0; i < ComponentesDisponiveis.Count; i++)
                {
                    if (circuito.Genes[i]) // Se o gene está ativo (componente incluído)
                    {
                        numeroDeComponentes++;
                        string nomeComponente = ComponentesDisponiveis[i].Nome;

                        if (nomeComponente.Contains("Bateria"))
                        {
                            temBateria = true;
                        }
                        if (nomeComponente.Contains("Microcontrolador"))
                        {
                            temMicrocontrolador = true;
                        }
                    }
                }

                if (circuito.TamanhoTotal > TamanhoMaximoPlaca ||       // Regra 1: Excede o tamanho?
                    numeroDeComponentes < MINIMO_COMPONENTES ||      // Regra 2: Tem componentes suficientes?
                    !temBateria ||                                  // Regra 3: Tem uma fonte de energia?
                    !temMicrocontrolador)
                {
                    fitness = 0.0;
                }
                else
                {
                    double scoreCusto = Math.Max(0, 1 - (circuito.CustoTotal / CUSTO_MAXIMO_ESPERADO));
                    double scoreEficiencia = Math.Max(0, 1 - (circuito.ConsumoTotal / CONSUMO_MAXIMO_ESPERADO));
                    double scoreTamanho = Math.Max(0, 1 - (circuito.TamanhoTotal / TamanhoMaximoPlaca));

                    // 4. Fitness final é a soma ponderada dos três critérios
                    fitness = (PesoCusto * scoreCusto) +
                              (PesoEficiencia * scoreEficiencia) +
                              (PesoTamanho * scoreTamanho);
                }
                circuito.Fitness = fitness;
            }
        }

        private static List<Circuito> EvoluirPopulacao(List<Circuito> populacao)
        {
            var novaPopulacao = new List<Circuito>();

            if (Elitismo)
            {
                novaPopulacao.Add((Circuito)populacao[0].Clone());
            }

            while (novaPopulacao.Count < TamanhoPopulacao)
            {
                Circuito pai1 = SelecaoPorTorneio(populacao);
                Circuito pai2 = SelecaoPorTorneio(populacao);
                Circuito filho = Cruzamento(pai1, pai2);
                Mutacao(filho);
                novaPopulacao.Add(filho);
            }
            return novaPopulacao;
        }

        private static Circuito SelecaoPorTorneio(List<Circuito> populacao)
        {
            var torneio = new List<Circuito>();
            for (int i = 0; i < TamanhoTorneio; i++)
            {
                int indiceAleatorio = rand.Next(populacao.Count);
                torneio.Add(populacao[indiceAleatorio]);
            }
            // Usando LINQ para encontrar o melhor do torneio.
            return torneio.OrderByDescending(c => c.Fitness).First();
        }

        private static Circuito Cruzamento(Circuito pai1, Circuito pai2)
        {
            bool[] genesFilho = new bool[pai1.Genes.Length];
            for (int i = 0; i < pai1.Genes.Length; i++)
            {
                // Para cada gene, sorteia aleatoriamente de qual pai ele virá
                genesFilho[i] = rand.NextDouble() < 0.5 ? pai1.Genes[i] : pai2.Genes[i];
            }
            return new Circuito(genesFilho);
        }

        private static void Mutacao(Circuito circuito)
        {
            for (int i = 0; i < circuito.Genes.Length; i++)
            {
                if (rand.NextDouble() < TaxaMutacao)
                {
                    circuito.Genes[i] = !circuito.Genes[i];
                }
            }
        }

        private static void ImprimirDetalhesSolucao(Circuito circuito)
        {
            circuito.CalcularAtributos(ComponentesDisponiveis, CustoAdicionalMultilayer, FatorOtimizacaoMultilayer);
            Console.WriteLine($"-> {circuito}");

            // A mensagem de impressão agora reflete corretamente o limite fixo.
            Console.WriteLine($"   (Limite Fixo da Placa: {TamanhoMaximoPlaca:F2}mm²)");

            Console.WriteLine("Componentes incluídos no design:");
            for (int i = 0; i < ComponentesDisponiveis.Count; i++)
            {
                if (circuito.Genes[i])
                {
                    Componente c = ComponentesDisponiveis[i];
                    Console.WriteLine($"  - {c.Nome} (Custo: {c.Custo:C2}, Consumo: {c.ConsumoEnergia:F2}W, Tamanho: {c.Tamanho:F2}mm²)");
                }
            }
        }
    }
}