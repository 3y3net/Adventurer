using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameStates {
    Ignore,
    Intro1Played,           //Scene 01 Intro animation
    HasKnive,               //Cogido cuchillo pequeño
    AssadBoxOpened,         //Caja abierta
    AssadBoxWached,         //Caja vista
    HasTablet,              //Tiene tablet
    GetHieratico,           //Tiene papiro hieratico
    CartaAssadLeida,        //Ha leido la carta de Assad
    GetVasijaMala,          //Tiene la vasija de la caja
    CartuchoVasijaFound,    //(10) Ha visto en inventario el nombre de INTEF I
    PeriodoVasijaDone,      //Ha visto en inventario el cartucho de la vasija
    CartuchoEsDeIntef,      //Ha localizado el nombre del faraón en el libro
    PuzleVasijaHecho,       //Ha termiando el puzzle de la vasila
    TextoPapiro,            //Ha encontrado varias palabras en el papiro
    CalidadPapiro,          //Ha visto la calidad del papiro
    PuzzlePapiroHecho,      //Ha terminado el puzzle del papiro, pero no lo ha traducido entero
    TableVisible,           //Tablet visible como icono (ESTADO PURO)
    TieneLlaveCofre,        //Tiene la llave del cofre
    TieneDicHieratico,      //Ha encontrado el diccionario de hierático
    TraduceSimbolos,        //Ha encontrado la correlación de simbolos
    PapiroTraducido,        //Ha traducido el papiro entero y salta a escena desierto...
    RecuerdaLlaveCofre,     //Recuerda donde ha dejado la llave del cofre
    CofreAbierto,           //Cofre de diccionarios abierto
}

public enum GameValues {
    None, StartingTextIndex
}

public enum GameStrings
{
    StartingScene, StartingText
}

public class GameState : PersistentData {

    public bool[] gameStates = new bool[System.Enum.GetNames(typeof(GameStates)).Length];
    public int[] gameValues = new int[System.Enum.GetNames(typeof(GameValues)).Length];

    [TextArea]
    public string[] gameStrings = new string[System.Enum.GetNames(typeof(GameStrings)).Length];

    protected override string SetKey()
    {
        return "GameStates";
    }

    protected override void Save()
    {
        for (int i = 0; i < gameStates.Length; i++)
        {
            string k = key + "_" + ((GameStates)i).ToString();
            saveLoad.Save(k, gameStates[i]);
        }

        for (int i = 0; i < gameValues.Length; i++)
        {
            string k = key + "_" + ((GameValues)i).ToString();
            saveLoad.Save(k, gameValues[i]);
        }

        for (int i = 0; i < gameStrings.Length; i++)
        {
            string k = key + "_" + ((GameStrings)i).ToString();
            saveLoad.Save(k, gameStrings[i]);
        }
    }

    protected override void Load()
    {
        bool state=false;
        for (int i = 0; i < gameStates.Length; i++)
        {
            string k = key + "_" + ((GameStates)i).ToString();
            if (saveLoad.Load(k, ref state))
                gameStates[i] = state;

            int v = 0;
            k = key + "_" + ((GameValues)i).ToString();
            if (saveLoad.Load(k, ref v))
                gameValues[i] = v;

            string s = "";
            k = key + "_" + ((GameStrings)i).ToString();
            if (saveLoad.Load(k, ref s))
                gameStrings[i] = s;
        }
    }    
}
