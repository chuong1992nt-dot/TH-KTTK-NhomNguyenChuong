using ASC.Web.Configuration;
using ASC.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Xunit;
using Moq;

public class HomeControllerTests
{
    [Fact]
    public void Index_Returns_ViewResult()
    {
        // Arrange
        var mockLogger = new Mock<ILogger<HomeController>>();

        var mockOptions = Options.Create(new ApplicationSettings
        {
            ApplicationTitle = "Test App"
        });

        var controller = new HomeController(
            mockLogger.Object,
            mockOptions
        );

        // Act
        var result = controller.Index();

        // Assert
        Assert.NotNull(result);
        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public void Index_Sets_ViewBag_Title()
    {
        var mockLogger = new Mock<ILogger<HomeController>>();

        var mockOptions = Options.Create(new ApplicationSettings
        {
            ApplicationTitle = "My App"
        });

        var controller = new HomeController(
            mockLogger.Object,
            mockOptions
        );

        var result = controller.Index();

        Assert.Equal("My App", controller.ViewBag.Title);
    }

    [Fact]
    public void Dashboard_Returns_ViewResult()
    {
        var mockLogger = new Mock<ILogger<HomeController>>();
        var mockOptions = Options.Create(new ApplicationSettings());

        var controller = new HomeController(
            mockLogger.Object,
            mockOptions
        );

        var result = controller.Dashboard();

        Assert.NotNull(result);
        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public void Privacy_Returns_ViewResult()
    {
        var mockLogger = new Mock<ILogger<HomeController>>();
        var mockOptions = Options.Create(new ApplicationSettings());

        var controller = new HomeController(
            mockLogger.Object,
            mockOptions
        );

        var result = controller.Privacy();

        Assert.NotNull(result);
        Assert.IsType<ViewResult>(result);
    }
}

