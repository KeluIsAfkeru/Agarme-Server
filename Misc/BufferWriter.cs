#pragma warning disable 8981,IDE0054
using Encoding = System.Text.Encoding;

namespace Agarme_Server.Misc;
public unsafe static class BufferWriter
{
    [Obsolete("这个函数好像有点错误")]
    public static void WriteString(string str, byte[] buffer, int offset = 0)
    {
        byte[] string_bytes = Encoding.UTF8.GetBytes(str);
        ushort length = (ushort)string_bytes.Length;
        ushort length_bytes = (ushort)(sizeof(ushort) + length);
        if (length > 0)
        {
            fixed (byte* ptr = &string_bytes[0])
            {
                for (int k = 0; k < string_bytes.Length; k++)
                {
                    buffer[offset + k] = *(ptr + k);
                }
            }
        }
    }

    public static void WriteUshort(ushort val, byte[] buffer, int offset = 0)
    {
        buffer[offset] = (byte)val;
        buffer[offset] = (byte)(val >> 8);
    }

    public static void WriteFloat(float val, byte[] buffer, int offset = 0)
    {
        fixed (byte* ptr = BitConverter.GetBytes(val))
        {
            buffer[offset] = *(ptr + 0);
            buffer[offset + 1] = *(ptr + 1);
            buffer[offset + 2] = *(ptr + 2);
            buffer[offset + 3] = *(ptr + 3);
        }
    }

    public static void WriteDouble(double val, byte[] buffer, int offset = 0)
    {
        fixed (byte* ptr = BitConverter.GetBytes(val))
        {
            buffer[offset + 0] = *(ptr + 0);
            buffer[offset + 1] = *(ptr + 1);
            buffer[offset + 2] = *(ptr + 2);
            buffer[offset + 3] = *(ptr + 3);
            buffer[offset + 4] = *(ptr + 4);
            buffer[offset + 5] = *(ptr + 5);
            buffer[offset + 6] = *(ptr + 6);
            buffer[offset + 7] = *(ptr + 7);
        }
    }

    public static void WriteUnmanaged<T>(T val, byte[] buffer, int offset = 0) where T : unmanaged
    {
        fixed (byte* ptr = buffer)
            *(T*)(ptr + offset) = val;
    }

    public static void WriteBytesToBytes(byte[] des, byte[] buffer, int offset = 0)
    {
        Buffer.BlockCopy(buffer, 0, des, offset, buffer.Length);
        //fixed (byte* ptr = &buffer[0])
        //{
        //    for (int k = 0; k < buffer.Length; k++)
        //    {
        //        des[offset + k] = *(ptr + k);
        //    }
        //}
    }

    public static void WriteArrayToUnint<T>(unint des, T[] buffer, int offset = 0)
        where T : unmanaged
    {
        fixed (T* ptr = &buffer[0])
        {
            for (int j = 0; j < buffer.Length; j++)
            {
                *((T*)des.value + j) = *(ptr + j);
            }
        }
    }

    public static T[] MergeTwoArray<T>(T[] buffer1, T[] buffer2)
    {
        var len1 = buffer1.Length;
        var len2 = buffer2.Length;
        var data = new T[len1 + len2];
        buffer1.CopyTo(data, 0);
        buffer2.CopyTo(data, len1);
        return data;
    }

    public static byte[] MergeWith(this byte[] data1, byte[] data2)
    {
        var MergedBytes = new byte[data1.Length + data2.Length];
        Buffer.BlockCopy(data1, 0, MergedBytes, 0, data1.Length);
        Buffer.BlockCopy(data2, 0, MergedBytes, data1.Length * sizeof(byte), data2.Length);
        return MergedBytes;
    }

    public static byte[] WritePackage(byte header, byte[] src)
    {
        var bytes = new byte[src.Length + 1];
        bytes[0] = header;
        WriteBytesToBytes(bytes, src, 1);
        return bytes;
    }

    public static byte[] WriteString(string str)
    {
        byte[] string_bytes = Encoding.UTF8.GetBytes(str);
        ushort length = (ushort)string_bytes.Length;
        ushort length_bytes = (ushort)(sizeof(ushort) + length);
        byte[] des_bytes = new byte[length_bytes];
        des_bytes[0] = (byte)length;
        des_bytes[1] = (byte)(length >> 8);
        if (length > 0)
        {
            fixed (byte* ptr = &string_bytes[0])
            {
                for (int k = 0; k < string_bytes.Length; k++)
                {
                    des_bytes[k + 2] = *(ptr + k);
                }
            }
        }
        return des_bytes;
    }

    public static byte[] WritePackage<T>(byte header, byte[] src, T len)
        where T : unmanaged
    {
        //如果想在src前写入长度数据，可以用这个重载
        var length = sizeof(T);
        var bytes = new byte[src.Length + 1 + length];
        bytes[0] = header;
        WriteUnmanaged(len, bytes, 1);
        WriteBytesToBytes(bytes, src, 1 + length);
        return bytes;
    }
}
