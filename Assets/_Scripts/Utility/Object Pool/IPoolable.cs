namespace SeraphUtil.ObjectPool
{
    public interface IPoolable
    {
        public void OnTakeFromPool();
        public void OnReturnToPool();
        
    }
}