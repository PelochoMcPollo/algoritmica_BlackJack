using UnityEngine;
using UnityEngine.UI;

public class Deck : MonoBehaviour
{
    public Sprite[] faces;
    public GameObject dealer;
    public GameObject player;
    public Button hitButton;
    public Button stickButton;
    public Button playAgainButton;
    public Text finalMessage;
    public Text probMessage;

    //Puntos
    public Text PointsText;
    public Text DealerPointsText;

    //Probabilidad
    public float totalcartas;
    public float cartasm21;
    public float cartas1721;
    public float dmp;

    //Apuesta
    public Dropdown Apuesta;
    public Text CreditsText;
    public float Credits = 1000;
    public float dinerillo;

    public int[] values = new int[52];
    int cardIndex = 0;    
       
    private void Awake()
    {    
        InitCardValues();        

    }

    private void Start()
    {
        ShuffleCards();
        hitButton.interactable = false;
        stickButton.interactable = false;      
        CreditsText.text = "Credits: " + Credits;  
    }

    private void InitCardValues()
    {
        /*TODO:
         * Asignar un valor a cada una de las 52 cartas del atributo "values".
         * En principio, la posición de cada valor se deberá corresponder con la posición de faces. 
         * Por ejemplo, si en faces[1] hay un 2 de corazones, en values[1] debería haber un 2.
         */
         for(int i=0; i<faces.Length;i++)
        {
            int InitCardValues = (i%13)+1;
            if(InitCardValues>10)
            {
                InitCardValues = 10;

            }
            if(InitCardValues == 1)
            {
                InitCardValues = 11;

            }
            values[i] = InitCardValues;
        }
    }

    private void ShuffleCards()
    {
        /*TODO:
         * Barajar las cartas aleatoriamente.
         * El método Random.Range(0,n), devuelve un valor entre 0 y n-1
         * Si lo necesitas, puedes definir nuevos arrays.
         */  

          for(int i=0; i<faces.Length;i++)
        {
            int j = Random.Range(0,faces.Length-1);
            Sprite tempFace = faces[i];
            faces[i] = faces[j];
            faces[j] = tempFace;

            int tempValue = values[i];
            values[i] = values[j];
            values[j] = tempValue;
        }
    }

    void StartGame()
    {
        Bet();
        CreditsText.text = "Credits: " + Credits;
        playAgainButton.interactable = false;
        Apuesta.interactable = false;
        DealerPointsText.text = "Points:";
        for (int i = 0; i < 2; i++)
        {
            PushPlayer();
            PushDealer();
            /*TODO:
             * Si alguno de los dos obtiene Blackjack, termina el juego y mostramos mensaje
             */
             if(player.GetComponent<CardHand>().points == 21)
             {
                finalMessage.text = "OLEEE AHIIIIII, HE GANAO";
                dealer.GetComponent<CardHand>().cards[0].GetComponent<CardModel>().ToggleFace(true);
                DealerPointsText.text = "Points: " +dealer.GetComponent<CardHand>().points; 
                Credits = Credits + (dinerillo*2);
                CreditsText.text = "Credits: " + Credits;
                hitButton.interactable = false;
                stickButton.interactable = false;
                playAgainButton.interactable = true;
                Apuesta.interactable = true;
             }
             if(dealer.GetComponent<CardHand>().points == 21)
             {
                finalMessage.text = "AYYYYYYY MI CUQUI, LO HE PERDIDO TOOOOOOOO!!!!!";
                dealer.GetComponent<CardHand>().cards[0].GetComponent<CardModel>().ToggleFace(true);
                DealerPointsText.text = "Points: " +dealer.GetComponent<CardHand>().points;  
                hitButton.interactable = false;
                stickButton.interactable = false;
                playAgainButton.interactable = true;
                Apuesta.interactable = true;
             }
             if(dealer.GetComponent<CardHand>().points == 21 && player.GetComponent<CardHand>().points == 21)
             {
                finalMessage.text = "NI HE GANAO NI HE PERDIO AHHHHHHHHHHHH";
                dealer.GetComponent<CardHand>().cards[0].GetComponent<CardModel>().ToggleFace(true);
                DealerPointsText.text = "Points: " +dealer.GetComponent<CardHand>().points;  
                Credits = Credits + dinerillo;
                CreditsText.text = "Credits: " + Credits;
                hitButton.interactable = false;
                stickButton.interactable = false;
                playAgainButton.interactable = true;
                Apuesta.interactable = true;
             }
        }
        PointsText.text = "Points: " +player.GetComponent<CardHand>().points;
    }

    private void CalculateProbabilities()
    {
        /*TODO:
         * Calcular las probabilidades de:
         * - Teniendo la carta oculta, probabilidad de que el dealer tenga más puntuación que el jugador
         * - Probabilidad de que el jugador obtenga entre un 17 y un 21 si pide una carta
         * - Probabilidad de que el jugador obtenga más de 21 si pide una carta          
         */

          totalcartas=0;
         float puntoplayer = player.GetComponent<CardHand>().points;
            cartasm21=0;
            cartas1721=0;
            dmp=0;

    for(int i = cardIndex+1; i < faces.Length; i++)
    {
        totalcartas++;

         if(values[3] + values[i] > puntoplayer)
        {
            dmp++;
        }


        if( values[i] == 11 && values[i] + puntoplayer > 21)
        {
            if(values[i] == 11)
            {
                values[i] = 1;
            }
        }



        if( values[i] + puntoplayer >= 17 && values[i] + puntoplayer <= 21 )
        {
            cartas1721++;  
        }


        if( values[i] + puntoplayer > 21)
        {
            cartasm21++;  
        }
    }
    
        probMessage.text =
        "Deal > Play: " + dmp/totalcartas + "\n" +
        "17 <= X <= 21: " + cartas1721/totalcartas + "\n" +
        "21 > X: " + cartasm21/totalcartas;

    }
    

    void PushDealer()
    {
        /*TODO:
         * Dependiendo de cómo se implemente ShuffleCards, es posible que haya que cambiar el índice.
         */
        dealer.GetComponent<CardHand>().Push(faces[cardIndex],values[cardIndex]);
        cardIndex++;        
    }

    void PushPlayer()
    {
        /*TODO:
         * Dependiendo de cómo se implemente ShuffleCards, es posible que haya que cambiar el índice.
         */
        player.GetComponent<CardHand>().Push(faces[cardIndex], values[cardIndex]/*,cardCopy*/);
        cardIndex++;
        CalculateProbabilities();
    }       

    public void Hit()
    {
        /*TODO: 
         * Si estamos en la mano inicial, debemos voltear la primera carta del dealer.
         */
        
        //Repartimos carta al jugador
        PushPlayer();
        /*TODO:
         * Comprobamos si el jugador ya ha perdido y mostramos mensaje
         */ 
        PointsText.text = "Ponits: " +player.GetComponent<CardHand>().points;     
        if(player.GetComponent<CardHand>().points>21)
        {
            finalMessage.text = "AYYYYYYY MI CUQUI, LO HE PERDIDO TOOOOOOOO!!!!!";
            hitButton.interactable = false;
            stickButton.interactable = false;
            playAgainButton.interactable = true;
            Apuesta.interactable = true;
            DealerPointsText.text = "Ponits: " +dealer.GetComponent<CardHand>().points;  
        }
        if(player.GetComponent<CardHand>().points == 21)
        {
           finalMessage.text = "OLEEE AHIIIIII, HE GANAO";
           hitButton.interactable = false;
           stickButton.interactable = false;
           playAgainButton.interactable = true;
           Apuesta.interactable = true;
           DealerPointsText.text = "Ponits: " +dealer.GetComponent<CardHand>().points;  
           Credits = Credits + (dinerillo*2);
           CreditsText.text = "Credits: " + Credits;
        }    

    }

    public void Stand()
    {
         /*TODO: 
         * Si estamos en la mano inicial, debemos voltear la primera carta del dealer.
         */
        dealer.GetComponent<CardHand>().cards[0].GetComponent<CardModel>().ToggleFace(true);

          
        /*TODO:
         * Repartimos cartas al dealer si tiene 16 puntos o menos
         * El dealer se planta al obtener 17 puntos o más
         * Mostramos el mensaje del que ha ganado
         */ 
        hitButton.interactable = false;
        stickButton.interactable = false;

        if(dealer.GetComponent<CardHand>().points < 17)
        {
            PushDealer();
        }


        DealerPointsText.text = "Ponits: " +dealer.GetComponent<CardHand>().points;  
        int PlayerPoints = player.GetComponent<CardHand>().points;
        int DealerPoints = dealer.GetComponent<CardHand>().points;
        

        if(DealerPoints == 21)
        {
            finalMessage.text = "AYYYYYYY MI CUQUI, LO HE PERDIDO TOOOOOOOO!!!!!";
            hitButton.interactable = false;
            stickButton.interactable = false;
            playAgainButton.interactable = true;
            Apuesta.interactable = true;
        }
        if(DealerPoints > PlayerPoints)
        {
            finalMessage.text = "AYYYYYYY MI CUQUI, LO HE PERDIDO TOOOOOOOO!!!!!";
            hitButton.interactable = false;
            stickButton.interactable = false;
            playAgainButton.interactable = true;
            Apuesta.interactable = true;
            
        }
         if(PlayerPoints > DealerPoints)
        {
            finalMessage.text = "OLEEE AHIIIIII, HE GANAO";
            hitButton.interactable = false;
            stickButton.interactable = false;
            Apuesta.interactable = true;
            playAgainButton.interactable = true;
            Credits = Credits + (dinerillo*2);
            CreditsText.text = "Credits: " + Credits;
            
             
        }
         if(DealerPoints == PlayerPoints)
        {
            finalMessage.text = "NI HE GANAO NI HE PERDIO AHHHHHHHHHHHH";
            hitButton.interactable = false;
            stickButton.interactable = false;
            Apuesta.interactable = true;
            playAgainButton.interactable = true;
            Credits = Credits + dinerillo;
            CreditsText.text = "Credits: " + Credits;
        
        }
        if(DealerPoints > 21)
        {
            finalMessage.text = "OLEEE AHIIIIII, HE GANAO";
            hitButton.interactable = false;
            stickButton.interactable = false;
            playAgainButton.interactable = true;
            Apuesta.interactable = true;
            Credits = Credits + (dinerillo*2);
            CreditsText.text = "Credits: " + Credits;
        }              
         
    }

    public void PlayAgain()
    {
        hitButton.interactable = true;
        stickButton.interactable = true;
        finalMessage.text = "";
        player.GetComponent<CardHand>().Clear();
        dealer.GetComponent<CardHand>().Clear();          
        cardIndex = 0;
        ShuffleCards();
        StartGame();
    }

    public void Bet()
    {
       switch(Apuesta.value) 
       {
        case 0:
            Credits=Credits-10;
            CreditsText.text = "Credits: " + Credits;
            dinerillo = 10;
            break;
       
        case 1:
            Credits=Credits-20;
            CreditsText.text = "Credits: " + Credits;
            dinerillo = 20;
            break;
       
        case 2:
            Credits=Credits-50;
            CreditsText.text = "Credits: " + Credits;
            dinerillo = 50;
            break;
       
        case 3:
            Credits=Credits-100;
            dinerillo = 100;
            CreditsText.text = "Credits: " + Credits;
            break;
       }

    }

    public void ONOFF()
    {
        switch(Apuesta.value)
        {
            case 0:
                if(Credits < 10)
                {
                    playAgainButton.interactable = false;
                }
                else
                {
                    playAgainButton.interactable = true;
                }
            break;
             case 1:
                if(Credits < 20)
                {
                    playAgainButton.interactable = false;
                }
                 else
                {
                    playAgainButton.interactable = true;
                }
            break;
             case 2:
                if(Credits < 50)
                {
                    playAgainButton.interactable = false;
                }
                 else
                {
                    playAgainButton.interactable = true;
                }
            break;
             case 3:
                if(Credits < 100)
                {
                    playAgainButton.interactable = false;
                }
                 else
                {
                    playAgainButton.interactable = true;
                }
            break;
        }
    }
    
}
