using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Utility
{
    public static class TypePropertyDictionaryGenerator
    {
        public static IEnumerable<PropertyBag> GetPropertyDictionaryForTypeGeneration(
            IEnumerable<Type> types,
            JsGeneratorOptions generatorOptions,
            List<PropertyBag> propertyTypeCollection = null)
        {

            if (propertyTypeCollection == null)
            {
                propertyTypeCollection = new List<PropertyBag>();
            }
            
            foreach (Type type in types)
            {
                if (type.IsEnum)
                {
                    string[] getVals = type.GetEnumNames();
                    string typeName = type.Name;
                    foreach (string enumVal in getVals)
                    {
                        if (generatorOptions.TreatEnumsAsStrings)
                        {
                            propertyTypeCollection.Add(new PropertyBag(typeName, type, enumVal, typeof (string),
                                null, PropertyBag.TransformablePropertyTypeEnum.Primitive, true, enumVal));
                        }
                        else
                        {
                            object trueVal = Convert.ChangeType(Enum.Parse(type, enumVal), type.GetEnumUnderlyingType());
                            propertyTypeCollection.Add(new PropertyBag(typeName, type, enumVal, type.GetEnumUnderlyingType(),
                                null, PropertyBag.TransformablePropertyTypeEnum.Primitive, true, trueVal));
                        }
                    }
                }
                else
                {
                    PropertyInfo[] props = type.GetProperties();
                    string typeName = type.Name;
                    foreach (PropertyInfo prop in props)
                    {
                        if (!Helpers.ShouldGenerateMember(prop, generatorOptions)) continue;

                        string propertyName = Helpers.GetPropertyName(prop, generatorOptions);
                        Type propertyType = prop.PropertyType;

                        if (!Helpers.IsPrimitive(propertyType))
                        {
                            if (Helpers.IsCollectionType(propertyType))
                            {
                                List<PropertyBagTypeInfo> collectionInnerTypes = GetCollectionInnerTypes(propertyType);
                                bool isDictionaryType = Helpers.IsDictionaryType(propertyType);

                                propertyTypeCollection.Add(new PropertyBag(typeName, type, propertyName, propertyType,
                                    collectionInnerTypes, isDictionaryType
                                        ? PropertyBag.TransformablePropertyTypeEnum.DictionaryType
                                        : PropertyBag.TransformablePropertyTypeEnum.CollectionType, false, null));

                                //if primitive, no need to reflect type
                                if (collectionInnerTypes.All(p => p.IsPrimitiveType)) continue;

                                foreach (PropertyBagTypeInfo collectionInnerType in collectionInnerTypes.Where(p => !p.IsPrimitiveType))
                                {
                                    string innerTypeName = collectionInnerType.Type.Name;
                                    if (propertyTypeCollection.All(p => p.TypeName != innerTypeName))
                                    {
                                        GetPropertyDictionaryForTypeGeneration(new[] {collectionInnerType.Type},
                                                generatorOptions, propertyTypeCollection);
                                    }
                                }
                            }
                            else
                            {
                                propertyTypeCollection.Add(new PropertyBag(typeName, type, propertyName, propertyType,
                                    null, PropertyBag.TransformablePropertyTypeEnum.ReferenceType, false, null));

                                if (propertyTypeCollection.All(p => p.TypeName != propertyType.Name))
                                {
                                    GetPropertyDictionaryForTypeGeneration(new[] {propertyType},
                                            generatorOptions, propertyTypeCollection);
                                }
                            }
                        }
                        else
                        {
                            bool hasDefaultValue = Helpers.HasDefaultValue(prop, generatorOptions);
                            if (hasDefaultValue)
                            {
                                object val = Helpers.ReadDefaultValueFromAttribute(prop);
                                propertyTypeCollection.Add(new PropertyBag(typeName, type, propertyName, propertyType,
                                    null, PropertyBag.TransformablePropertyTypeEnum.Primitive, true, val));
                            }
                            else
                            {
                                propertyTypeCollection.Add(new PropertyBag(typeName, type, propertyName, propertyType,
                                    null, PropertyBag.TransformablePropertyTypeEnum.Primitive, false, null));
                            }

                            if (propertyType.IsEnum)
                            {
                                if (propertyTypeCollection.All(p => p.TypeName != propertyType.Name))
                                {
                                    GetPropertyDictionaryForTypeGeneration(new[] {propertyType},
                                            generatorOptions, propertyTypeCollection);
                                }
                            }

                        }
                    }
                }
            }
            return propertyTypeCollection;
        }

        private static List<PropertyBagTypeInfo> GetCollectionInnerTypes(Type propertyType)
        {
            if (propertyType.IsArray)
            {
                return new List<PropertyBagTypeInfo>()
                {
                    new PropertyBagTypeInfo()
                    {
                        Type = propertyType.GetElementType(),
                        IsPrimitiveType = Helpers.IsPrimitive(propertyType.GetElementType()),
                        IsEnumType = propertyType.IsEnum
                    }
                };
            }

            if (Helpers.IsDictionaryType(propertyType))
            {
                return new List<PropertyBagTypeInfo>()
                {
                    new PropertyBagTypeInfo()
                    {
                        Type = propertyType.GetGenericArguments()[0],
                        IsPrimitiveType = Helpers.IsPrimitive(propertyType.GetGenericArguments()[0]),
                        IsDictionaryKey = true,
                        IsEnumType = propertyType.GetGenericArguments()[0].IsEnum
                    },
                    new PropertyBagTypeInfo()
                    {
                        Type = propertyType.GetGenericArguments()[1],
                        IsPrimitiveType = Helpers.IsPrimitive(propertyType.GetGenericArguments()[1]),
                        IsEnumType = propertyType.GetGenericArguments()[1].IsEnum
                    }
                };
            }

            return new List<PropertyBagTypeInfo>()
            {
                new PropertyBagTypeInfo()
                {
                    Type =
                        propertyType.GetGenericArguments().Any()
                            ? propertyType.GetGenericArguments()[0]
                            : typeof (string),
                    IsPrimitiveType = Helpers.IsPrimitive(propertyType.GetGenericArguments().Any()
                        ? propertyType.GetGenericArguments()[0]
                        : typeof (string)),
                    IsEnumType = (propertyType.GetGenericArguments().Any()
                        ? propertyType.GetGenericArguments()[0]
                        : typeof (string)).IsEnum
                }
            };
        }
    }
}
