using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Configuration;
namespace Application.Encryption
{
    public class SaltAndPepperEncryption : IPasswordEncryption
    {
        private readonly string _pepperLetters;
        private readonly int _pepperLength;

        /// <summary>
        /// Initializes a new instance of the <see cref="SaltAndPepperEncryption"/> class.
        /// This constructor initializes the encryption configuration with specified pepper value,
        /// along with the respective length. It validates the inputs to ensure the pepper string
        /// is not null and that the length is non-negative.
        /// </summary>
        /// <param name="pepperLetters">The pepper letters used for encryption. Cannot be null.</param>
        /// <param name="pepperLength">The length of the pepper letters. Must be non-negative.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="pepperLetters"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown when <paramref name="pepperLength"/> is less than 0.
        /// </exception>
        public SaltAndPepperEncryption(string pepperLetters, int pepperLength)
        {
            _pepperLetters = pepperLetters;
            _pepperLength = pepperLength;
        }

        public (string encryptedPassword, string encryptionKey) EncryptionPassword(string password)
        {
            byte[] generatedSalt = GenerateSalt();
            string generatedPepper = GeneratePepper();
            var encryptedPassword = HashPassword(password + generatedPepper, generatedSalt);
            return (encryptedPassword, Convert.ToBase64String(generatedSalt));
        }

        public bool CheckPasswordValid(string passwordToCheck, string encryptedPassword, string passwordKey)
        {
            byte[] salt = Convert.FromBase64String(passwordKey);
            for (int i = 0; i < Math.Pow(_pepperLetters.Length, _pepperLength); i++)
            {
                int[] currentPepper = ConvertToPepperArray(i);
                var pepper = ConvertIndexArrayToPepperWord(currentPepper);
                var currentEncryptedPassword = HashPassword(passwordToCheck + pepper, salt);
                if (encryptedPassword == currentEncryptedPassword)
                {
                    return true;
                }

            }
            return false;
        }
        private static string HashPassword(string pepperedPassword, byte[] salt)
        {
            // derive a 256-bit subkey (use HMACSHA256 with 100,000 iterations)
            string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: pepperedPassword!,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));
            return hashedPassword;
        }

        private string GeneratePepper()
        {
            string result = "";

            for (int i = 0; i < _pepperLength; i++)
            {
                int randomIndex = RandomNumberGenerator.GetInt32(_pepperLetters.Length - 1); // Pick a random index from the string
                result += _pepperLetters[randomIndex]; // Add the random character to the result array
            }
            return result;
        }
        private static byte[] GenerateSalt()
        {
            // Generate a 128-bit salt using a sequence of
            // cryptographically strong random bytes.
            return RandomNumberGenerator.GetBytes(128 / 8); // divide by 8 to convert bits to bytes
        }

        private string ConvertIndexArrayToPepperWord(int[] indexArray)
        {
            string word = "";
            foreach (int i in indexArray)
            {
                word += _pepperLetters[i];
            }
            return word;
        }

        private int[] ConvertToPepperArray(int value)
        {
            int[] array = new int[_pepperLength];
            int max = _pepperLetters.Length;
            int index = 0;
            while (value > 0 && index < _pepperLength)
            {
                array[index] += value; // Add the value to the current cell
                if (array[index] >= max) // Check if it overflows
                {
                    value = array[index] / max; // Carry over to the next cell
                    array[index] %= max;        // Keep only the remainder in the current cell
                }
                else
                {
                    value = 0; // If no overflow, we're done
                }

                index++; // Move to the next cell
            }
            return array;
        }
    }
}