using System;
using System.Collections.Generic;
using System.Text;

namespace GFCCategoryMappingFunction.Models
{
    public interface ISecuritySettings
    {
        string PublicKey { get; set; }
    }

    public class SecuritySettings : ISecuritySettings
{
        public string PublicKey { get; set; }
    }
}
