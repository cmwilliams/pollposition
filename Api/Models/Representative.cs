﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Models
{
    public class Representative
    {
        public string Office { get; set; }
        public List<Level> Levels { get; set; }
        public List<Official> Officials { get; set; }

        public Representative()
        {
            Levels = new List<Level>();
            Officials = new List<Official>();
        }
    }
}
