using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public int dado;
    public Node proximo;

    public Node(int dadoIndex)
    {
        dado = dadoIndex;
        proximo = null;
    }

    public void adicionarNoFinal(int dado)
    {
        if (proximo == null)
        {
            proximo = new Node(dado);
        }
        else
        {
            proximo.adicionarNoFinal(dado);
        }
    }
}

public class MinhaLista
{
    public static Node raizNode;
    public static int quantidade;

    public MinhaLista()
    {
        raizNode = null;
    }

    public void adicionarNoFinal(int dado)
    {
        if (raizNode == null)
        {
            quantidade++;
            raizNode = new Node(dado);
        }
        else
        {
            quantidade++;
            raizNode.adicionarNoFinal(dado);
        }
    }

    public int procurarNaLista(int dado)
    {
        bool existe = false;
        Node temp = raizNode;

        while (existe == false && temp != null)
        {
            if (temp.dado == dado)
            {
                existe = true;
            }
            else
            {
                if (temp.proximo != null)
                {
                    temp = temp.proximo;
                    existe = true;
                }
                else
                {
                    existe = true;
                }
            }
        }
        if (existe == false)
        {
            return 0;
        }
        return temp.dado;
    }

    public int procurarDadoNaPosicao(int dadoIndex)
    {
        if (dadoIndex > quantidade)
        {
            return 0;
        }

        int posicao = 0;
        bool existe = false;
        Node temp = raizNode;

        while (existe == false && temp != null)
        {
            if (posicao == dadoIndex)
            {
                existe = true;
            }
            else
            {
                if (temp.proximo != null)
                {
                    posicao++;
                    temp = temp.proximo;
                }
                else
                {
                    existe = true;
                }
            }
        }
        return temp.dado;
    }

    public void LimpaLista()
    {
        raizNode = null;
        quantidade = 0;
    }
}
