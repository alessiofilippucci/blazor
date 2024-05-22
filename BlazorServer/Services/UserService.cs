using BlazorServer.Config;

namespace BlazorServer.Services
{
    public class UserService
    {
        private readonly RedirectRules _redirectRules;

        public UserService(RedirectRules redirectRules)
        {
            _redirectRules = redirectRules;
        }
    }
}
