﻿using System;

using JackTheEnumRipper.Interfaces;
using JackTheEnumRipper.Models;

namespace Serializer
{
    public class PhpSerializer : ISerializer
    {
        public Format Format => Format.Php;

        public void Serialize()
        {
            Console.WriteLine(this.Format);
        }
    }
}