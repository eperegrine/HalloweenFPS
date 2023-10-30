namespace DefaultNamespace
{
    public class PlayerHealth : Health
    {
        public override void RunDeath()
        {
            GameManager.Instance.PlayerDeath();
        }
    }
}