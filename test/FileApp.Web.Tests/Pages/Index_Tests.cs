using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace FileApp.Pages
{
    public class Index_Tests : FileAppWebTestBase
    {
        [Fact]
        public async Task Welcome_Page()
        {
            var response = await GetResponseAsStringAsync("/");
            response.ShouldNotBeNull();
        }
    }
}
