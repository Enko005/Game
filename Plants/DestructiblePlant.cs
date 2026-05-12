using System;
using UnityEngine;
using System.Collections;

public class DestructiblePlant : MonoBehaviour
{
    public event EventHandler OnDestructibleTakeDamage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Sword>())
        {
            OnDestructibleTakeDamage?.Invoke(this, EventArgs.Empty);

            StartCoroutine(DestroyAfterPhysics());
        }
    }

    private IEnumerator DestroyAfterPhysics()
    {
        yield return null;

        Destroy(gameObject);

        if (NavMeshSurfaceManagement.Instance != null)
        {
            NavMeshSurfaceManagement.Instance.RebakeNavmeshSurface();
        }
    }
}