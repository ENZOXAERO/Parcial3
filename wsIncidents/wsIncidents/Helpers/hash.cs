using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace wsIncidents.Helpers {
    internal static class hash {

        private static string rand { get => "371a81472367801a"; }

        private static byte[] iv = new byte[16];

        public static string encrypt(string key) {

            byte[] data;

            using(Aes aes = Aes.Create()) {

                aes.Key = Encoding.UTF8.GetBytes(rand);
                aes.IV = iv;
                ICryptoTransform cryptoTransform = aes.CreateEncryptor(aes.Key,aes.IV);

                using(MemoryStream stream = new MemoryStream()) {

                    using(CryptoStream crypto = new CryptoStream(stream,cryptoTransform,CryptoStreamMode.Write)) {

                        using(StreamWriter writer = new StreamWriter(crypto)) {
                            writer.Write(key);
                        }

                        data = stream.ToArray();
                    }

                }
            }

            return Convert.ToBase64String(data);
        }

        public static string decrypt(string key) {

            byte[] data = Convert.FromBase64String(key);

            using(Aes aes = Aes.Create()) {

                aes.Key = Encoding.UTF8.GetBytes(rand);
                aes.IV = iv;
                ICryptoTransform cryptoTransform = aes.CreateDecryptor(aes.Key,aes.IV);

                using(MemoryStream stream = new MemoryStream(data)) {

                    using(CryptoStream crypto = new CryptoStream(stream,cryptoTransform,CryptoStreamMode.Read)) {

                        using(StreamReader reader = new StreamReader(crypto)) {
                            return reader.ReadToEnd();
                        }
                    }

                }
            }

        }
    }
}