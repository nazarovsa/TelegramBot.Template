using System.Text.Json;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ProjectName.Persistence.Infrastructure;

public static class PropertyBuilderExtensions
{
    public static PropertyBuilder<TProperty> HasJsonConversion<TProperty>(
        this PropertyBuilder<TProperty> propertyBuilder) where TProperty : class
    {
        var serializationType = GetSerializationTypeByConvention<TProperty>();
        return HasJsonConversionWithSerializationType(propertyBuilder, serializationType);
    }

    public static PropertyBuilder<TProperty> HasJsonConversionWithSerializationType<TProperty>(
        this PropertyBuilder<TProperty> propertyBuilder, Type serializationType) where TProperty : class
    {
        if (!typeof(TProperty).IsAssignableFrom(serializationType))
            throw new InvalidOperationException(
                $"Must be able to assign {nameof(serializationType)} '{serializationType.FullName}' to <{nameof(TProperty)}> '{typeof(TProperty).FullName}'");

        var converter = MakeConverter<TProperty>(serializationType);
        var comparer = MakeComparer<TProperty>(serializationType);

        propertyBuilder.HasConversion(converter);
        propertyBuilder.Metadata.SetValueConverter(converter);
        propertyBuilder.Metadata.SetValueComparer(comparer);

        return propertyBuilder;
    }

    private static Type GetSerializationTypeByConvention<TProperty>() where TProperty : class
    {
        var typeIsReadOnlyDictionary = typeof(TProperty).IsGenericType &&
                                       typeof(TProperty).GetGenericTypeDefinition() == typeof(IReadOnlyDictionary<,>);
        if (typeIsReadOnlyDictionary)
            return typeof(Dictionary<,>).MakeGenericType(typeof(TProperty).GetGenericArguments());

        var typeIsReadOnlyList = typeof(TProperty).IsGenericType &&
                                 typeof(TProperty).GetGenericTypeDefinition() == typeof(IReadOnlyList<>);
        if (typeIsReadOnlyList)
            return typeof(List<>).MakeGenericType(typeof(TProperty).GetGenericArguments());

        return typeof(TProperty);
    }

    private static Microsoft.EntityFrameworkCore.ChangeTracking.ValueComparer<TProperty> MakeComparer<TProperty>(
        Type serializationType)
        => new(
            (l, r) => Serialize(l) == Serialize(r),
            v => v == null ? 0 : Serialize(v).GetHashCode(),
            v => Deserialize<TProperty>(Serialize(v), serializationType));

    private static ValueConverter<TProperty, string> MakeConverter<TProperty>(Type serializationType)
        => new(
            v => Serialize(v),
            v => Deserialize<TProperty>(v, serializationType));


    private static TProperty Deserialize<TProperty>(string v, Type serializationType)
    {
        if (v == null)
            return default;

        return (TProperty)JsonSerializer.Deserialize(v, serializationType, JsonSerializerOptions.Default);
    }

    private static string Serialize<TProperty>(TProperty v)
        => JsonSerializer.Serialize(v, JsonSerializerOptions.Default);
}