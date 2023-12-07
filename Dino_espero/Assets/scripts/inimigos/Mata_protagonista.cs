using UnityEngine;

public class Mata_protagonista : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Verifica se o objeto colidido possui a tag "Player"
        {
            BirdController bird = other.GetComponent<BirdController>();
            if (bird != null)
            {
                bird.Die(); // Chama o método 'Die()' no script do pássaro para tratar a morte do protagonista
            }
        }
    }
}
