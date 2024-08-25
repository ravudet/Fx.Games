
namespace Fx.Games.Game.Amazons.Extensions
{
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    static class ArrayExtensions
    {
        public static void Fill<T>(this T[,] array, T value)
        {
            ref byte data = ref MemoryMarshal.GetArrayDataReference(array);
            ref T first = ref Unsafe.As<byte, T>(ref data);
            var span = MemoryMarshal.CreateSpan(ref first, array.Length);
            span.Fill(value);
        }
    }
}

