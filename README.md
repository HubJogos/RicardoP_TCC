# TCC
Repositório para o projeto de TCC sobre análise para o aprimoramento de geradores de mapas procedruais para jogos top-down 2D.

O projeto contém diversos pacotes, usados para implementar as funções visuais do projeto, como as sprites, cenas e objetos (prefabs) de jogo. 
Os scripts estão todos contidos na pasta "Assets/Scripts", onde são separados por categoria.

## Os scripts relevantes para a geração são:
- PlayerScript.cs : onde são gerenciadas todas as mecânicas do jogador, juntamente com todos seus dados
- MapScripts/MapGenAutomata.cs : onde o mapa é de fato gerado, contendo as funções de conexão, aleatoriedade, entre outras. Também é o script que salva a matriz do mapa 
assim que é gerado
- MapScripts/MeshGenerator.cs : gera as meshes do mapa, definindo colisores
- MapScripts/DataGenerator.cs : o único script que salva os dados de geração
- MapScripts/PoissonSampling.cs : um método de colocação de objetos de forma procedural, ainda em implementação
- MapScripts/TestPlacement.cs : script para ajudar na visualização do funcionamento de "PoissonSampling"

## Cenas
Existem seis cenas no diretório, sendo "SampleScene" a que iniciou com o projeto, não possuindo funcionalidades.
- Preload : cena com um objeto que permanece ao longo das outras cenas, futuramente usado para carregar efeitos sonoros, sistema de save/load, entre outros
- MainMenu : cena contendo menu inicial e de opções, incompleto pois o sistema de save/load ainda não foi concluido
- Game : cena básica de implementação de mecânicas
- MapGeneration : única cena onde o mapa é gerado proceduralmente
- PlacementTesting : cena teste para a colocação de objetos
