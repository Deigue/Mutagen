﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mutagen
{
    public partial class GlobalShort
    {
        protected override char TriggerChar => TRIGGER_CHAR;
        public const char TRIGGER_CHAR = 's';

        partial void CustomCtor()
        {
            this.RawFloat_Property.Subscribe((change) => this.Data = (short)Math.Round(change.New));
            this.Data_Property.Subscribe((change) => this.RawFloat = change.New, fireInitial: false);
        }
    }
}
