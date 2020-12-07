namespace XeMart.Services
{
    using System.Threading.Tasks;

    public interface IViewRenderService
    {
        public Task<string> RenderToStringAsync(string viewName, object model);
    }
}
