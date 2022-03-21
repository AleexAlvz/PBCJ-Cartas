using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Faz o controle dos botoes do jogo inteiro
/// </summary>
public class ManageButtons : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Carrega o modo de jogo normal, feito em aula
    public void IniciaModoNormalDeJogo()
    {
        SceneManager.LoadScene("CartasModo1");
    }

    //Carrega o primeiro modo customizado
    public void IniciaModoCustomizado1()
    {
        SceneManager.LoadScene("CartasModo2");
    }

    //Carrega o segundo modo customizado
    public void IniciaModoCustomizado2()
    {
        SceneManager.LoadScene("CartasModo3");
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
