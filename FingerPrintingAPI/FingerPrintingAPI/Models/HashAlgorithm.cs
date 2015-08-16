using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerPrintingAPI.Models
{
    public enum HashAlgorithm
    {
        /// <summary>
        ///   Locality Sensitive Hashing + Min Hash
        /// </summary>
        LSH = 0,

        /// <summary>
        ///   Neural Hasher
        /// </summary>
        NeuralHasher = 1,

        /// <summary>
        ///   No Hash
        /// </summary>
        None = 2
    }
}
