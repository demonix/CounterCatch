﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CounterCatch.Configurations
{
    public class CounterElement : ConfigurationElement
    {
        public CounterElement()
        {
        }

        [ConfigurationProperty("id", IsRequired = true, IsKey=true)]
        public string Id
        {
            get
            {
                return (string)this["id"];
            }
            set
            {
                this["id"] = value;
            }
        }

        [ConfigurationProperty("hostGroup", IsRequired = true)]
        public string HostGroup
        {
            get
            {
                return (string)this["hostGroup"];
            }
            set
            {
                this["hostGroup"] = value;
            }
        }

        [ConfigurationProperty("category", IsRequired = true)]
        public string Category
        {
            get
            {
                return (string)this["category"];
            }
            set
            {
                this["category"] = value;
            }
        }

        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get
            {
                return (string)this["name"];
            }
            set
            {
                this["name"] = value;
            }
        }

        [ConfigurationProperty("instance", IsRequired = false, DefaultValue="")]
        public string Instance
        {
            get
            {
                return (string)this["instance"];
            }
            set
            {
                this["instance"] = value;
            }
        }
    }
}