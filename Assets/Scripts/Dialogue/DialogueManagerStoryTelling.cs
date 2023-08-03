using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


public class DialogueManagerStoryTelling : MonoBehaviour
{
    // Start is called before the first frame update


    public string NomeNPC;
    public bool npcNormal = true;

    public bool firstTime = true;
    public bool backToThePortal = false;
    public bool dialogueStarted = false;
    public GameObject portal;


    PersistentStats stats;
    DataGenerator dataGen;

    public GameObject HudDialogue;
    [SerializeField]
    private TextMeshProUGUI textDialogue;

    [SerializeField]
    private TextMeshProUGUI textChoice1;
    [SerializeField]
    private TextMeshProUGUI textChoice2;
    [SerializeField]
    private TextMeshProUGUI textChoice3;

    [SerializeField]
    private GameObject buttonChoice1;
    [SerializeField]
    private GameObject buttonChoice2;
    [SerializeField]
    private GameObject buttonChoice3;
    [SerializeField]
    private GameObject buttonChoicesContinue;

    public AudioManager soundStoryTelling;

    private int ato = 1;

    [SerializeField]
    public string fraseNPC = "";

    [SerializeField]
    public Dictionary<string, int> Dialogo;
    public int dialogoAux = 0;


    Dictionary<string, List<string>> dialogoHomemMisteriosoAto1 = new Dictionary<string, List<string>>
    {
        { "Você me lembra alguém que entrou nesse mesmo portal a uns anos atrás…", new List<string> { "Era o meu pai.", "E o que aconteceu com este homem?", "..." } },
        { "Ele nunca voltou de lá. Por que você acha que está preparado para entrar lá também?", new List<string> { "Treinei durante anos para este dia. Minha espada será o suficiente", "Sei me cuidar muito bem em uma caverna", "Cale-se e deixe eu passar logo." } },
        { "Você não viria de tão longe apenas para isso… Você com certeza já ouviu falar das riquezas que tem lá dentro... E você pode não ter tempo suficiente para encontrar o que está procurando e ainda sair com riquezas… MUAHAHAHAHAHA ", new List<string> { "Vim para encontrar o meu pai, é só isso que importa", "Também estou precisando de dinheiro.", "Está vendo essa espada? Ela pode conquistar o que eu quiser." } },
        { "Você lembra mesmo muito a pessoa que entrou antes aqui neste portal, talvez seja por esse motivo que ele nunca voltou, ou falta de tempo. Muahahaha Entre, mas lembre-se, todas as suas ações terão consequências… muahahahahaha", new List<string>()},
        { "O o portal está liberado, não perca tempo. Encontre o que você procura... ou não... MUAHAHAHAH ", new List<string>()},
    };

    Dictionary<string, List<string>> dialogoHomemMisteriosoAto1Cave = new Dictionary<string, List<string>>
    {
        { "Olá querido aventureiro... Tenho algumas coisas para conversar com você.", new List<string> () },
        { "Primeiro de tudo, muitas respostas suas serão respondidas em breve... ", new List<string>() },
        { "Vamos falar como foi lá dentro... antes de tudo, você achou dificil? Muahahahah", new List<string> { "Achei fácil, pensava que seria mais difícil", "Foi mais ou menos, tive algumas dificuldades.", "Achei difícil, saí lá de dentro por pouco." } },
        { "E você achou divertido?", new List<string>{"Bastante divertido, a caverna realmente é interessante.", "Não muito.", "Não achei divertido, foi chato." } },
        { "Você ter atacado os inimigos e levado seus tesouros pode ter gerado algumas consequências... ", new List<string>()},
        { "As riquezas e tesouros dentro das cavernas são tentadores, mas o verdadeiro perigo está no poder que elas podem conceder. Um poder que pode levar à dominação ", new List<string>()},
        { "Essas cavernas mágicas são apenas a ponta do iceberg. Seu potencial para o caos é inigualável, e um dia, elas se espalharão como uma praga ", new List<string>()},
        { "Você se mostrou habilidoso lá dentro das cavernas e passou na sua provação... ", new List<string>()},
        { "Junte-se a mim! Não faça como seu pai fez! MUAHAHAHAHAH ", new List<string>{"Como assim? O que meu pai tem a ver com isso?", "Nunca! Jamais me unirei a você Explique-se!", "Meu pai? Dominação do mundo mágico? Quê?" } },
        { "As sombras que habitam nas profundezas das cavernas mágicas são apenas o começo. Logo, elas se espalharão e dominarão tudo. ", new List<string>{"Você é louco!", "Esse mundo mágico não vai dominar nada!", "O que você fez com meu pai?" } },
        { "Sofra as consequências então! Não ouse entrar nas cavernas nunca mais ou será exterminado assim como o seu pai foi.", new List<string>() },
    };

    Dictionary<string, List<string>> dialogoHomemMisteriosoAto2 = new Dictionary<string, List<string>>
    {
        { "Sua ambição fez com que causasse uma guerra do mundo mágico com o nosso, parabéns. MUAHAHAHAHAH", new List<string> { "Você armou para mim, era tudo que você queria.", "Eu não tive culpa de nada, a culpa é sua!", "?????? (espanto irônico)" } },
        { "O mundo que você conhece não existirá mais em breve, o mundo mágico irá dominar todo ele", new List<string>() },
        { "Desde que você entrou na caverna, passaram 7 anos de guerra entre o seu mundo e o mundo mágico. Claramente o seu mundo está perdendo...", new List<string>() },
        { "Não, você não pode entrar, não permitirei.", new List<string>()},
        { "Se você entrar irá enfrentar consequências! Não permitirei! Não ouse! ", new List<string>()},
        { "Nos encontraremos novamente mais tarde. Muahahahahaah ", new List<string>()},
    };

    Dictionary<string, List<string>> dialogoHomemMisteriosoAto2Cave = new Dictionary<string, List<string>>
    {
        { "Você será exterminado.", new List<string> () },
    };

    private string[] dialogosAleatoriosBasicos =
    {
        "Não tenho tempo para conversar. Tenho muito trabalho a fazer.",
        "As cavernas mágicas são um lugar amaldiçoado. Você não deveria ir lá.",
        "O homem encapuzado é um perigo para todos nós. Ele tem poderes sombrios.",
        "Você é novo por aqui? Não se meta em encrenca.",
        "Você precisa de alguma coisa? Se não, me deixe em paz.",
        "As cavernas mágicas guardam segredos antigos. Mas também armadilhas mortais.",
        "O homem encapuzado é um mistério. Ninguém sabe quem ele é ou o que ele quer.",
        "Você parece perdido. Você sabe onde está se metendo?",
        "Você não é bem-vindo aqui. Este é um vilarejo de gente honesta e trabalhadora.",
        "As cavernas mágicas atraem muitos aventureiros. Mas poucos voltam de lá.",
        "O homem encapuzado é o líder de uma seita maligna. Eles adoram as forças das trevas.",
        "Você tem alguma coisa a ver com o homem encapuzado? Se tiver, é melhor sair daqui.",
        "As cavernas mágicas são um tesouro para os curiosos. Mas também um pesadelo para os imprudentes.",
        "O homem encapuzado está sempre rondando as cavernas mágicas. Ele procura algo lá dentro.",
        "Você veio explorar as cavernas mágicas? Você é corajoso ou louco?",
        "Tenho pouco a dizer para estrangeiros. Não confio neles.",
        "As cavernas mágicas escondem segredos antigos. Respeite sua magia e mantenha-se afastado.",
        "O homem encapuzado é um sinal de problemas iminentes. Fique alerta.",
        "Vá embora. Não precisamos de forasteiros em nosso vilarejo.",
        "As cavernas mágicas já cobraram um alto preço. Muitos foram perdidos lá dentro.",
        "Não fale comigo. Estranhos sempre causam problemas.",
        "Cuidado com as cavernas mágicas. Elas engolem os tolos que se aventuram lá.",
        "Não há lugar para estranhos em nossa comunidade. Mantenha-se afastado.",
        "As cavernas mágicas são perigosas demais. Eu não arriscaria entrar nelas.",
        "O homem encapuzado traz consigo a escuridão. Fique longe dele.",
        "Vá embora. Você não é bem-vindo aqui.",
        "As cavernas mágicas são o lar de criaturas malignas. Fique longe delas.",
        "Estrangeiros não têm lugar em nosso vilarejo. Vá embora.",
        "As cavernas mágicas são um labirinto mortal. Melhor não se perder lá dentro.",
        "Cuidado com o homem encapuzado. Ele não hesitará em causar danos.",
        "Não se aproxime das cavernas mágicas. Elas têm um poder sombrio.",
        "Você não parece daqui. Mantenha-se afastado dos problemas.",
        "As cavernas mágicas são uma porta para o desconhecido. E poucos sobrevivem para contar a história.",
        "O homem encapuzado é um pesadelo que assombra nossos sonhos.",
        "Vilarejos como este não são para pessoas como você. Vá embora.",
        "As cavernas mágicas estão cheias de armadilhas traiçoeiras. Não seja imprudente.",
        "O homem encapuzado é um espectro que assombra nosso vilarejo.",
        "Estrangeiros não são bem-vindos em nossa terra. Parta agora.",
        "As cavernas mágicas são um enigma que poucos conseguem decifrar.",
        "O homem encapuzado traz consigo a escuridão. Fique longe dele, se puder.",
        "Vá embora. Você não é bem-vindo aqui.",
        "As cavernas mágicas são traiçoeiras. Apenas os tolos ousam entrar.",
        "O homem encapuzado traz consigo a escuridão. Fique longe dele se quiser sobreviver.",
        "Você não é daqui. Deveria partir antes que algo de ruim aconteça.",
        "As cavernas mágicas são um desafio que poucos conseguem superar.",
        "O homem encapuzado é uma ameaça que não podemos ignorar.",
        "Não confio em estrangeiros. Eles sempre trazem problemas.",
        "Cuidado com as cavernas mágicas. Elas escondem perigos mortais.",
        "O homem encapuzado é o executor das trevas. Ele traz consigo a morte.",
        "Você não é bem-vindo em nosso vilarejo. Saia antes que eu mude de ideia.",
        "As cavernas mágicas são um labirinto sem saída. Não se perca e nem arrisque sua vida à toa.",
        "O homem encapuzado é um mensageiro da desgraça. Não se aproxime dele, a menos que queira seu fim.",
        "Não me incomode com suas palavras. Não tenho tempo para lidar com estrangeiros.",
        "As cavernas mágicas são um lugar onde a luz não chega. É melhor você se manter longe.",
        "O homem encapuzado é um símbolo da destruição que se aproxima.",
        "Estrangeiros não são bem-vindos em nosso vilarejo. Não insista.",
        "As cavernas mágicas são traiçoeiras. Não se aventure sem uma boa razão.",
        "O homem encapuzado é uma sombra sinistra que devemos temer. Mantenha distância dele.",
        "Você não é daqui. Deveria partir antes que cause problemas.",
        "As cavernas mágicas são um enigma que poucos conseguem decifrar. Pense bem antes de explorá-las.",
        "O homem encapuzado traz consigo a escuridão. Fique longe se quiser preservar sua sanidade.",
        "Não temos tempo para estranhos. Desapareça antes que eu me arrependa de não ter chamado os guardas.",
        "As cavernas mágicas são um labirinto sem saída. Não se perca e nem arrisque sua vida à toa.",
        "O homem encapuzado é um mensageiro da desgraça. Não se aproxime dele, a menos que queira seu fim.",
        "Não me incomode com suas palavras vazias. Não tenho paciência para lidar com estrangeiros.",
        "As cavernas mágicas são um lugar onde a luz não chega. É melhor você se manter longe.",
        "O homem encapuzado é um símbolo da destruição que se aproxima. Fuja enquanto pode.",
        "Estrangeiros não são bem-vindos em nosso vilarejo. Não insista, ou enfrentará as consequências.",
        "As cavernas mágicas são traiçoeiras. Não se aventure sem um bom motivo.",
        "O homem encapuzado é uma abominação que não deve ser enfrentada. Fique longe dele.",
        "Não confie em estranhos. Eles sempre trazem problemas e perturbações.",
        "Cuidado com as cavernas mágicas. Elas são traiçoeiras e podem te devorar sem piedade.",
        "O homem encapuzado é um espectro que assombra nossos pensamentos mais sombrios.",
        "Vilarejos como este não são para pessoas como você. Vá embora antes que seja tarde demais.",
        "As cavernas mágicas são um portal para o desconhecido. E poucos retornam de lá vivos.",
        "O homem encapuzado é um presságio de problemas iminentes. Mantenha-se alerta.",
        "Não há lugar para estranhos em nosso vilarejo. Parta agora ou sofra as consequências.",
        "As cavernas mágicas já cobraram um alto preço em vidas. Não seja o próximo a pagar.",
        "Não fale comigo. Estrangeiros sempre causam problemas por aqui.",
        "Cuidado com as cavernas mágicas. Elas têm um poder sombrio que atrai os incautos.",
        "Não há espaço para estranhos em nossa comunidade. Mantenha-se afastado.",
        "As cavernas mágicas são perigosas demais. Eu não arriscaria entrar nelas, se fosse você.",
        "O homem encapuzado traz consigo a escuridão. Fique longe dele, se quiser preservar sua vida.",
        "Vá embora. Você não é bem-vindo aqui, e não temos tempo para perder com forasteiros."
    };

    public string[] dialogosNpcsJogadorMorreu =
    {
        "Hahaha! Parece que alguém se perdeu nas cavernas! Precisando de uma ajudinha?",
        "Olha só quem voltou... Todo ensanguentado e derrotado! Não disse para tomar cuidado?",
        "Morreu nas profundezas escuras, hein? Aposto que nem encontrou o tesouro!",
        "Bem-vindo de volta, fracassado! Parece que as cavernas não são tão amigáveis assim, né?",
        "Não se preocupe, o herói da história chegou! Só que não...",
        "Mais um que se aventurou e falhou miseravelmente! Quando você aprenderá a lição?",
        "Risos ecoam nas profundezas... Será que você será resgatado dessa vez?",
        "Hahaha! Aquele que se dizia valente agora está preso nas garras das cavernas!",
        "Aqui estamos novamente... Você sempre encontrando problemas onde quer que vá!",
        "Eu diria para tomar cuidado, mas você claramente não sabe o que isso significa!",
        "Outro dia, outra derrota! Você realmente não aprende, não é mesmo?",
        "É tão divertido vê-lo tropeçando e caindo... Uma e outra vez!",
        "Parece que as cavernas estão se alimentando bem desses aventureiros fracassados!",
        "Hahaha! Você é um verdadeiro comediante... mas nem sabe disso!",
        "Não se preocupe, herói de araque! Alguém irá salvá-lo... talvez!",
        "Você morreu? Ah, que pena! Mas alguém precisa manter as cavernas ocupadas com suas falhas!",
        "Mais uma vez, você nos brinda com sua falta de habilidade. É quase hilário!",
        "Dizem que as cavernas estão cheias de perigos mortais... e você só confirma isso!",
        "Vamos lá, dê o seu melhor! Ou talvez nem seja necessário... afinal, você vai falhar!",
        "Risos ecoam enquanto você tropeça nas sombras. Não perca a esperança... ou talvez perca!"
    };

    private string[] dialogosVilaEmGuerra =
    {
        "As criaturas mágicas estão se tornando cada vez mais fortes. Precisamos de ajuda urgente!",
        "Já faz anos desde que a vila foi atacada pela primeira vez. Não sei como ainda resistimos.",
        "As criaturas mágicas invadem nossa vila todas as noites. A guerra parece não ter fim.",
        "Nossa vila foi devastada pelo último ataque das criaturas mágicas. Muitos perderam suas casas.",
        "As crianças não podem mais brincar livremente nas ruas. A guerra afetou a todos nós.",
        "Estamos lutando bravamente, mas precisamos de reforços. Não podemos vencer sozinhos.",
        "Cada noite é uma batalha pela sobrevivência. Mal temos tempo para descansar.",
        "Os portais estão se abrindo com mais frequência. É como se as criaturas estivessem nos caçando.",
        "Muitos de nossos melhores guerreiros já foram mortos nas batalhas contra as criaturas.",
        "Precisamos encontrar uma maneira de fechar os portais para que a guerra finalmente acabe.",
        "Toda vez que pensamos que estamos vencendo, mais criaturas aparecem dos portais.",
        "Nossos estoques de comida estão se esgotando. A guerra nos trouxe fome e desespero.",
        "Algumas noites, o rugido das criaturas mágicas é ensurdecedor. É aterrorizante.",
        "Muitas famílias foram separadas pela guerra. Alguns ainda estão desaparecidos.",
        "Já perdi amigos e entes queridos nas batalhas contra as criaturas mágicas.",
        "Os portais são uma fonte constante de medo e ansiedade para todos nós.",
        "Algumas das criaturas mágicas são enormes e muito difíceis de combater.",
        "Nossas defesas estão enfraquecendo. Precisamos de reforços antes que seja tarde demais.",
        "O conselho da vila está buscando uma solução para acabar com a guerra. Mas não é fácil.",
        "As criaturas mágicas têm uma força sobrenatural. É como se estivessem protegidas por magia.",
        "Às vezes, me pergunto se algum dia veremos o fim dessa guerra.",
        "Muitos jovens se alistaram para lutar contra as criaturas mágicas. É uma batalha desigual.",
        "A guerra mudou a todos nós. Não somos mais o mesmo vilarejo pacífico de antes.",
        "Os portais são como feridas abertas, constantemente derramando criaturas em nossa vila.",
        "Nossas casas estão em ruínas. Precisamos de ajuda para reconstruir o que foi destruído.",
        "A coragem dos guerreiros que lutam contra as criaturas mágicas é inspiradora.",
        "A vila costumava ser um lugar alegre, mas agora somos assombrados pela guerra.",
        "As criaturas mágicas vêm em diferentes formas e tamanhos. É um desafio lidar com todas elas.",
        "Os portais parecem se multiplicar. Como podemos lutar contra algo que não para de surgir?",
        "Nossa vila está cercada pelas criaturas mágicas. É como se estivéssemos encurralados.",
        "A guerra nos tornou mais fortes e resilientes. Mas também nos deixou cicatrizes profundas.",
        "Estamos todos exaustos, mas não podemos parar de lutar.",
        "As criaturas mágicas são implacáveis. Elas não têm piedade de ninguém.",
        "Os portais são como feridas na alma da vila, sempre sangrando novos horrores.",
        "Já perdi a conta de quantas vezes fomos atacados pelas criaturas mágicas.",
        "Nossos corações estão pesados ​​pela perda e pelo sofrimento causado por esta guerra.",
        "A vila está ficando cada vez mais vazia. Muitos decidiram partir em busca de um lugar mais seguro.",
        "A esperança de um dia vivermos em paz novamente é o que nos mantém lutando.",
        "As criaturas mágicas parecem ter um líder poderoso que comanda os ataques.",
        "Os portais são como portões para o desconhecido, e o desconhecido é assustador.",
        "A guerra deixou nossos espíritos abalados. Precisamos encontrar uma maneira de nos recuperar.",
        "Os portais são a fonte de nossos problemas. Se pudermos fechá-los, talvez haja esperança.",
        "Muitos de nós perderam entes queridos nas mãos das criaturas mágicas.",
        "As noites são as piores. É quando as criaturas mágicas saem dos portais com mais força.",
        "Nossos recursos estão se esgotando. A guerra nos coloca à beira da sobrevivência.",
        "A vila costumava ser um refúgio, mas agora é um campo de batalha.",
        "As criaturas mágicas têm olhos brilhantes e assustadores. Eles nos observam de fora dos portais.",
        "Nossos corações estão cheios de tristeza por tudo o que perdemos nesta guerra.",
        "Os portais são como feridas que nunca cicatrizam. Eles continuam nos machucando.",
        "A vila não é mais segura. Qualquer sombra pode ser uma criatura mágica à espreita.",
        "As criaturas mágicas têm poderes que nunca vimos antes. É como se fossem invencíveis.",
        "Os portais são como imãs, atraindo as criaturas mágicas para nossa vila.",
        "A esperança é o que nos mantém lutando, mesmo quando tudo parece perdido.",
        "Nossas muralhas não são páreo para as criaturas mágicas. Elas as derrubam com facilidade.",
        "A guerra mudou muitas coisas, mas não nossa determinação em proteger nossa vila.",
        "Os portais são como cicatrizes que lembram constantemente dos horrores que enfrentamos.",
        "A vila está encharcada de sangue derramado nas batalhas contra as criaturas mágicas.",
        "As criaturas mágicas são impiedosas. Elas não têm compaixão por ninguém.",
        "Os portais são portões para o desconhecido, mas o desconhecido é aterrorizante.",
        "A guerra nos fez perder muito, mas também mostrou nossa força e resistência.",
        "As criaturas mágicas têm uma vantagem na escuridão. É quando elas atacam com mais ferocidade.",
        "Nossos guerreiros lutam com coragem, mas precisamos de reforços para vencer esta guerra.",
        "A vila costumava ser um lugar acolhedor. Agora, é um campo de batalha constante.",
        "Os portais são como feridas abertas, permitindo que as criaturas mágicas nos ataquem.",
        "A guerra nos deixou com feridas visíveis e invisíveis.",
        "As criaturas mágicas são assustadoras, mas não vamos desistir sem lutar.",
        "Os portais são uma ameaça constante. Não podemos abaixar a guarda nem por um momento.",
        "Nossos guerreiros lutam bravamente, mas as criaturas mágicas são numerosas demais.",
        "A vila está em ruínas. Parece que a guerra nunca vai acabar.",
        "As criaturas mágicas não têm medo de nós. Elas avançam sem hesitar.",
        "Os portais são fontes de angústia e desespero para todos nós.",
        "A esperança é o que nos mantém firmes, mesmo quando tudo parece perdido.",
        "Nossas defesas são corajosas, mas as criaturas mágicas são implacáveis.",
        "A vila era um lugar pacífico, mas agora é um campo de batalha constante.",
        "Os portais são como feridas abertas, sangrando constantemente criaturas mágicas.",
        "A guerra nos fez perder muito, mas também mostrou nossa determinação.",
        "As criaturas mágicas vêm em diferentes formas, todas igualmente aterrorizantes.",
        "Nossos guerreiros são valentes, mas não podemos vencer essa guerra sozinhos.",
        "A vila costumava ser um lar seguro, mas agora é um lugar de medo constante.",
        "Os portais são como portões para um mundo sombrio e desconhecido.",
        "A guerra deixou marcas profundas em nossos corações.",
        "As criaturas mágicas são como pesadelos que ganham vida.",
        "Os portais são como feridas abertas, atraindo criaturas mágicas para nosso vilarejo.",
        "Nossos guerreiros lutam com bravura, mas a guerra parece não ter fim.",
        "A vila está cercada de perigo por todos os lados.",
        "As criaturas mágicas são poderosas, mas não vamos desistir sem lutar.",
        "Os portais são como portões para um mundo que não podemos controlar.",
        "A guerra nos trouxe sofrimento inimaginável.",
        "Nossos guerreiros são heróis, mas precisamos de mais ajuda para enfrentar as criaturas.",
        "A vila costumava ser um lugar de alegria, mas agora é um campo de batalha constante.",
        "Os portais são como feridas abertas, permitindo que as criaturas mágicas nos ataquem.",
        "A guerra nos uniu em nossa determinação de proteger nossa casa.",
        "As criaturas mágicas são assustadoras, mas não vamos nos curvar diante delas.",
        "Os portais são fontes constantes de medo e desespero.",
        "Nossos guerreiros são valentes, mas precisamos de reforços para combater as criaturas.",
        "A vila costumava ser um lugar seguro, mas agora é um campo de batalha constante.",
        "Os portais são como feridas abertas, sangrando constantemente criaturas mágicas.",
        "A guerra nos fez perder muito, mas também mostrou nossa determinação.",
        "As criaturas mágicas vêm em diferentes formas, todas igualmente aterrorizantes.",
        "Nossos guerreiros são valentes, mas não podemos vencer essa guerra sozinhos.",
        "A vila costumava ser um lar seguro, mas agora é um lugar de medo constante.",
        "Os portais são como portões para um mundo sombrio e desconhecido.",
        "A guerra deixou marcas profundas em nossos corações.",
        "As criaturas mágicas são como pesadelos que ganham vida.",
        "Os portais são como feridas abertas, atraindo criaturas mágicas para nosso vilarejo.",
        "Nossos guerreiros lutam com bravura, mas a guerra parece não ter fim.",
        "A vila está cercada de perigo por todos os lados.",
        "As criaturas mágicas são poderosas, mas não vamos desistir sem lutar.",
        "Os portais são como portões para um mundo que não podemos controlar.",
        "A guerra nos trouxe sofrimento inimaginável.",
        "Nossos guerreiros são heróis, mas precisamos de mais ajuda para enfrentar as criaturas.",
        "A vila costumava ser um lugar de alegria, mas agora é um campo de batalha constante.",
        "Os portais são como feridas abertas, permitindo que as criaturas mágicas nos ataquem.",
        "A guerra nos uniu em nossa determinação de proteger nossa casa.",
        "As criaturas mágicas são assustadoras, mas não vamos nos curvar diante delas.",
        "Os portais são fontes constantes de medo e desespero.",
        "Nossos guerreiros são valentes, mas precisamos de reforços para combater as criaturas.",
        "A guerra com as criaturas mágicas foi causada pelo homem encapuzado. Ele abriu os portais e trouxe a destruição para nossa vila.",
        "O homem encapuzado é o responsável por todas essas criaturas mágicas que nos atacam. Ele precisa ser detido.",
        "Essa guerra sem fim com as criaturas mágicas começou quando o homem encapuzado abriu os portais proibidos.",
        "As criaturas mágicas são apenas as marionetes do homem encapuzado. Ele as controla com suas trevas.",
        "Nosso vilarejo vivia em paz até que o homem encapuzado abriu os portais e trouxe a guerra para cá.",
        "O homem encapuzado é a fonte de todo esse caos. Enquanto ele estiver por perto, não haverá paz.",
        "As criaturas mágicas são apenas a ponta do iceberg. O verdadeiro perigo é o homem encapuzado que as comanda.",
        "A guerra com as criaturas mágicas é culpa do homem encapuzado. Ele deseja espalhar o caos e a escuridão.",
        "O homem encapuzado é um ser das trevas que trouxe essa guerra para nossa vila. Precisamos detê-lo.",
        "Essa guerra com as criaturas mágicas só vai acabar quando o homem encapuzado for derrotado.",
        "As criaturas mágicas são resultado dos poderes sombrios do homem encapuzado. Ele deve ser parado.",
        "O homem encapuzado é um inimigo mortal de nossa vila. Sua presença atrai as criaturas mágicas.",
        "Enquanto o homem encapuzado estiver vivo, nossa vila nunca estará a salvo das criaturas mágicas.",
        "A guerra com as criaturas mágicas começou com o homem encapuzado e só terminará com sua derrota.",
        "O homem encapuzado é um ser maligno que trouxe essa guerra para nossa vila. Ele precisa pagar por seus crimes.",
        "Essa guerra é resultado das ambições sombrias do homem encapuzado. Ele busca o poder absoluto.",
        "As criaturas mágicas são a manifestação dos desejos sombrios do homem encapuzado. Ele deve ser detido.",
        "O homem encapuzado é o responsável por todas essas criaturas mágicas que nos atacam. Ele precisa ser parado.",
        "Essa guerra sem fim com as criaturas mágicas começou quando o homem encapuzado abriu os portais proibidos.",
        "As criaturas mágicas são apenas as marionetes do homem encapuzado. Ele as controla com suas trevas.",
        "Nosso vilarejo vivia em paz até que o homem encapuzado abriu os portais e trouxe a guerra para cá.",
        "O homem encapuzado é a fonte de todo esse caos. Enquanto ele estiver por perto, não haverá paz.",
        "As criaturas mágicas são apenas a ponta do iceberg. O verdadeiro perigo é o homem encapuzado que as comanda.",
        "A guerra com as criaturas mágicas é culpa do homem encapuzado. Ele deseja espalhar o caos e a escuridão.",
        "O homem encapuzado é um ser das trevas que trouxe essa guerra para nossa vila. Precisamos detê-lo.",
        "Essa guerra com as criaturas mágicas só vai acabar quando o homem encapuzado for derrotado.",
        "As criaturas mágicas são resultado dos poderes sombrios do homem encapuzado. Ele deve ser parado.",
        "O homem encapuzado é um inimigo mortal de nossa vila. Sua presença atrai as criaturas mágicas.",
        "Enquanto o homem encapuzado estiver vivo, nossa vila nunca estará a salvo das criaturas mágicas.",
        "A guerra com as criaturas mágicas começou com o homem encapuzado e só terminará com sua derrota.",
        "O homem encapuzado é um ser maligno que trouxe essa guerra para nossa vila. Ele precisa pagar por seus crimes.",
        "Essa guerra é resultado das ambições sombrias do homem encapuzado. Ele busca o poder absoluto.",
    };


    public string getFraseAleatoriaBasica()
    {
        int index = Random.Range(0, dialogosAleatoriosBasicos.Length-1);
        return dialogosAleatoriosBasicos[index];
    }

    void Start()
    {

        dataGen = FindObjectOfType<DataGenerator>();
        stats = FindObjectOfType<PersistentStats>();

        soundStoryTelling = FindObjectOfType<AudioManager>();

        if (npcNormal == true && fraseNPC == "")
        {
            fraseNPC = getFraseAleatoriaBasica();
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (dialogueStarted)
        {
            if (Input.GetMouseButtonDown(0))
            {
                //print("The Left mouse button was pressed");
                //rodaDialogo();
            }
        }

    }


    public void rodaDialogo(int dialogo = -1)
    {

        if (npcNormal == false)
        {
            switch (dataGen.ato)
            {
                case 1: //ato 1
                    // quer dizer que o jogador está falando com o homem encapuzado dentro da caverna
                    if (SceneManager.GetActiveScene().name.ToString() == "MapGeneration")
                    {
                        if (dialogoHomemMisteriosoAto1Cave.Count > 0)
                        {
                            Debug.Log(dialogoAux + " " + dialogoHomemMisteriosoAto1Cave.Count);
                            if (dialogoAux < dialogoHomemMisteriosoAto1Cave.Count)
                            {
                                int dialogueToRun = dialogoAux;
                                if (dialogo != -1)
                                {
                                    dialogueToRun = dialogo;
                                }


                                int activeQuests = dataGen.activeQuests;
                                bool foundSecret = dataGen.foundSecret;

                                KeyValuePair<string, List<string>> primeiroDialogo = dialogoHomemMisteriosoAto1Cave.ElementAt(dialogueToRun);

                                List<string> respostas = primeiroDialogo.Value;
                                textDialogue.text = primeiroDialogo.Key;

                                if (respostas.Count > 0)
                                {
                                    buttonChoice1.SetActive(true);
                                    buttonChoice2.SetActive(true);
                                    buttonChoice3.SetActive(true);
                                    buttonChoicesContinue.SetActive(false);

                                    // ativar botões de decisões 1, 2 e 3
                                    string resposta1 = respostas[0];
                                    string resposta2 = respostas[1];
                                    string resposta3 = respostas[2];

                                    textChoice1.text = resposta1;
                                    textChoice2.text = resposta2;
                                    textChoice3.text = resposta3;

                                }
                                else
                                {
                                    // colocar botão de continuar
                                    buttonChoice1.SetActive(false);
                                    buttonChoice2.SetActive(false);
                                    buttonChoice3.SetActive(false);
                                    buttonChoicesContinue.SetActive(true);

                                }
                            }
                            else
                            {
                                continueChoices();
                            }

                        }
                    } else
                    {
                        if (dialogoHomemMisteriosoAto1.Count > 0)
                        {
                            Debug.Log(dialogoAux + " " + dialogoHomemMisteriosoAto1.Count);
                            if (dialogoAux < dialogoHomemMisteriosoAto1.Count)
                            {
                                int dialogueToRun = dialogoAux;
                                if (dialogo != -1)
                                {
                                    dialogueToRun = dialogo;
                                }


                                int activeQuests = dataGen.activeQuests;
                                bool foundSecret = dataGen.foundSecret;

                                KeyValuePair<string, List<string>> primeiroDialogo = dialogoHomemMisteriosoAto1.ElementAt(dialogueToRun);

                                List<string> respostas = primeiroDialogo.Value;
                                textDialogue.text = primeiroDialogo.Key;

                                if (respostas.Count > 0)
                                {
                                    buttonChoice1.SetActive(true);
                                    buttonChoice2.SetActive(true);
                                    buttonChoice3.SetActive(true);
                                    buttonChoicesContinue.SetActive(false);

                                    // ativar botões de decisões 1, 2 e 3
                                    string resposta1 = respostas[0];
                                    string resposta2 = respostas[1];
                                    string resposta3 = respostas[2];

                                    textChoice1.text = resposta1;
                                    textChoice2.text = resposta2;
                                    textChoice3.text = resposta3;

                                }
                                else
                                {
                                    // colocar botão de continuar
                                    buttonChoice1.SetActive(false);
                                    buttonChoice2.SetActive(false);
                                    buttonChoice3.SetActive(false);
                                    buttonChoicesContinue.SetActive(true);

                                }
                            }
                            else
                            {
                                continueChoices();
                            }

                        }
                    }
 
                    break;
                case 2:
                    if (dialogoHomemMisteriosoAto2.Count > 0)
                    {
                        if (dialogoAux < dialogoHomemMisteriosoAto2.Count)
                        {
                            int dialogueToRun = dialogoAux;
                            if (dialogo != -1)
                            {
                                dialogueToRun = dialogo;
                            }


                            int activeQuests = dataGen.activeQuests;
                            bool foundSecret = dataGen.foundSecret;

                            KeyValuePair<string, List<string>> primeiroDialogo = dialogoHomemMisteriosoAto2.ElementAt(dialogueToRun);

                            List<string> respostas = primeiroDialogo.Value;
                            textDialogue.text = primeiroDialogo.Key;

                            if (respostas.Count > 0)
                            {
                                buttonChoice1.SetActive(true);
                                buttonChoice2.SetActive(true);
                                buttonChoice3.SetActive(true);
                                buttonChoicesContinue.SetActive(false);

                                // ativar botões de decisões 1, 2 e 3
                                string resposta1 = respostas[0];
                                string resposta2 = respostas[1];
                                string resposta3 = respostas[2];

                                textChoice1.text = resposta1;
                                textChoice2.text = resposta2;
                                textChoice3.text = resposta3;

                            }
                            else
                            {
                                // colocar botão de continuar
                                buttonChoice1.SetActive(false);
                                buttonChoice2.SetActive(false);
                                buttonChoice3.SetActive(false);
                                buttonChoicesContinue.SetActive(true);

                            }
                        }
                        else
                        {
                            continueChoices();
                        }

                    }
                    break;
            }
                
        } else
        {
            textDialogue.text = fraseNPC;
        }
        
    }

    public void playerChoice(int decisao)
    {
        string escolha = textDialogue.text;

        switch (escolha)
        {
            case ("Você me lembra alguém que entrou nesse mesmo portal a uns anos atrás…"):
                switch (decisao)
                {
                    case 1:
                        string texto = "Seu pai? Então isso envolve sentimentalismo? ahahaha Acho que você pode se arrepender disso no final...";
                        adicionaNovoTextoDuranteFala(texto);
                        break;
                    case 2:
                        string texto2 = "Você está bem interessado para alguém que não conhece, não é mesmo? muahahahaha";
                        adicionaNovoTextoDuranteFala(texto2);
                        break;
                    case 3:
                        string texto3 = "Você tem poucas palavras... misterioso você... Vamos ver quanto tempo isso irá durar.";
                        adicionaNovoTextoDuranteFala(texto3);
                        break;
                }
                break;
        }
        continueChoices();
    }

    private void adicionaNovoTextoDuranteFala(string texto)
    {
        KeyValuePair<string, List<string>> novoDialogo = new KeyValuePair<string, List<string>>(texto, new List<string>());
        Dictionary<string, List<string>> novoDicionario = new Dictionary<string, List<string>>();
        if (dialogoAux + 1 >= 0 && dialogoAux + 1 < dialogoHomemMisteriosoAto1.Count)
        {
            // Copia os diálogos existentes até chegar à posição desejada
            int contador = 0;
            foreach (var dialogo in dialogoHomemMisteriosoAto1)
            {
                if (contador == dialogoAux + 1)
                {
                    novoDicionario.Add(novoDialogo.Key, novoDialogo.Value); // Insere o novo diálogo
                }

                novoDicionario.Add(dialogo.Key, dialogo.Value);
                contador++;
            }
        }
        else
        {
            // Caso o índice desejado seja maior ou igual ao número de diálogos existentes,
            // o novo diálogo será adicionado no final do dicionário
            novoDicionario = new Dictionary<string, List<string>>(dialogoHomemMisteriosoAto1);
            novoDicionario.Add(novoDialogo.Key, novoDialogo.Value);
        }

        dialogoHomemMisteriosoAto1 = novoDicionario;
    }

    public void liberaPortal()
    {

        // se for a segunda passagem, checa se o mapa veio gerado pela regressão

        // verifica se o jogador é combatente, velocista ou explorador

        // gera o mundo aqui na hora que libra o portal

        // Presets de várias variáveis que mudam em tempo real com a história



        HudDialogue.SetActive(false);
        firstTime = false;

        portal.SetActive(true);

    }


    void OnTriggerStay2D(Collider2D other)
    {

        if (other.gameObject.CompareTag("Player"))
        {
            if (Input.GetKeyUp(KeyCode.E))
            {
                Debug.Log("iniciou dialogo 1");
                dialogueStarted = true;
                HudDialogue.SetActive(true);
                rodaDialogo();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            dialogueStarted = false;
            HudDialogue.SetActive(false);
        }
    }

    public void continueChoices()
    {
        int countProxDialogo = 0;

        switch (dataGen.ato)
        {
            case 1:
                if (SceneManager.GetActiveScene().name.ToString() != "Game")
                {
                    countProxDialogo = dialogoHomemMisteriosoAto1Cave.Count;
                } else
                {
                    countProxDialogo = dialogoHomemMisteriosoAto1.Count;
                }
                break;
            case 2:
                countProxDialogo = dialogoHomemMisteriosoAto2.Count;
                break;
            case 3:
                break;
            default:
                break;
        }

        if (dialogoAux <= countProxDialogo)
        {
            dialogoAux++;
            rodaDialogo();
        } else
        {
            dialogoAux++;
            liberaPortal();
            homemEncapuzadoSome();
            fechaHud();
        }
    }

    public void fechaHud(){
        HudDialogue.SetActive(false);
    }

    public void homemEncapuzadoSome()
    {
        gameObject.SetActive(false);
        if (transform.parent != null)
        {
            transform.parent.gameObject.SetActive(false);
        } 
    }

    public void adicionaInimigos()
    {

    }

    public void adicionaMoedas()
    {

    }

    public void aumentaMapa()
    {

    }

    public void diminuiMapa()
    {

    }



}
