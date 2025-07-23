🧬 Otimizador de Circuitos com Algoritmo Genético (EletronicaGenetica)
Este projeto utiliza um Algoritmo Genético (AG) implementado em C# .NET para resolver um problema complexo de otimização de múltiplos objetivos: o design de um circuito eletrônico para um novo dispositivo portátil.

O objetivo é encontrar a configuração de componentes ideal que minimize simultaneamente o Custo, o Consumo de Energia e o Tamanho da placa, ao mesmo tempo que cumpre um conjunto de restrições funcionais para garantir que o circuito seja viável na prática.

🎯 O Problema
A empresa fictícia "Tecnologia Inovadora" precisa projetar o circuito para um novo dispositivo. A equipe de engenharia enfrenta o desafio de selecionar os componentes corretos dentre uma vasta lista de opções, incluindo variantes Padrão e SMD (Surface-Mount Device), e decidir se a placa deve ser de camada única ou Multilayer.

O design perfeito deve equilibrar três fatores conflitantes:

Custo (R$): Minimizar o custo total de fabricação.

Eficiência Energética (W): Minimizar o consumo total de energia para maximizar a vida da bateria.

Tamanho (mm²): Garantir que todos os componentes caibam no espaço físico limitado da placa.

Além disso, um circuito funcional deve obrigatoriamente possuir um número mínimo de componentes, uma fonte de energia (bateria) e uma unidade de processamento (microcontrolador).

🧠 Como Funciona: A Abordagem Genética
O algoritmo genético simula o processo de seleção natural de Darwin para "evoluir" uma população de soluções (circuitos) ao longo de várias gerações, até encontrar uma solução ótima.

1. O Cromossomo (Circuito)
Cada solução candidata é um "cromossomo" representado pela classe Circuito. Seu "DNA" é um array de booleanos (bool[] Genes), onde cada gene representa uma decisão:

Genes de Componentes: Cada um dos primeiros genes indica a presença (true) ou ausência (false) de um componente da lista de disponíveis.

Gene Multilayer: O último gene do array funciona como um interruptor global, definindo se a placa é Multilayer (true) ou não (false).

2. A Função de Aptidão (Fitness)
A "mente" do algoritmo. Ela avalia cada circuito e lhe atribui uma pontuação de "aptidão". Um circuito só é avaliado se for funcionalmente válido.

Regras de Validade (Pena de Morte)
Um circuito recebe fitness 0 (e é descartado) se qualquer uma das seguintes regras for violada:

Ocupar um tamanho maior que o permitido na placa.

Possuir menos de 10 componentes.

Não possuir nenhum componente do tipo Bateria.

Não possuir nenhum componente do tipo Microcontrolador.

O Cálculo do Fitness
Para os circuitos válidos, o fitness é uma Soma Ponderada de Scores Normalizados:

Normalização: Para cada critério (custo, consumo, tamanho), um score de 0 a 1 é calculado pela fórmula Score = 1 - (ValorAtual / ValorMaximoEsperado). Isso garante que um valor baixo (bom) resulte em um score alto (próximo de 1).

Soma Ponderada: O fitness final é a soma desses scores, multiplicados por seus respectivos pesos (PesoCusto, PesoEficiencia, PesoTamanho). Isso permite ajustar a importância de cada objetivo.

3. Os Operadores Evolutivos
Seleção (Seleção por Torneio): Um pequeno grupo aleatório de circuitos "compete", e o de maior fitness é selecionado para ser um "pai". Este método equilibra bem a seleção dos melhores com a manutenção da diversidade genética.

Cruzamento (Cruzamento Uniforme): Para cada gene do filho, uma "moeda é jogada" para decidir se ele herdará o gene do pai 1 ou do pai 2. Isso cria uma mistura profunda e eficaz das características dos pais.

Mutação (Bit Flip Mutation): Cada gene de um novo filho tem uma pequena chance (definida pela TaxaMutacao) de ser invertido aleatoriamente. Isso introduz novas características na população, sendo vital para evitar a estagnação do algoritmo.

🚀 Como Executar
Este é um projeto de console em .NET.

Clone o repositório:

Bash

git clone [URL_DO_SEU_REPOSITORIO]
Navegue até o diretório do projeto.

Execute o projeto usando o CLI do .NET:

Bash

dotnet run
O algoritmo iniciará o processo evolutivo e imprimirá o status da melhor solução a cada geração. Ao final, a configuração completa do melhor circuito encontrado será exibida.

🛠️ Configuração e Parâmetros
Você pode facilmente ajustar o comportamento do algoritmo alterando as constantes no topo do arquivo Program.cs.

TamanhoPopulacao: Número de soluções em cada geração. (Mais = maior diversidade, mais lento).

NumeroGeracoes: Quantas gerações o algoritmo irá evoluir.

TaxaMutacao: A probabilidade de um gene sofrer mutação. (Mais = mais exploração, menos estabilidade).

TamanhoTorneio: Número de indivíduos que competem na seleção. (Mais = maior pressão seletiva, convergência mais rápida).

PesoCusto, PesoEficiencia, PesoTamanho: Permitem ajustar a prioridade de cada objetivo de otimização.

💻 Tecnologias Utilizadas
.C#

.NET