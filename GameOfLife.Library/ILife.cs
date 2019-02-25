namespace GameOfLife
{
    public interface ILife
    {
        /// <summary>
        /// 生きているか
        /// </summary>
        bool IsAlive { get; set; }
    }
}
