using Encoding = System.Text.Encoding;
namespace Agarme_Server.Misc;
public unsafe static class BufferReader
{
    public static T Read<T>(byte* ptr, ref int offset) where T : unmanaged
    {
        T value = *(T*)(ptr + offset);
        offset += sizeof(T);
        return value;
    }

    public static string ReadStr(byte[] data, int len, ref int offset)
    {
        var value = Encoding.UTF8.GetString(data, offset, len);
        offset += len;
        return value;
    }

    public static byte ReadByte(byte* ptr, ref int offset) => *(ptr + offset++);
}
