using UnityEngine;

public class PipeMovement : MonoBehaviour
{
    public float moveSpeed = 3f; // Velocidade de movimentação dos canos

    void Update()
    {
        Move();
        if(this.transform.position.x <= -11)
        {
            destroi();
        }
    }

    void Move()
    {
        transform.position += Vector3.left * moveSpeed * Time.deltaTime; // Movimenta o cano para a esquerda

        // Se o cano sair do campo de visão, destrua-o para economizar recursos
        if (transform.position.x < -15f) // Define o limite de remoção do cano
        {
            Destroy(gameObject);
        }
    }
    void destroi()
    {
        Destroy(this.gameObject);
    }
}
