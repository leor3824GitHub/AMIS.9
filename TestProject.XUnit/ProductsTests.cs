using System;
using System.IO;
using System.Threading.Tasks;
using AMIS.Blazor.Client.Pages.Catalog;
using AMIS.Blazor.Infrastructure.Api;
using Microsoft.AspNetCore.Components.Forms;
using Moq;
using MudBlazor;
using Xunit;

public class ProductsTests
{
    private readonly Mock<ISnackbar> _snackbarMock;
    private readonly Mock<IApiClient> _clientMock;
    private readonly Products _products;

    public ProductsTests()
    {
        _snackbarMock = new Mock<ISnackbar>();
        _clientMock = new Mock<IApiClient>();
        _products = new Products
        {
            Snackbar = _snackbarMock.Object,
            _client = _clientMock.Object
        };
    }

    [Fact]
    public async Task UploadFiles_NoFileSelected_ShowsError()
    {
        // Arrange
        var args = new InputFileChangeEventArgs(null);

        // Act
        await _products.UploadFiles(args);

        // Assert
        _snackbarMock.Verify(s => s.Add("No file selected.", Severity.Error), Times.Once);
    }

    [Fact]
    public async Task UploadFiles_UnsupportedImageFormat_ShowsError()
    {
        // Arrange
        var fileMock = new Mock<IBrowserFile>();
        fileMock.Setup(f => f.Name).Returns("test.txt");
        var args = new InputFileChangeEventArgs(fileMock.Object);

        // Act
        await _products.UploadFiles(args);

        // Assert
        _snackbarMock.Verify(s => s.Add("Image format not supported.", Severity.Error), Times.Once);
    }

    [Fact]
    public async Task UploadFiles_FileSizeExceedsLimit_ShowsError()
    {
        // Arrange
        var fileMock = new Mock<IBrowserFile>();
        fileMock.Setup(f => f.Name).Returns("test.jpg");
        fileMock.Setup(f => f.Size).Returns(AppConstants.MaxAllowedSize + 1);
        var args = new InputFileChangeEventArgs(fileMock.Object);

        // Act
        await _products.UploadFiles(args);

        // Assert
        _snackbarMock.Verify(s => s.Add("File size exceeds the maximum allowed size.", Severity.Error), Times.Once);
    }

    [Fact]
    public async Task UploadFiles_ValidFile_UploadsSuccessfully()
    {
        // Arrange
        var fileMock = new Mock<IBrowserFile>();
        fileMock.Setup(f => f.Name).Returns("test.jpg");
        fileMock.Setup(f => f.Size).Returns(AppConstants.MaxAllowedSize - 1);
        fileMock.Setup(f => f.RequestImageFileAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(fileMock.Object);
        fileMock.Setup(f => f.OpenReadStream(It.IsAny<long>())).Returns(new MemoryStream(new byte[AppConstants.MaxAllowedSize - 1]));
        var args = new InputFileChangeEventArgs(fileMock.Object);

        // Act
        await _products.UploadFiles(args);

        // Assert
        _snackbarMock.Verify(s => s.Add(It.IsAny<string>(), It.IsAny<Severity>()), Times.Never);
    }
}
