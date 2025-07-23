using MicroservicesUser.BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;

namespace MicroservicesUser.BusinessLogic.Implementations
{
    public class ViewRenderService : IViewRenderService
    {
        private readonly IRazorViewEngine _viewEngine;
        private readonly ITempDataProvider _tempDataProvider;
        private readonly IServiceProvider _serviceProvider;

        public ViewRenderService(IRazorViewEngine viewEngine, ITempDataProvider tempDataProvider, IServiceProvider serviceProvider)
        {
            _viewEngine = viewEngine;
            _tempDataProvider = tempDataProvider;
            _serviceProvider = serviceProvider;
        }

        public async Task<string> RenderToStringAsync(string viewName, object model)
        {
            DefaultHttpContext httpContext = new()
            {
                RequestServices = _serviceProvider
            };
            ActionContext actionContext = new(httpContext, new RouteData(), new ActionDescriptor());

            ViewEngineResult viewResult = _viewEngine.FindView(actionContext, viewName, isMainPage: false);

            if (!viewResult.Success)
            {
                throw new InvalidOperationException($"Couldn't find view '{viewName}'");
            }

            ViewDataDictionary viewDictionary = new(new EmptyModelMetadataProvider(), new ModelStateDictionary())
            {
                Model = model
            };

            StringWriter stringWriter = new();
            ViewContext viewContext = new(
                actionContext,
                viewResult.View,
                viewDictionary,
                new TempDataDictionary(httpContext, _tempDataProvider),
                stringWriter,
                new HtmlHelperOptions());

            await viewResult.View.RenderAsync(viewContext);
            return stringWriter.ToString();
        }
    }


}
