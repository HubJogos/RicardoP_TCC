# TCC
Repositório para o projeto de TCC sobre análise para o aprimoramento de geradores de mapas procedruais para jogos top-down 2D.

O projeto contém diversos pacotes, usados para implementar as funções visuais do projeto, como as sprites, cenas e objetos (prefabs) de jogo. 
Os scripts estão todos contidos na pasta "Assets/Scripts", onde são separados por categoria.

## Os scripts relevantes para a geração são:
- DamagePlayer.cs : onde é gerenciada a matriz de mortes do jogdor
- MapScripts/MapGenAutomata.cs : onde o mapa é de fato gerado, contendo as funções de conexão, aleatoriedade, entre outras. Também é o script que salva a matriz do mapa 
assim que é gerado
- MapScripts/MeshGenerator.cs : gera as meshes do mapa, definindo colisores
- MapScripts/DataGenerator.cs : o que irá gerar outros dados sobre o jogo, como tempo gasto, objetos coletados, inimigos derrotados, dano recebido, etc.
- MapScripts/PoissonSampling.cs : um método de colocação de objetos de forma procedural, ainda em implementação
- MapScripts/TestPlacement.cs : script para ajudar na visualização do funcionamento de "PoissonSampling"

## Cenas
Existem seis cenas no diretório, sendo "SampleScene" a que iniciou com o projeto, não possuindo funcionalidades.
1- Preload : cena com um objeto que permanece ao longo das outras cenas, futuramente usado para carregar efeitos sonoros, sistema de save/load, entre outros
2- MainMenu : cena contendo menu inicial e de opções, incompleto pois o sistema de save/load ainda não foi concluido
3- Game : cena básica de implementação de mecânicas
4- MapGeneration : única cena onde o mapa é gerado proceduralmente
5- PlacementTesting : cena teste para a colocação de objetos
