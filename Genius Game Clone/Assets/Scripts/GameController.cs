using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public enum GameState
{
    SEQUENCIA,
    RESPONDER,
    NOVA,
    ERRO
}

public class GameController : MonoBehaviour
{
    public GameState gameState;
    public List<Jogador> listaJogadores;

    public Text rodadaTxt, sequenciaTxt;
    public Text textoRanking;

    public Color[] cor;
    public Image[] imagemButton;
    public GameObject startButton;    
    public MinhaLista listCores;
    private AudioSource fonteAudio;
    public AudioClip[] sons;
    public int idResposta, sequenciaCores, rodada;

    #region Start
    void Start()
    {
        listCores = new MinhaLista();
        fonteAudio = GetComponent<AudioSource>();
        novaRodada();
    }    
    #endregion

    #region Salvar Sequencia

    void SalvarSequencias()
    {
        StreamWriter arqSaida = new StreamWriter("TamanhoSequencias.txt", true);           

        arqSaida.Write("Sequência de " + SortearNomes() + ":" + MinhaLista.quantidade + ";");

        arqSaida.Close();
    }
    #endregion
    
    #region Sortear nomes para simular alguem no Txt
    public string SortearNomes()
    {
        string[] Nomes = new string[10];

        Nomes[0] = "Agostinho Carrara";
        Nomes[1] = "Chico Bioca";
        Nomes[2] = "Samu";
        Nomes[3] = "Marilene";
        Nomes[4] = "Vittas";
        Nomes[5] = "Tigas";
        Nomes[6] = "Jonathan da nova geração";
        Nomes[7] = "Galo cego";
        Nomes[8] = "Jailson Mendes";
        Nomes[9] = "Bergue";

        int nomeRandomico = Random.Range(0,9);

        return Nomes[nomeRandomico];
    }
    #endregion
    
    #region Ler jogadores salvos no Txt
    string[] LerJogadoresRanking()
    {
        StreamReader arqEntrada = new StreamReader(@"C:\Users\Rodrigo\Documents\Trabalho AED - Genius\TamanhoSequencias.txt");

        string linha = arqEntrada.ReadToEnd();
        
        string [] jogadores = linha.Split(';');
               
        return jogadores;
    }
    #endregion

    #region Carregamento do Ranking
    public void CarregarRanking()
    {
        string[]jogadores = LerJogadoresRanking();
        
        if(jogadores.Length -1 == listaJogadores.Count) { return; }
        
        listaJogadores.Clear();

        foreach(string jogador in jogadores)
        {
            if(!jogador.Contains(":")){continue;} 

            Jogador jogadorClass = new Jogador();

            string [] valores = jogador.Split(':');

            jogadorClass.nomeJogador = valores[0];
            jogadorClass.totalSequencia = int.Parse(valores[1]);

            listaJogadores.Add(jogadorClass);
        }

        OrdenandoRanking();
        MostrarNomesRanking();
    }
    #endregion

    #region Fazendo formatação para mostrar o Ranking no Text
    void MostrarNomesRanking()
    {
        string nomeFormatado = string.Empty;

        foreach(Jogador jogador in listaJogadores)
        {
            nomeFormatado += jogador.nomeJogador + ": " + jogador.totalSequencia + "\n";
        }

        textoRanking.text = nomeFormatado;
    }
    #endregion

    #region Ordenando Ranking que foi salvo no txt com Selection Sort
    void OrdenandoRanking()
    {
        int maior;
        int tamVetor = listaJogadores.Count;
        bool trocou;

        Debug.Log("Ordenando");
        for (int i = 0; i < tamVetor - 1; i++)
        {
            maior = i;
            trocou = false;

            for (int j = i + 1; j < tamVetor; j++)
            {                
                if (listaJogadores[maior].totalSequencia < listaJogadores[j].totalSequencia)
                {
                    trocou = true;
                    maior = j;
                }
            }

            if (!trocou){ return; }

            if (i != maior)
            {                
                trocarElementos(maior, i);
            }
        }
    }
    void trocarElementos(int p1, int p2)
    {
        Jogador aux = listaJogadores[p1];
        listaJogadores[p1] = listaJogadores[p2];
        listaJogadores[p2] = aux;
    }
    #endregion

    #region Lógica do Genius usando Lista e Coroutine
    public void StartRodada()
    {
        StartCoroutine("mostrarSequencia", sequenciaCores + rodada);
    } 

    void novaRodada()
    {
        idResposta = 0;

        foreach (Image imagem in imagemButton)
        {
            imagem.color = cor[0];
        }

        startButton.SetActive(true);

        rodadaTxt.text = "Rodada: " + (rodada + 1).ToString();
        sequenciaTxt.text = "Sequência: " + (sequenciaCores + rodada).ToString();
    }

    IEnumerator novaCor()
    {
        startButton.SetActive(false);

        int random = Random.Range(0, imagemButton.Length);
        imagemButton[random].color = cor[1];
        fonteAudio.PlayOneShot(sons[random]);

        listCores.adicionarNoFinal(random);

        yield return new WaitForSeconds(0.5f);

        imagemButton[random].color = cor[0];

        gameState = GameState.RESPONDER;
    }

    IEnumerator responder(int idBotao)
    {
        imagemButton[idBotao].color = cor[1];
        
        if (listCores.procurarDadoNaPosicao(idResposta) == idBotao)
        {
            Debug.Log("Correto");
            fonteAudio.PlayOneShot(sons[idBotao]);
        }
        else
        {
            Debug.Log("Incorreto");
            gameState = GameState.ERRO;            
            StartCoroutine("gameOver");           
        }

        idResposta += 1;

        if (idResposta == MinhaLista.quantidade)
        {
            gameState = GameState.NOVA;
            rodada += 1;
            yield return new WaitForSeconds(0.3f);
            novaRodada();
        }

        yield return new WaitForSeconds(0.3f);
        imagemButton[idBotao].color = cor[0];
    }

    IEnumerator gameOver()
    {
        SalvarSequencias();

        rodada = 0;
        listCores.LimpaLista();

        fonteAudio.PlayOneShot(sons[4]);

        yield return new WaitForSeconds(0.9f);

        for (int i = 3; i >= 0; i--)
        {
            foreach (Image image in imagemButton)
            {
                image.color = cor[1];
            }

            yield return new WaitForSeconds(0.3f);

            foreach (Image image in imagemButton)
            {
                image.color = cor[0];
            }

            yield return new WaitForSeconds(0.3f);
        }

        int idButt = 0;
        for (int i = 12; i > 0; i--)
        {
            imagemButton[idButt].color = cor[1];

            yield return new WaitForSeconds(0.1f);

            imagemButton[idButt].color = cor[0];

            idButt += 1;

            if (idButt == 4) { idButt = 0; }
        }

        gameState = GameState.NOVA;
        novaRodada();
    }

    IEnumerator mostrarSequencia()
    {
        for (int i = 0; i < MinhaLista.quantidade; i++)
        {

            foreach (Image image in imagemButton)
            {
                image.color = cor[0];
            }

            imagemButton[listCores.procurarDadoNaPosicao(i)].color = cor[1];
            fonteAudio.PlayOneShot(sons[listCores.procurarDadoNaPosicao(i)]);

            yield return new WaitForSeconds(0.5f);

            imagemButton[listCores.procurarDadoNaPosicao(i)].color = cor[0];

            yield return new WaitForSeconds(0.5f);
        }
        StartCoroutine("novaCor", sequenciaCores + rodada);
    }
    #endregion
}
