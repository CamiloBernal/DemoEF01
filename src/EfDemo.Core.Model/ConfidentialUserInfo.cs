using System;
using EfDemo.Application.Services.CriptoModels;

namespace EfDemo.Core.Model
{
    public class ConfidentialUserInfo : IEncryptedEntity
    {
        [PropertyEncrypted]
        public string Name { get; set; }

        [PropertyEncrypted]
        public string LastName { get; set; }

        public int Age { get; set; }

        public string Email { get; set; }



        public string DecryptionPrivateKey { get; set; }

        
    }
}