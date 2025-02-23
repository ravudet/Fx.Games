namespace System
{
    public static class Of
    {
        public static Type<T> Type<T>()
        {
            return System.Type<T>.Instance;
        }
    }
}
