using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueNPCPortal : MonoBehaviour
{
    // Start is called before the first frame update

    public bool firstTime = true;
    public bool backToThePortal = false;

    public string[] dialogos;
    public int dialogoAux = 0;
    public string nomePersonagem = "Homem misterioso";

    public int playerResponse = 0;

    public GameObject portal;

    public TMPro.TextMeshProUGUI textoDialogo;
    
    public GameObject Hud_dialogue;
    public GameObject buttonLiberatePortal;

    public AudioSource soundEnviroment;

    void Start()
    {
        dialogos[0] = "Ah, finalmente voc� chegou, jovem aventureiro. Sinto a presen�a de seu cora��o valente. Meu nome n�o importa, mas saiba que estou aqui para gui�-lo em sua jornada.";
        dialogos[1] = "N�o tenha pressa em saber meu nome, pois agora sou apenas um guia neste plano de exist�ncia. Sua vinda foi prevista nas entrelinhas do tempo e espa�o. Recebeste uma mensagem de seu av�, n�o foi? Um sinal de que �s o escolhido para cumprir um destino maior.";
        dialogos[2] = "Ent�o, sua jornada come�a agora. Nesta ilha escondida, o portal das sombras aguarda seu toque. Por�m, tenha em mente que o poder que ele concede tamb�m despertou for�as sombrias. Cabe a voc� encontrar um equil�brio entre luz e sombras.";
        dialogos[3] = "O caminho para o portal � nebuloso e cheio de desafios. Primeiro, voc� deve buscar a experi�ncia necess�ria para enfrentar o desconhecido. Explore este vilarejo, converse com seus habitantes, encontre os tesouros ocultos. Assim, poder�s ganhar a sabedoria e for�a necess�rias para restaurar a paz.";
        dialogos[4] = "Lembre-se, jovem aventureiro, nem tudo � o que parece. O destino est� em suas m�os, e o equil�brio depende de suas escolhas. Agora, permita-me fazer algumas perguntas, para que eu possa moldar o portal das sombras de acordo com sua personalidade e habilidades.";

    }

    // Update is called once per frame
    void Update()
    {
        if (Hud_dialogue.active)
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

        if (firstTime == true)
        {
            if (dialogoAux < dialogos.Length)
            {
                textoDialogo.text = dialogos[dialogoAux];
                dialogoAux++;
            } else
            {
                if (Hud_dialogue.active)
                {
                    buttonLiberatePortal.SetActive(true);

                }
            }
            
        } else
        {
            textoDialogo.text = "V�. o Portal para a caverna est� liberado.";
        }
        
    }

    public void rodaPergunta()
    {

    }

    public void liberaPortal()
    {
        Hud_dialogue.SetActive(false);
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
                Hud_dialogue.SetActive(true);
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
            Hud_dialogue.SetActive(false);
            soundEnviroment.Play();
            //firstTime = false;
            
            //liberaPortal();
            //interactionText.SetActive(false);
        }
    }
}
