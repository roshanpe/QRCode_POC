using System;
using System.IO;
using Xunit;
using QR;
using System.Runtime.Versioning;

namespace QRCode_POC.Tests;

public class QRCodeServiceTests
{
    private readonly QRCodeService _qrCodeService;

    public QRCodeServiceTests()
    {
        _qrCodeService = new QRCodeService();
    }

    [SupportedOSPlatform("windows")]
    [Fact]
    public void GenerateQRCode_WithValidInput_ReturnsFilePath()
    {
        // Arrange
        string name = "John Doe";
        string email = "john.doe@example.com";
        DateTime dateOfBirth = new DateTime(1990, 5, 15);

        // Act
        string filePath = _qrCodeService.GenerateQRCode(name, email, dateOfBirth);

        // Assert
        Assert.NotNull(filePath);
        Assert.NotEmpty(filePath);
        Assert.Contains("QRCode_", filePath);
        Assert.EndsWith(".png", filePath);

        // Cleanup
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }

    [SupportedOSPlatform("windows")]
    [Fact]
    public void GenerateQRCode_CreatesPhysicalFile()
    {
        // Arrange
        string name = "Jane Smith";
        string email = "jane.smith@example.com";
        DateTime dateOfBirth = new DateTime(1985, 3, 20);

        // Act
        string filePath = _qrCodeService.GenerateQRCode(name, email, dateOfBirth);

        // Assert
        Assert.True(File.Exists(filePath), $"File {filePath} was not created");
        Assert.True(new FileInfo(filePath).Length > 0, "Generated file is empty");

        // Cleanup
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }

    [Theory]
    [InlineData("John", "john@test.com", "1990-01-01")]
    [InlineData("Jane", "jane@test.com", "1985-12-25")]
    [InlineData("Bob", "bob@test.com", "2000-06-15")]
    [SupportedOSPlatform("windows")]
    public void GenerateQRCode_WithMultipleInputs_ReturnsValidPaths(string name, string email, string dobString)
    {
        // Arrange
        DateTime dob = DateTime.Parse(dobString);

        // Act
        string filePath = _qrCodeService.GenerateQRCode(name, email, dob);

        // Assert
        Assert.NotNull(filePath);
        Assert.NotEmpty(filePath);
        Assert.Contains(filePath,"QRCode_");

        // Cleanup
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }
}
