namespace Db.System
{
    public struct DbNullable<T>
    {
        public DbNullable(T value)
        {
            this.Value = value;
            this.HasValue = true;
        }

        public T Value { get; }

        public bool HasValue { get; }
    }
}
