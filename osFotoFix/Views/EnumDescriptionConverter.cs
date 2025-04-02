using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Avalonia;
using Avalonia.Data.Converters;
using MetadataExtractor;

public class EnumDescriptionConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value.GetType().IsEnum)
        {
            return ((Enum)value).ToDescription();
        }
        throw new ArgumentException("Convert:Value must be an enum.");
    }
    
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if(value is EnumDescription enumDescription)
        {
            return enumDescription.Value;
        }
        throw new ArgumentException("ConvertBack:EnumDescription must be an enum.");
    }
}

public record EnumDescription
{
    public object Value { get; set; }

    public string Description { get; set; }
    
    public string Help { get; set; }
    
    public override string ToString()
    {
        return Description;
    }
}

public static class EnumUtils
{
    
    public static IEnumerable<EnumDescription> ToDescriptions(Type t)
    {
        if (!t.IsEnum)
            throw new ArgumentException($"{nameof(t)} must be an enum type");
    
        return Enum.GetValues(t).Cast<Enum>().Select(ToDescription).ToList();
    }
    
    public static EnumDescription ToDescription(this Enum value)
    {
        string description;
        string help = null;
        
        var attributes = value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
        if (attributes.Any())
        {
            description = (attributes.First() as DescriptionAttribute)?.Description;
        }
        else
        {
            TextInfo ti = CultureInfo.CurrentCulture.TextInfo;
            description = ti.ToTitleCase(ti.ToLower(value.ToString().Replace("_", " ")));
        }
        
        if(description.IndexOf(';') is var index && index != -1)
        {
            help = description.Substring(index + 1);
            description = description.Substring(0, index);
        }

        return new EnumDescription() { Value = value, Description = description, Help = help };
    }

    public static string GetStringValue(Enum value)
    {
        string output = null;
        Type type = value.GetType();
        FieldInfo fi = type.GetField(value.ToString());
        // if (fi.GetCustomAttributes(typeof(StringValue),
        //         false) is StringValue[] { Length: > 0 } attrs)
        // if (fi.GetCustomAttribute(typeof(StringValue), false)
        //     .OfType<StringValue>()
        //     .ToArray() is { Length: > 0 } atts)
        // )
        if (fi.GetCustomAttributes(typeof(EnumDescription), false) is object[] objArray 
            && objArray.OfType<EnumDescription>().ToArray() is { Length: > 0 } attrs)
        {
            output = attrs[0].ToString();
        }
        return output;
    }

    public static T? Parse<T>(string input) where T : struct
    {
        if (!typeof(T).IsEnum)
        {
            throw new ArgumentException("Generic Type 'T' must be an Enum.");
        }
        if (string.IsNullOrEmpty(input)) return null;
        if (Enum.GetNames(typeof(T)).Any(
                e => e.Trim().ToUpperInvariant() == input.Trim().ToUpperInvariant()))
        {
            return (T)Enum.Parse(typeof(T), input, true);
        }
        return null;
    }
}





public class EnumBindingSource : AvaloniaObject /*: MarkupExtension*/
{
    private Type _enumType;
    public Type EnumType
    {
        get { return this._enumType; }
        set
        {
            if (value != this._enumType)
            {
                if (null != value)
                {
                    Type enumType = Nullable.GetUnderlyingType(value) ?? value;

                    if (!enumType.IsEnum)
                        throw new ArgumentException("Type must be for an Enum.");
                }

                this._enumType = value;
            }
        }
    }

    public EnumBindingSource() { }

    public EnumBindingSource(Type enumType)
    {
        this.EnumType = enumType;
    }

    public Array ProvideValue(IServiceProvider serviceProvider)
    {
        if (null == this._enumType)
            throw new InvalidOperationException("The EnumType must be specified.");

        Type actualEnumType = Nullable.GetUnderlyingType(this._enumType) ?? this._enumType;
        Array enumValues = Enum.GetValues(actualEnumType);

        if (actualEnumType == this._enumType)
            return enumValues;

        Array tempArray = Array.CreateInstance(actualEnumType, enumValues.Length + 1);
        enumValues.CopyTo(tempArray, 1);
        return tempArray;
    }
}