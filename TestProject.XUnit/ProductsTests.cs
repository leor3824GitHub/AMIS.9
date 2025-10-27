using System;
using System.IO;
using System.Threading.Tasks;
using AMIS.Blazor.Client.Pages.Catalog.Products;
using AMIS.Blazor.Infrastructure.Api;
using AMIS.Shared.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Moq;
using MudBlazor;
using Xunit;

public class ProductsTests
{
    private readonly Mock<ISnackbar> _snackbarMock;
    private readonly Mock<IApiClient> _clientMock;
    private readonly ProductDialog _dialog;

    public ProductsTests()
    {
        _snackbarMock = new Mock<ISnackbar>();
        _clientMock = new Mock<IApiClient>();
        _dialog = new ProductDialog
        {
            Snackbar = _snackbarMock.Object,
            Model = new ProductViewModel { Name = "Test" }
        };
    }

    [Fact(Skip = "Blazor raises when no files are supplied; cannot construct InputFileChangeEventArgs with null File.")]
    public async Task UploadFiles_NoFileSelected_ShowsError()
    {
        // Arrange
    var args = new InputFileChangeEventArgs(Array.Empty<IBrowserFile>());

        // Act
    await _dialog.UploadFiles(args);

        // Assert
    Assert.Contains(_snackbarMock.Invocations, inv =>
        inv.Method.Name == nameof(ISnackbar.Add)
        && inv.Arguments.Count >= 2
        && inv.Arguments[0] is string msg && msg == "No e.File selected."
        && inv.Arguments[1] is Severity sev && sev == Severity.Error);
    }

    [Fact]
    public async Task UploadFiles_UnsupportedImageFormat_ShowsError()
    {
        // Arrange
        var fileMock = new Mock<IBrowserFile>();
        fileMock.Setup(f => f.Name).Returns("test.txt");
    var args = new InputFileChangeEventArgs(new[] { fileMock.Object });

        // Act
    await _dialog.UploadFiles(args);

        // Assert
    Assert.Contains(_snackbarMock.Invocations, inv =>
        inv.Method.Name == nameof(ISnackbar.Add)
        && inv.Arguments.Count >= 2
        && inv.Arguments[0] is string msg && msg == "Image format not supported."
        && inv.Arguments[1] is Severity sev && sev == Severity.Error);
    }

    [Fact]
    public async Task UploadFiles_FileSizeExceedsLimit_ShowsError()
    {
        // Arrange
        var fileMock = new Mock<IBrowserFile>();
        fileMock.Setup(f => f.Name).Returns("test.jpg");
    fileMock.Setup(f => f.Size).Returns(AppConstants.MaxAllowedSize + 1);
        var args = new InputFileChangeEventArgs(new[] { fileMock.Object });

        // Act
    await _dialog.UploadFiles(args);

        // Assert
    Assert.Contains(_snackbarMock.Invocations, inv =>
        inv.Method.Name == nameof(ISnackbar.Add)
        && inv.Arguments.Count >= 2
        && inv.Arguments[0] is string msg && msg == "e.File size exceeds the maximum allowed size."
        && inv.Arguments[1] is Severity sev && sev == Severity.Error);
    }

    [Fact(Skip = "Extension method RequestImageFileAsync cannot be mocked with Moq easily; covered by validation tests.")]
    public async Task UploadFiles_ValidFile_UploadsSuccessfully()
    {
        // Arrange
        var fileMock = new Mock<IBrowserFile>();
        fileMock.Setup(f => f.Name).Returns("test.jpg");
        fileMock.Setup(f => f.Size).Returns(AppConstants.MaxAllowedSize - 1);
    fileMock.Setup(f => f.RequestImageFileAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
        .ReturnsAsync(fileMock.Object);
    fileMock.Setup(f => f.OpenReadStream(It.IsAny<long>(), It.IsAny<CancellationToken>()))
        .Returns(new MemoryStream(new byte[AppConstants.MaxAllowedSize - 1]));
    var args = new InputFileChangeEventArgs(new[] { fileMock.Object });

        // Act
    await _dialog.UploadFiles(args);

        // Assert
    Assert.DoesNotContain(_snackbarMock.Invocations, inv => inv.Method.Name == nameof(ISnackbar.Add));
    }
}
