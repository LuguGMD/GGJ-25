using TMPro;
using UnityEngine;

public class GameControllerGol : MonoBehaviour
{
    //Aqui vão ficar as variaveis
    public string ultimotocarbola;
    public TextMeshProUGUI Texto;

    void Start()
    {
        
    }

    void Update()
    {

    }

    private void OnCollisionEnter(Collision other)
    {
        if(ultimotocarbola == null)
        {
            ultimotocarbola = other.gameObject.name;
        } else if(ultimotocarbola !=  other.gameObject.name)
        {
            ultimotocarbola = other.gameObject.name;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "GolAzul")
        {
            Texto.text = $"{ultimotocarbola} fez um gol no Azul";
        }
        else if (other.gameObject.name == "GolAvermelho")
        {
            Texto.text = $"{ultimotocarbola} fez um gol no Vermelho";
        }
    }
}
