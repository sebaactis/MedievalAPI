﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedievalGame.Application.Features.Weapons.Dtos
{
    public class WeaponDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int AttackPower { get; set; }
        public string Type { get; set; }
    }
}
