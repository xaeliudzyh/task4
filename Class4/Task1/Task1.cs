using static Task1.Task1;

namespace Task1
{
    // Необходимо заменить на более подходящий тип (коллекцию), позволяющий
    // эффективно искать диапазон по заданному IP-адресу
    using IPRangesDatabase = List<IPRange>;

    public class Task1
    {
        /*
        * Объекты этого класса создаются из строки, но хранят внутри помимо строки
        * ещё и целочисленное значение соответствующего адреса. Например, для адреса
         * 127.0.0.1 должно храниться число 1 + 0 * 2^8 + 0 * 2^16 + 127 * 2^24 = 2130706433.
        */
        internal record IPv4Addr (string StrValue) : IComparable<IPv4Addr>
        {
            internal uint IntValue = Ipstr2Int(StrValue);

            private static uint Ipstr2Int(string StrValue)
            {
                uint uValue = 0, square = 1;
                for (int i = StrValue.Length - 1; i >= 0; i--)
                {
                    if (StrValue[i] != '.')
                    {
                        uint currentSum = 0;
                        uint tens = 1;
                        for (int j = i; j >= 0; j--)
                        {
                            if (StrValue[j] == '.') break;
                            uint stoU = Convert.ToUInt32(StrValue[j].ToString());
                            currentSum += stoU * tens;
                            tens *= 10;
                            i--;
                        }

                        uValue += currentSum * square;
                        square *= 256;
                    }
                }

                return uValue;
            }

            private static uint Ipstr2Int()
            {
                throw new NotImplementedException();
            }

            // Благодаря этому методу мы можем сравнивать два значения IPv4Addr
            public int CompareTo(IPv4Addr other)
            {
                return IntValue.CompareTo(other.IntValue);
            }

            public override string ToString()
            {
                return StrValue;
            }
        }

       /// <summary>
        /// сделал класс для хранения диапазонов
        /// </summary>
        /// <param name="IpFrom"></param>
        /// <param name="IpTo"></param>
        internal record class IPRange(IPv4Addr IpFrom, IPv4Addr IpTo)
        {
            public override string ToString()
            {
                return $"{IpFrom},{IpTo}";
            }
        }

        /// <summary>
        /// хранение полученных данных
        /// </summary>
        /// <param name="IpsFile"></param>
        /// <param name="IprsFiles"></param>
        internal record class IPLookupArgs(string IpsFile, List<string> IprsFiles);
        /// <summary>
        /// преобразование этих данных
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        internal static IPLookupArgs? ParseArgs(string[] args)
        {
            if (args.Length <= 1)
                return null;
            
            var ipsFile = args[0];
            List<string> iprsFiles = args[1..].ToList();

            return new IPLookupArgs(ipsFile, iprsFiles);
        }

        /// <summary>
        /// сделал список ip адресов
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        internal static List<string> LoadQuery(string filename) {
            return new List<string>(File.ReadAllLines(filename));
        }
        
        /// <summary>
        /// коллекция для хранения диапазонов
        /// </summary>
        /// <param name="filenames"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal static IPRangesDatabase LoadRanges(List<String> filenames) {
            var ipRangesDatabase = new IPRangesDatabase();

            foreach (var filename in filenames)
            {
                var ipRanges = new List<string>(File.ReadAllLines(filename));
                foreach (var strIpRange in ipRanges)
                {
                    var strIps = strIpRange.Split(',');
                    var ipRange = new IPRange(new IPv4Addr(strIps[0]), new IPv4Addr(strIps[1]));
                    ipRangesDatabase.Add(ipRange);
                }
            }

            return ipRangesDatabase;
        }

        
        /// <summary>
        /// смотрим есть ли искомый IP в данном диапазоне
        /// </summary>
        /// <param name="ranges"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        internal static IPRange? FindRange(IPRangesDatabase ranges, IPv4Addr query) {
            foreach (var range in ranges)
            {
                if (query.CompareTo(range.IpFrom) >= 0 && query.CompareTo(range.IpTo) <= 0)
                    return range;
            }

            return null;
        }
        
        public static void Main(string[] args)
        {
            var ipLookupArgs = ParseArgs(args);
            if (ipLookupArgs == null)
            {
                return;
            }
            var queries = LoadQuery(ipLookupArgs.IpsFile);
            var ranges = LoadRanges(ipLookupArgs.IprsFiles);
            var file = args[0];

            File.WriteAllText(file, string.Empty);

            foreach (var ip in queries)
            {
                var findRange = FindRange(ranges, new IPv4Addr(ip));
                var result = ShowTheResult(ip, findRange);

                File.AppendAllText(file, result + Environment.NewLine);
            }
        }
        
        private static string ShowTheResult(string ip, IPRange? findRange)
        {
            if (findRange == null)
                return $"{ip}: NO";
            else
                return $"{ip}: YES ({findRange})";
        }
    }
}