namespace System
{
    public sealed class Type<T>
    {
        private Type()
        {
        }

        public static Type<T> Instance { get; } = new Type<T>();
    }
}
