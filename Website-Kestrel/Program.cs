using System;
using System.Security.Cryptography;
using System.Threading;
using BeagleBone;
using Encryption_Demo;
using Web;

namespace BBB
{
    class Program
    {
        static void Main(String[] args)
        {
            Console.WriteLine("Entering Main");

            EncryptionTest();

            var exitEvent = new ManualResetEvent(false);

            // This is used to handle Ctrl+C
            Console.CancelKeyPress += (sender, eventArgs) =>
            {
                eventArgs.Cancel = true;
                exitEvent.Set();
            };

            int[] pins = { 3, 4, 5, 6 };
            PinManager pm = new PinManager(pins, new PinReader(2), new DBHelper("logbook.db"));
            //PinManager pm = new PinManager(pins, new MockReader(), new DBHelper("logbook.db"));

            pm.Initialize();

            using var webWorker = new wsWorker();
            Thread webThread = new Thread(webWorker.DoWork);
            webThread.Start();
        }

        private static void EncryptionTest()
        {
            Console.WriteLine("Entering Encryption Test");
            // generate a key
            var key = new byte[32];
            RandomNumberGenerator.Fill(key);

            using var aes = new AesGcm(key);
            string plaintext = "Vincent is the coolest boss in the world. Please give me a raise";

            Console.WriteLine($"Plaintext: {plaintext}");

            (byte[] ciphertext, byte[] nonce, byte[] tag) values = AesGcmEncryption.EncryptWithNet(plaintext, key);

            Console.WriteLine($"Ciphertext: {Convert.ToHexString(values.ciphertext)}");
            Console.WriteLine($"Ciphertext64: {Convert.ToBase64String(values.ciphertext)}");
            Console.WriteLine($"Nonce: {Convert.ToHexString(values.nonce)}");
            Console.WriteLine($"Nonce64: {Convert.ToBase64String(values.nonce)}");
            Console.WriteLine($"Tag: {Convert.ToHexString(values.tag)}");
            Console.WriteLine($"Tag64: {Convert.ToBase64String(values.tag)}");
            //TODO: Base64 text encoding

            var decrypted = AesGcmEncryption.DecryptWithNet(values.ciphertext, values.nonce, values.tag, key);

            Console.WriteLine($"Decrypted: {decrypted}");

        }
    }
}