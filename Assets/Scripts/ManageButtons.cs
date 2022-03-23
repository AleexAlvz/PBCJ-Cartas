using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Faz o controle dos botoes do jogo inteiro
/// </summary>

public class ManageButtons : MonoBehaviour
{
    //Carrega o modo de jogo normal, feito em aula
    public void IniciaModoNormalDeJogo()
    {
        SceneManager.LoadScene("CartasModo1");
        PlayerPrefs.SetString("UltimoModoJogado", "ModoNormal"); //Salva o ultimo modo jogado no PlayerPrefs
    }

    //Carrega o primeiro modo customizado
    public void IniciaModoCustomizado1()
    {
        SceneManager.LoadScene("CartasModo2");
        PlayerPrefs.SetString("UltimoModoJogado", "ModoCustomizado1"); //Salva o ultimo modo jogado no PlayerPrefs
    }

    //Carrega o segundo modo customizado
    public void IniciaModoCustomizado2()
    {
        SceneManager.LoadScene("CartasModo3");
        PlayerPrefs.SetString("UltimoModoJogado", "ModoCustomizado2"); //Salva o ultimo modo jogado no PlayerPrefs
    }

    //Carrega tela de inicio
    public void VoltarParaInicio()
    {
        SceneManager.LoadScene("TelaInicial");
    }

    // Busca nas Prefs o ultimo modo jogado e inicia ele, quando o botão replay é clicado.
    public void IniciaUltimoModoJogado()
    {
        string ultimoModoJogado = PlayerPrefs.GetString("UltimoModoJogado");
        if (ultimoModoJogado == "ModoNormal")
        {
            IniciaModoNormalDeJogo();
        } else if (ultimoModoJogado == "ModoCustomizado1")
        {
            IniciaModoCustomizado1();
        } else if (ultimoModoJogado == "ModoCustomizado2")
        {
            IniciaModoCustomizado2();
        }
    }
}
