namespace Agarme_Server.Misc
{
    public static class HKRand
    {
        /* 提供通过静态类高效生成真随机序列的操作 */
        private static ulong seed = (ulong)DateTime.Now.Ticks; //获取1970到当前时间的毫秒数 

        /* 生成基础随机数 */
        public static byte Byte() => (byte)ULong();
        public static sbyte Sbyte() => (sbyte)ULong();
        public static ushort UShort() => (ushort)ULong();
        public static short Short() => (short)ULong();
        public static uint UInt() => (uint)ULong();
        public static int Int() => (int)ULong();
        public static ulong ULong()
        {
            seed += (seed * (seed >> 2) * (seed >> 5)) + (seed >> 3);
            return seed;
        }
        public static long Long() => (long)ULong();
        public static bool Bool() => (Byte() & 0x0001) == 0;
        public static float Float() => (float)Double();
        public static double Double() => (double)ULong() / (double)ulong.MaxValue;

        /* 生成具有最大值限制的随机数 */
        public static byte Byte(byte max) => (byte)(Byte() % max);
        public static sbyte Sbyte(sbyte max) => (sbyte)(Sbyte() % max);
        public static ushort UShort(ushort max) => (ushort)(UShort() % max);
        public static short Short(short max) => (short)(Short() % max);
        public static uint UInt(uint max) => UInt() % max;
        public static int Int(int max) => (int)((uint)Int() % (uint)max);
        public static ulong ULong(ulong max) => ULong() % max;
        public static long Long(long max) => Long() % max;
        public static float Float(float max) => (float)(Double() * max);
        public static double Double(double max) => Double() * max;

        /* 生成指定范围内的随机数 */
        public static byte Byte(byte min, byte max) => (byte)(min + Byte((byte)(max - min)));
        public static sbyte Sbyte(sbyte min, sbyte max) => (sbyte)(min + Sbyte((sbyte)(max - min)));
        public static ushort UShort(ushort min, ushort max) => (ushort)(min + UShort((ushort)(max - min)));
        public static short Short(short min, short max) => (short)(min + Short((short)(max - min)));
        public static uint UInt(uint min, uint max) => min + UInt(max - min);
        public static int Int(int min, int max) => min + Int(max - min);
        public static ulong ULong(ulong min, ulong max) => min + ULong(max - min);
        public static long Long(long min, long max) => min + Long(max - min);
        public static float Float(float min, float max) => (float)(min + Double(max - min));
        public static double Double(double min, double max) => min + Double(max - min);
    }
}
