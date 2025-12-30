using System;
using System.Diagnostics.CodeAnalysis;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using osFotoFix.ViewModels;

namespace osFotoFix;

/// <summary>
/// Given a view model, returns the corresponding view if possible.
/// </summary>
[RequiresUnreferencedCode(
    "Default implementation of ViewLocator involves reflection which may be trimmed away.",
    Url = "https://docs.avaloniaui.net/docs/concepts/view-locator")]
public class ViewLocator : IDataTemplate
{
    public Control? Build(object? param)
    {
        if (param is null)
            return null;
        
        string name = param.GetType().FullName ?? string.Empty;
        if (name.StartsWith("Design", StringComparison.Ordinal))
        {
            name = name.Substring("Design".Length);
        }
        if (name.EndsWith("ViewModel", StringComparison.Ordinal))
        {
            name = name.Replace("ViewModel", "View", StringComparison.Ordinal);
        }
        var type = Type.GetType(name);
        if (type != null)
        {
            return (Control)Activator.CreateInstance(type)!;
        }

        return new TextBlock { Text = "Not Found: " + name };
    }

    public bool Match(object? data)
    {
        return data is ViewModelBase;
    }
}
