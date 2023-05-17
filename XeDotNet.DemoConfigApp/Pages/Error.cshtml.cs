using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.FeatureManagement;

namespace XeDotNet.DemoConfigApp.Pages;

[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
[IgnoreAntiforgeryToken]
public class ErrorModel : PageModel
{
    public string? RequestId { get; set; }

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

    private readonly ILogger<ErrorModel> _logger;
    private IFeatureManager _featureManager;
    public ErrorModel(ILogger<ErrorModel> logger, IFeatureManager featureManager)
    {
        _logger = logger;
        _featureManager = featureManager;
    }

    public async Task OnGet()
    {
        if (await _featureManager.IsEnabledAsync("EnableRequestId"))
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
    }
}

