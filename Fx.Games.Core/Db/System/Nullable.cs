namespace Db.System
{
    internal struct Nullable<T>
    {
        public Nullable(T value)
        {
            this.Value = value;
            this.HasValue = true;
        }

        public T Value { get; }

        public bool HasValue { get; }
    }
}
