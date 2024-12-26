namespace Player
{
    public class PlayerHealth : MonoBehaviour
    {
        [SerializeField] 
        private int _maxHealth = 5;
        [SerializedField]
        private int _currentHealth;
        
        public int Health => health;
        
        private void IncreaseHealth(int hp)
        {
            health = Mathf.Clamp(++health, 0, _maxHealth);
        }
        
        private void DecreaseHealth(int hp)
        {
            health = Mathf.Clamp(--health, 0, _maxHealth);
        }
    }
}