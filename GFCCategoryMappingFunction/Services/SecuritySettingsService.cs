using GFCCategoryMappingFunction.Models;

namespace GFCCategoryMappingFunction.Services
{
    public interface ISecuritySettingsService
    {
        string GetPublicKey();
    }

    public class SecuritySettingsService : ISecuritySettingsService
    {
        public string _publicKey;

        public SecuritySettingsService(ISecuritySettings securitySettings)
        {
            _publicKey = securitySettings.PublicKey;
        }
        
        public string GetPublicKey()
        {
            return _publicKey;
        }
    }
}
