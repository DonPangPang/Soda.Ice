using Masa.Blazor;
using Soda.Ice.Shared;
using Soda.Ice.Shared.ViewModels;

namespace Soda.Ice.Client.Services
{
    public class AuthorizationApiService : ApiServiceBase
    {
        private readonly IPopupService _popupService;
        private readonly IIceHttpClient _iceHttpClient;

        public AuthorizationApiService(IPopupService popupService, IIceHttpClient iceHttpClient) : base(popupService, "Authorization")
        {
            _popupService = popupService;
            _iceHttpClient = iceHttpClient;
        }

        public async Task Login(VLogin login)
        {
            var res = await _iceHttpClient.Create().Url($"/api/{ControllerName}/login")
                .Body(login)
                .PostAsync<IceResponse>();

            await MessageHandler(res);
        }

        public async Task Register(VRegisterUser vRegisterUser)
        {
            var res = await _iceHttpClient.Create().Url($"/api/{ControllerName}/register")
                .Body(vRegisterUser)
                .PostAsync<IceResponse>();

            await MessageHandler(res);
        }

        public async Task ChangePassword(VChangePassword vChangePassword)
        {
            var res = await _iceHttpClient.Create().Url($"/api/{ControllerName}/change")
                .Body(vChangePassword)
                .PostAsync<IceResponse>();

            await MessageHandler(res);
        }
    }
}