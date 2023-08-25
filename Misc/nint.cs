using System.Runtime.CompilerServices;
#pragma warning disable 8981,IDE1006,IDE0090

namespace Agarme_Server.Misc
{
    /// <summary>
    /// 有符号NativeInt
    /// </summary>
    public unsafe struct nint : IComparable, IComparable<nint>, IEquatable<nint>
    {
        internal byte* value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public nint(int value)
        {
            this.value = (byte*)value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public nint(uint value)
        {
            this.value = (byte*)value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public nint(long value)
        {
            this.value = (byte*)value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public nint(ulong value)
        {
            this.value = (byte*)value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public nint(IntPtr value)
        {
            this.value = (byte*)value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public nint(UIntPtr value)
        {
            this.value = (byte*)value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public nint(void* value)
        {
            this.value = (byte*)value;
        }

        #region 转换为其他类型

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator int(nint a) => (int)a.value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator uint(nint a) => (uint)a.value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator long(nint a) => (long)a.value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator ulong(nint a) => (ulong)a.value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator IntPtr(nint a) => (IntPtr)a.value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator UIntPtr(nint a) => (UIntPtr)a.value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator void*(nint a) => a.value;

        #endregion 转换为其他类型

        #region 转换为此类型

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator nint(int a) => new nint(a);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator nint(uint a) => new nint(a);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator nint(long a) => new nint(a);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator nint(ulong a) => new nint(a);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator nint(IntPtr a) => new nint(a);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator nint(UIntPtr a) => new nint(a);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator nint(void* a) => new nint(a);

        #endregion 转换为此类型

        #region 运算

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static nint operator +(nint a, nint b) => new nint((long)a.value + (long)b.value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static nint operator -(nint a, nint b) => new nint((long)a.value - (long)b.value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static nint operator *(nint a, nint b) => new nint((long)a.value * (long)b.value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static nint operator /(nint a, nint b) => new nint((long)a.value / (long)b.value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static nint operator %(nint a, nint b) => new nint((long)a.value % (long)b.value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static nint operator <<(nint a, int b) => new nint((long)a.value << b);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static nint operator >>(nint a, int b) => new nint((long)a.value >> b);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <(nint a, nint b) => (long)a < (long)b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >(nint a, nint b) => (long)a > (long)b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <=(nint a, nint b) => (long)a <= (long)b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >=(nint a, nint b) => (long)a >= (long)b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(nint a, nint b) => a.value == b.value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(nint a, nint b) => a.value != b.value;

        #endregion 运算

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int CompareTo(object obj)
        {
            return CompareTo((nint)obj);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int CompareTo(nint other)
        {
            return this > other ? 1 : this == other ? 0 : -1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(nint other)
        {
            return value == other.value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj)
        {
            return obj is nint other && Equals(other);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
        {
            return unchecked((int)(long)value);
        }

        public override string ToString()
        {
            return ((long)value).ToString();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write<T>(T value) where T : unmanaged
        {
            *(T*)this.value = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Read<T>() where T : unmanaged
        {
            return *(T*)value;
        }
    }
}