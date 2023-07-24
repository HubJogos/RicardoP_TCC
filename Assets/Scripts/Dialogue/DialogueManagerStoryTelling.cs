using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
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
    private Button buttonChoice1;
    [SerializeField]
    private Button buttonChoice2;
    [SerializeField]
    private Button buttonChoice3;

    public AudioSource soundEnviroment;

    private int ato = 1;

    [SerializeField]
    public string fraseNPC = "";

    [SerializeField]
    public Dictionary<string, int> Dialogo;
    public int dialogoAux = 0;


    Dictionary<string, List<string>> dialogoHomemMisteriosoAto1 = new Dictionary<string, List<string>>
    {
        { "Você me lembra alguém que entrou nesse mesmo portal a uns anos atrás…", new List<string> { "Era o meu pai.", "E o que aconteceu com este homem?", "..." } },
        { "Não tenha pressa em saber meu nome, pois agora sou apenas um guia neste plano de existência. Sua vinda foi prevista nas entrelinhas do tempo e espaço. Recebeste uma mensagem de seu avô, não foi? Um sinal de que és o escolhido para cumprir um destino maior.", new List<string> { "Cale a boca!", "Não sei dizer.", "Vá embora." } },
        { "Então, sua jornada começa agora. Nesta ilha escondida, o portal das sombras aguarda seu toque. Porém, tenha em mente que o poder que ele concede também despertou forças sombrias. Cabe a você encontrar um equilíbrio entre luz e sombras.", new List<string>() },
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


    public string getFraseAleatoriaBasica()
    {
        int index = Random.Range(0, dialogosAleatoriosBasicos.Length-1);
        return dialogosAleatoriosBasicos[index];
    }

    void Start()
    {

        dataGen = FindObjectOfType<DataGenerator>();
        stats = FindObjectOfType<PersistentStats>();

        if (npcNormal == true && fraseNPC == "")
        {
            fraseNPC = getFraseAleatoriaBasica();
        } 


        if (dataGen.playthroughs == 1)
        {
            Debug.Log("Estou de volta, vivo ?");
        } else {
            if (dataGen.playthroughs > 1){
                Debug.Log("Voltei tantas vezes que nem me importo mais.");
            }

        }

        var outputStats = JsonUtility.ToJson(stats, true);
        var outputDataGen = JsonUtility.ToJson(dataGen, true);
        Debug.Log("Stats: " + outputStats);
        Debug.Log("DataGen: " + outputDataGen);

    }

    // Update is called once per frame
    void Update()
    {
        if (dialogueStarted)
        {
            if (Input.GetMouseButtonDown(0))
            {
                print("The Left mouse button was pressed");
                rodaDialogo();
            }
        }

    }


    public void rodaDialogo()
    {

        if (npcNormal == false)
        {
            if (dialogoHomemMisteriosoAto1.Count > 0)
            {
                // Obtém o primeiro diálogo (pergunta) do dicionário
                KeyValuePair<string, List<string>> primeiroDialogo = dialogoHomemMisteriosoAto1.First();
  
                List<string> respostas = primeiroDialogo.Value;
                textDialogue.text = primeiroDialogo.Key;

                if (respostas.Count > 0)
                {
                    // ativar botões de decisões 1, 2 e 3
                    string resposta1 = respostas[0];
                    string resposta2 = respostas[1];
                    string resposta3 = respostas[2];
  
                    textChoice1.text = resposta1;
                    textChoice2.text = resposta2;
                    textChoice3.text = resposta3;


                } else
                {
                    // colocar botão de continuar
                }

            }
                
        } else
        {
            textDialogue.text = fraseNPC;
        }
        

    }

    public void rodaPergunta()
    {

    }

    public void liberaPortal()
    {
        HudDialogue.SetActive(false);
        firstTime = false;

        portal.SetActive(true);

    }

    void OnTriggerStay2D(Collider2D other)
    {

        if (other.gameObject.CompareTag("Player"))
        {
            //Debug.Log("Entrou.....");
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("iniciou dialogo 1");
                dialogueStarted = true;
                HudDialogue.SetActive(true);
                rodaDialogo();
                //TriggerDialogue();
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //Debug.Log("Entrou ############");
            soundEnviroment.Pause();
            if (Input.GetKeyDown(KeyCode.E))
            {

                //Debug.Log("Iniciou dialogo 2");
                //TriggerDialogue();
            }
            //interactionText.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Saiu");
            dialogueStarted = false;
            HudDialogue.SetActive(false);
            //soundEnviroment.Play();
            //firstTime = false;

            //liberaPortal();
            //interactionText.SetActive(false);
        }
    }


    public void fechaHud(){
        HudDialogue.SetActive(false);
    }

}
