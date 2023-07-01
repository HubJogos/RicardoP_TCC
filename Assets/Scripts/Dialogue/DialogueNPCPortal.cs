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
        dialogos[0] = "Ah, finalmente você chegou, jovem aventureiro. Sinto a presença de seu coração valente. Meu nome não importa, mas saiba que estou aqui para guiá-lo em sua jornada.";
        dialogos[1] = "Não tenha pressa em saber meu nome, pois agora sou apenas um guia neste plano de existência. Sua vinda foi prevista nas entrelinhas do tempo e espaço. Recebeste uma mensagem de seu avô, não foi? Um sinal de que és o escolhido para cumprir um destino maior.";
        dialogos[2] = "Então, sua jornada começa agora. Nesta ilha escondida, o portal das sombras aguarda seu toque. Porém, tenha em mente que o poder que ele concede também despertou forças sombrias. Cabe a você encontrar um equilíbrio entre luz e sombras.";
        dialogos[3] = "O caminho para o portal é nebuloso e cheio de desafios. Primeiro, você deve buscar a experiência necessária para enfrentar o desconhecido. Explore este vilarejo, converse com seus habitantes, encontre os tesouros ocultos. Assim, poderás ganhar a sabedoria e força necessárias para restaurar a paz.";
        dialogos[4] = "Lembre-se, jovem aventureiro, nem tudo é o que parece. O destino está em suas mãos, e o equilíbrio depende de suas escolhas. Agora, permita-me fazer algumas perguntas, para que eu possa moldar o portal das sombras de acordo com sua personalidade e habilidades.";

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
            textoDialogo.text = "Vá. o Portal para a caverna está liberado.";
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
