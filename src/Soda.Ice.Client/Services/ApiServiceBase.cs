using BlazorComponent;
using Masa.Blazor;
using Soda.Ice.Abstracts;
using Soda.Ice.Client.Extensions;
using Soda.Ice.Shared;
using Soda.Ice.Shared.Parameters;
using Soda.Ice.Shared.ViewModels;
using Soda.Ice.WebApi.Controllers;
using Soda.Ice.WebApi.Helpers;

namespace Soda.Ice.Client.Services
{
    public class ApiServiceBase
    {
        private readonly IPopupService _popupService;

        internal string ControllerName { get; }

        public ApiServiceBase(IPopupService popupService, string controllerName)
        {
            _popupService = popupService;
            ControllerName = controllerName;
        }

        public async Task MessageHandler<T>(IceResponse<T>? response)
        {
            if (response?.IsSuccess ?? false)
            {
                await _popupService.EnqueueSnackbarAsync(response.Message, BlazorComponent.AlertTypes.Success);
            }
            else
            {
                await _popupService.EnqueueSnackbarAsync(response?.Message ?? "访问失败", BlazorComponent.AlertTypes.Error);
            }
        }
    }

    public class ApiServiceBase<TViewModel, TParameters> : ApiServiceBase
        where TViewModel : IViewModel, new()
        where TParameters : class, IParameters
    {
        private readonly IIceHttpClient _iceHttpClient;

        public ApiServiceBase(IPopupService popupService, IIceHttpClient iceHttpClient, string controller) : base(popupService, controller)
        {
            _iceHttpClient = iceHttpClient;
        }

        public async Task<VPagedList<TViewModel>> GetList(TParameters parameters)
        {
            var res = await _iceHttpClient.Create().Url($"/api/{ControllerName}{parameters.GetQueryString()}")
                .GetAsync<IceResponse<IEnumerable<TViewModel>>>();

            await MessageHandler(res);

            return (VPagedList<TViewModel>)(res?.Data ?? Enumerable.Empty<TViewModel>());
        }

        public async Task<TViewModel> Get(Guid id)
        {
            var res = await _iceHttpClient.Create().Url($"/api/{ControllerName}?id={id}")
                .GetAsync<IceResponse<TViewModel>>();

            await MessageHandler(res);

            if (res is null) return new TViewModel();

            return res.Data ?? new TViewModel();
        }

        public async Task Update(TViewModel model)
        {
            var res = await _iceHttpClient.Create().Url($"/api/{ControllerName}")
                .Body(model)
                .PutAsync<IceResponse>();

            await MessageHandler(res);
        }

        public async Task Add(TViewModel model)
        {
            var res = await _iceHttpClient.Create().Url($"/api/{ControllerName}")
                .Body(model)
                .PostAsync<IceResponse>();

            await MessageHandler(res);
        }

        public async Task Delete(Guid id)
        {
            var res = await _iceHttpClient.Create().Url($"/api/{ControllerName}?id={id}")
                .DeleteAsync<IceResponse>();

            await MessageHandler(res);
        }
    }

    public class UserApiService : ApiServiceBase<VUser, UserParameters>
    {
        public UserApiService(IPopupService popupService, IIceHttpClient iceHttpClient) : base(popupService, iceHttpClient, "User")
        {
        }
    }

    public class BlogApiService : ApiServiceBase<VBlog, BlogParameters>
    {
        public BlogApiService(IPopupService popupService, IIceHttpClient iceHttpClient) : base(popupService, iceHttpClient, "Blog")
        {
        }
    }

    public class BlogGroupApiService : ApiServiceBase<VBlogGroup, BlogGroupParameters>
    {
        public BlogGroupApiService(IPopupService popupService, IIceHttpClient iceHttpClient) : base(popupService, iceHttpClient, "BlogGroup")
        {
        }
    }

    public class BlogTagApiService : ApiServiceBase<VBlogTag, BlogTagParameters>
    {
        public BlogTagApiService(IPopupService popupService, IIceHttpClient iceHttpClient) : base(popupService, iceHttpClient, "BlogTag")
        {
        }
    }

    public class BlogViewLogApiService : ApiServiceBase<VBlogViewLog, BlogViewLogParameters>
    {
        public BlogViewLogApiService(IPopupService popupService, IIceHttpClient iceHttpClient) : base(popupService, iceHttpClient, "BlogViewLog")
        {
        }
    }

    public class FileResourceApiService : ApiServiceBase<VFileResource, FileResourceParameters>
    {
        public FileResourceApiService(IPopupService popupService, IIceHttpClient iceHttpClient) : base(popupService, iceHttpClient, "FileResource")
        {
        }
    }

    public class HistoryLogApiService : ApiServiceBase<VHistoryLog, HistoryLogParameters>
    {
        public HistoryLogApiService(IPopupService popupService, IIceHttpClient iceHttpClient) : base(popupService, iceHttpClient, "HistoryLog")
        {
        }
    }

    public class LoginLogApiService : ApiServiceBase<VLoginLog, LoginLogParameters>
    {
        public LoginLogApiService(IPopupService popupService, IIceHttpClient iceHttpClient) : base(popupService, iceHttpClient, "LoginLog")
        {
        }
    }
}