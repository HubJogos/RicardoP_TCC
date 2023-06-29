using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManagerStoryTelling : MonoBehaviour
{
    // Start is called before the first frame update

    PersistentStats stats;
    DataGenerator dataGen;

    [SerializeField]
    public string[] dialogosAleatoriosBasicos = 
    {
        "N�o tenho tempo para conversar. Tenho muito trabalho a fazer.",
        "As cavernas m�gicas s�o um lugar amaldi�oado. Voc� n�o deveria ir l�.",
        "O homem encapuzado � um perigo para todos n�s. Ele tem poderes sombrios.",
        "Voc� � novo por aqui? N�o se meta em encrenca.",
        "Voc� precisa de alguma coisa? Se n�o, me deixe em paz.",
        "As cavernas m�gicas guardam segredos antigos. Mas tamb�m armadilhas mortais.",
        "O homem encapuzado � um mist�rio. Ningu�m sabe quem ele � ou o que ele quer.",
        "Voc� parece perdido. Voc� sabe onde est� se metendo?",
        "Voc� n�o � bem-vindo aqui. Este � um vilarejo de gente honesta e trabalhadora.",
        "As cavernas m�gicas atraem muitos aventureiros. Mas poucos voltam de l�.",
        "O homem encapuzado � o l�der de uma seita maligna. Eles adoram as for�as das trevas.",
        "Voc� tem alguma coisa a ver com o homem encapuzado? Se tiver, � melhor sair daqui.",
        "As cavernas m�gicas s�o um tesouro para os curiosos. Mas tamb�m um pesadelo para os imprudentes.",
        "O homem encapuzado est� sempre rondando as cavernas m�gicas. Ele procura algo l� dentro.",
        "Voc� veio explorar as cavernas m�gicas? Voc� � corajoso ou louco?",
        "Tenho pouco a dizer para estrangeiros. N�o confio neles.",
        "As cavernas m�gicas escondem segredos antigos. Respeite sua magia e mantenha-se afastado.",
        "O homem encapuzado � um sinal de problemas iminentes. Fique alerta.",
        "V� embora. N�o precisamos de forasteiros em nosso vilarejo.",
        "As cavernas m�gicas j� cobraram um alto pre�o. Muitos foram perdidos l� dentro.",
        "N�o fale comigo. Estranhos sempre causam problemas.",
        "Cuidado com as cavernas m�gicas. Elas engolem os tolos que se aventuram l�.",
        "N�o h� lugar para estranhos em nossa comunidade. Mantenha-se afastado.",
        "As cavernas m�gicas s�o perigosas demais. Eu n�o arriscaria entrar nelas.",
        "O homem encapuzado traz consigo a escurid�o. Fique longe dele.",
        "V� embora. Voc� n�o � bem-vindo aqui.",
        "As cavernas m�gicas s�o o lar de criaturas malignas. Fique longe delas.",
        "Estrangeiros n�o t�m lugar em nosso vilarejo. V� embora.",
        "As cavernas m�gicas s�o um labirinto mortal. Melhor n�o se perder l� dentro.",
        "Cuidado com o homem encapuzado. Ele n�o hesitar� em causar danos.",
        "N�o se aproxime das cavernas m�gicas. Elas t�m um poder sombrio.",
        "Voc� n�o parece daqui. Mantenha-se fora de problemas.",
        "As cavernas m�gicas s�o uma porta para o desconhecido. E poucos sobrevivem para contar a hist�ria.",
        "O homem encapuzado � um pesadelo que assombra nossos sonhos.",
        "Vilarejos como este n�o s�o para pessoas como voc�. V� embora.",
        "As cavernas m�gicas est�o cheias de armadilhas trai�oeiras. N�o seja imprudente.",
        "O homem encapuzado � um espectro que assombra nosso vilarejo.",
        "Estrangeiros n�o s�o bem-vindos em nossa terra. Parta agora.",
        "As cavernas m�gicas t�m uma aura de perigo. Mantenha-se afastado delas.",
        "O homem encapuzado � um sinal de desgra�a. Fique longe dele se quiser viver.",
        "N�o h� lugar para forasteiros aqui. V� embora antes que algo aconte�a.",
        "As cavernas m�gicas s�o um portal para o desconhecido. E poucos retornam.",
        "O homem encapuzado � um mal que devemos evitar a todo custo.",
        "Voc� n�o � daqui. Mantenha-se afastado dos problemas.",
        "As cavernas m�gicas s�o um enigma que poucos conseguem decifrar.",
        "O homem encapuzado traz consigo a escurid�o. Fique longe dele, se puder.",
        "Vilarejos como este n�o s�o para estrangeiros. Parta agora mesmo.",
        "As cavernas m�gicas s�o trai�oeiras. N�o se arrisque sem motivo.",
        "O homem encapuzado � uma sombra que nos aterroriza.",
        "N�o h� espa�o para estranhos em nosso vilarejo. V� embora antes que se arrependa.",
        "As cavernas m�gicas s�o um mundo � parte. Melhor n�o se aventurar l�.",
        "O homem encapuzado � um mist�rio que nos assombra.",
        "Estrangeiros n�o s�o bem-vindos aqui. Parta imediatamente.",
        "As cavernas m�gicas s�o trai�oeiras. Apenas os tolos ousam entrar.",
        "O homem encapuzado traz consigo a escurid�o. Fique longe se quiser sobreviver.",
        "Voc� n�o � daqui. Deveria partir antes que algo de ruim aconte�a.",
        "As cavernas m�gicas s�o um desafio que poucos conseguem superar.",
        "O homem encapuzado � uma amea�a que n�o podemos ignorar.",
        "N�o confio em estrangeiros. Eles sempre trazem problemas.",
        "Cuidado com as cavernas m�gicas. Elas escondem perigos mortais.",
        "O homem encapuzado � a personifica��o do mal. N�o se aproxime dele.",
        "Voc� n�o � bem-vindo aqui. Saia antes que eu mude de ideia.",
        "As cavernas m�gicas s�o trai�oeiras. N�o se aventure sem motivo.",
        "O homem encapuzado � uma figura sinistra que devemos temer.",
        "Forasteiros n�o t�m lugar em nosso vilarejo. Parta agora.",
        "As cavernas m�gicas s�o um enigma mortal. Fique longe delas.",
        "O homem encapuzado � um dem�nio em forma humana. N�o se envolva com ele.",
        "N�o confie em estranhos. Eles sempre t�m segundas inten��es.",
        "Cuidado com as cavernas m�gicas. Elas podem levar sua vida em um piscar de olhos.",
        "O homem encapuzado � uma amea�a que paira sobre n�s.",
        "Voc� n�o � daqui. Mantenha-se afastado se quiser continuar respirando.",
        "As cavernas m�gicas s�o um lugar proibido. N�o as desafie.",
        "O homem encapuzado � uma sombra que se alimenta de medo.",
        "N�o temos tempo para conversa fiada. Desapare�a daqui.",
        "As cavernas m�gicas s�o o lar de criaturas malignas. � melhor voc� n�o encontrar nenhuma delas.",
        "O homem encapuzado � o executor das trevas. Ele traz consigo a morte.",
        "Voc� n�o � bem-vindo em nosso vilarejo. Saia antes que eu chame os guardas.",
        "As cavernas m�gicas s�o um labirinto sem sa�da. N�o se perca l� dentro.",
        "O homem encapuzado � um mensageiro da desgra�a. N�o se aproxime dele.",
        "N�o me incomode com suas palavras. N�o tenho tempo para forasteiros.",
        "As cavernas m�gicas s�o um lugar onde a luz n�o chega. Fique longe delas.",
        "O homem encapuzado � um s�mbolo da destrui��o que se aproxima.",
        "Estrangeiros n�o t�m lugar em nosso vilarejo. N�o insista.",
        "As cavernas m�gicas s�o um abismo de mist�rios. Melhor n�o se aventurar l� dentro.",
        "O homem encapuzado � uma abomina��o que n�o deve ser enfrentada.",
        "N�o confie em estranhos. Eles s� trazem problemas.",
        "Cuidado com as cavernas m�gicas. Elas s�o trai�oeiras e podem te engolir.",
        "O homem encapuzado � um espectro que nos assombra � noite.",
        "Vilarejos como este n�o s�o para pessoas como voc�. V� embora enquanto ainda pode.",
        "As cavernas m�gicas s�o um portal para o desconhecido. E poucos retornam de l� vivos.",
        "O homem encapuzado � um sinal de problemas iminentes. Mantenha-se alerta.",
        "N�o h� lugar para estranhos em nosso vilarejo. Parta agora ou sofra as consequ�ncias.",
        "As cavernas m�gicas j� cobraram um alto pre�o em vidas. N�o seja o pr�ximo.",
        "N�o fale comigo. Estrangeiros sempre causam problemas por aqui.",
        "Cuidado com as cavernas m�gicas. Elas t�m um poder sombrio que atrai os incautos.",
        "N�o h� espa�o para estranhos em nossa comunidade. Mantenha-se afastado.",
        "As cavernas m�gicas s�o perigosas demais. Eu n�o arriscaria entrar nelas, se fosse voc�.",
        "O homem encapuzado traz consigo a escurid�o. Fique longe dele, se quiser preservar sua vida.",
        "V� embora. Voc� n�o � bem-vindo aqui, e n�o temos tempo para perder com forasteiros.",
        "As cavernas m�gicas s�o o lar de criaturas malignas e armadilhas mortais. N�o v� l�.",
        "Estrangeiros n�o s�o bem-vindos em nosso vilarejo. Parta imediatamente, antes que se arrependa.",
        "As cavernas m�gicas s�o um tesouro para os curiosos, mas tamb�m um pesadelo para os imprudentes.",
        "O homem encapuzado est� sempre rondando as cavernas m�gicas. Parece estar em busca de algo.",
        "Voc� veio explorar as cavernas m�gicas? Voc� � corajoso ou apenas louco?",
        "As cavernas m�gicas s�o um mist�rio que poucos desvendaram. Tenha cautela ao adentr�-las.",
        "O homem encapuzado � uma figura sinistra que amedronta nosso vilarejo. Evite-o a todo custo.",
        "N�o confio em estrangeiros. Eles s� trazem problemas e desordem.",
        "Cuidado com as cavernas m�gicas. Elas escondem segredos sombrios e perigos mortais.",
        "O homem encapuzado � a personifica��o do mal. N�o se aproxime dele, a menos que queira seu fim.",
        "Voc� n�o � bem-vindo em nosso vilarejo. Saia antes que as coisas se compliquem para o seu lado.",
        "As cavernas m�gicas s�o um desafio para os mais valentes. Voc� tem o que � preciso?",
        "O homem encapuzado � uma amea�a que paira sobre nossa tranquilidade. Fique longe dele.",
        "N�o h� espa�o para estranhos em nossa comunidade. Parta imediatamente, antes que eu mude de ideia.",
        "As cavernas m�gicas s�o trai�oeiras. N�o se aventure sem uma boa raz�o.",
        "O homem encapuzado � uma sombra sinistra que devemos temer. Mantenha dist�ncia dele.",
        "Voc� n�o � daqui. Deveria partir antes que cause problemas.",
        "As cavernas m�gicas s�o um enigma que poucos conseguem decifrar. Pense bem antes de explor�-las.",
        "O homem encapuzado traz consigo a escurid�o. Fique longe se quiser preservar sua sanidade.",
        "N�o temos tempo para estranhos. Desapare�a antes que eu me arrependa de n�o ter chamado os guardas.",
        "As cavernas m�gicas s�o um labirinto sem sa�da. N�o se perca e nem arrisque sua vida � toa.",
        "O homem encapuzado � um mensageiro da desgra�a. N�o se aproxime dele, a menos que queira seu fim.",
        "N�o me incomode com suas palavras vazias. N�o tenho paci�ncia para lidar com estrangeiros.",
        "As cavernas m�gicas s�o um lugar onde a luz n�o chega. � melhor voc� se manter longe.",
        "O homem encapuzado � um s�mbolo da destrui��o que se aproxima. Fuja enquanto pode.",
        "Estrangeiros n�o s�o bem-vindos em nosso vilarejo. N�o insista, ou enfrentar� as consequ�ncias.",
        "As cavernas m�gicas s�o trai�oeiras. N�o se aventure sem um bom motivo.",
        "O homem encapuzado � uma abomina��o que n�o deve ser enfrentada. Fique longe dele.",
        "N�o confie em estranhos. Eles sempre trazem problemas e perturba��es.",
        "Cuidado com as cavernas m�gicas. Elas s�o trai�oeiras e podem te devorar sem piedade.",
        "O homem encapuzado � um espectro que assombra nossos pensamentos mais sombrios.",
        "Vilarejos como este n�o s�o para pessoas como voc�. V� embora antes que seja tarde demais.",
        "As cavernas m�gicas s�o um portal para o desconhecido. E poucos retornam de l� vivos.",
        "O homem encapuzado � um press�gio de problemas iminentes. Mantenha-se alerta.",
        "N�o h� lugar para estranhos em nosso vilarejo. Parta agora ou sofra as consequ�ncias.",
        "As cavernas m�gicas j� cobraram um alto pre�o em vidas. N�o seja o pr�ximo a pagar.",
        "N�o fale comigo. Estrangeiros sempre causam problemas por aqui.",
        "Cuidado com as cavernas m�gicas. Elas t�m um poder sombrio que atrai os incautos.",
        "N�o h� espa�o para estranhos em nossa comunidade. Mantenha-se afastado.",
        "As cavernas m�gicas s�o perigosas demais. Eu n�o arriscaria entrar nelas, se fosse voc�.",
        "O homem encapuzado traz consigo a escurid�o. Fique longe dele, se quiser preservar sua vida.",
        "V� embora. Voc� n�o � bem-vindo aqui, e n�o temos tempo para perder com forasteiros."
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

        var output = JsonUtility.ToJson(stats, true);
        Debug.Log("Stats: " + output);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
