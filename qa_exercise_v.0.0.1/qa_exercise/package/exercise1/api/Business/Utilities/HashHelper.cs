using Microsoft.EntityFrameworkCore.Storage.Json;
using System;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;


namespace StargateAPI.Business.Utilities
{
    public static class HashHelper
    {
        public static string ComputeHash(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // Convert the input string to a byte array and compute the hash.
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert the byte array to a string.
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public static string ComputeBase64(object rawData)
        {
            string jsonString = JsonConvert.SerializeObject(rawData);
            byte[] byteArray = Encoding.UTF8.GetBytes(jsonString);
            string base64String = Convert.ToBase64String(byteArray);
            return base64String;
        }

        public static string ComputeJSONString(string base64)
        {
            byte[] byteArray = Convert.FromBase64String(base64);
            string jsonString = Encoding.UTF8.GetString(byteArray);
            return jsonString;
        }
    }
}
