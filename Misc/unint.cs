using System.Runtime.CompilerServices;
#pragma warning disable 8981

namespace Agarme_Server.Misc
{
    /// <summary>
    /// !符号NativeInt，用于指代指针、内存区域长度、或者作为普通整数
    /// </summary>
    public unsafe struct unint : IComparable, IComparable<unint>, IEquatable<unint>
    {
        public byte* value;

        #region 构造函数

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unint(int value)
        {
            this.value = (byte*)value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unint(uint value)
        {
            this.value = (byte*)value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unint(long value)
        {
            this.value = (byte*)value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unint(ulong value)
        {
            this.value = (byte*)value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unint(IntPtr value)
        {
            this.value = (byte*)value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unint(UIntPtr value)
        {
            this.value = (byte*)value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unint(void* value)
        {
            this.value = (byte*)value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unint(nint value)
        {
            this.value = (byte*)value;
        }

        #endregion 构造函数

        #region 转换为其他类型

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator int(unint a) => (int)a.value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator uint(unint a) => (uint)a.value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator long(unint a) => (long)a.value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator ulong(unint a) => (ulong)a.value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator IntPtr(unint a) => (IntPtr)a.value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator UIntPtr(unint a) => (UIntPtr)a.value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator void*(unint a) => a.value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator nint(unint a) => new nint(a.value);

        #endregion 转换为其他类型

        #region 转换为此类型

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator unint(int a) => new unint(a);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator unint(uint a) => new unint(a);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator unint(long a) => new unint(a);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator unint(ulong a) => new unint(a);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator unint(IntPtr a) => new unint(a);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator unint(UIntPtr a) => new unint(a);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator unint(void* a) => new unint(a);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator unint(nint a) => new unint(a);

        #endregion 转换为此类型

        #region 运算

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unint operator +(unint a, unint b) => new unint((ulong)a.value + (ulong)b.value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unint operator -(unint a, unint b) => new unint((ulong)a.value - (ulong)b.value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unint operator *(unint a, unint b) => new unint((ulong)a.value * (ulong)b.value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unint operator /(unint a, unint b) => new unint((ulong)a.value / (ulong)b.value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unint operator %(unint a, unint b) => new unint((ulong)a.value % (ulong)b.value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unint operator <<(unint a, int b) => new unint((ulong)a.value << b);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unint operator >>(unint a, int b) => new unint((ulong)a.value >> b);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <(unint a, unint b) => (ulong)a < (ulong)b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >(unint a, unint b) => (ulong)a > (ulong)b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <=(unint a, unint b) => (ulong)a <= (ulong)b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >=(unint a, unint b) => (ulong)a >= (ulong)b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(unint a, unint b) => a.value == b.value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(unint a, unint b) => a.value != b.value;

        #endregion 运算

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int CompareTo(object obj)
        {
            return CompareTo((unint)obj);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int CompareTo(unint other)
        {
            return this > other ? 1 : this == other ? 0 : -1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(unint other)
        {
            return value == other.value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj)
        {
            return obj is unint other && Equals(other);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
        {
            return unchecked((int)(ulong)value);
        }

        public override string ToString()
        {
            return ((ulong)value).ToString();
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

        public unint this[unint offset]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return this + offset;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Fill(unint len, byte value = 0)
        {
            var i = (ulong)len;
            var p = (byte*)this;
            while (i-- != 0)
            {
                *(p + i) = value;
            }
        }
    }
}