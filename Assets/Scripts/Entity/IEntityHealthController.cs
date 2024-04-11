namespace BTG.Entity
{
    public interface IEntityHealthController
    {
        public event System.Action<int, int> OnHealthUpdated;        // int - CurrentHealth, int - MaxHealth

        public void TakeDamage(int damage);
    }
}