﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MarketingBox.AffiliateApi.Swagger
{
    internal class XmsEnumOperationFilter : IOperationFilter
    {
        private readonly XmsEnumExtensionsOptions _options;

        public XmsEnumOperationFilter(XmsEnumExtensionsOptions options)
        {
            _options = options;
        }

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
            {
                return;
            }
            
            foreach (var parameter in GetEnumParameters(context))
            {
                var operationParameter = operation.Parameters.Single(p => p.Name == parameter.Name);

                XmsEnumExtensionApplicator.Apply(operationParameter.Extensions, parameter.Type, _options);
            }
        }

        private static IEnumerable<(Type Type, string Name)> GetEnumParameters(OperationFilterContext context)
        {
            return context
                .ApiDescription
                .ParameterDescriptions
                .Where(x => x.Type != null)
                .Select(x => (Type: TryGetEnumType(x), Name: x.Name))
                .Where(x => x.Type != null);
        }

        private static Type TryGetEnumType(ApiParameterDescription parameter)
        {
            if (parameter.Type.GetTypeInfo().IsEnum)
            {
                return parameter.Type;
            }

            var nullableUnderlyingType = Nullable.GetUnderlyingType(parameter.Type);

            return nullableUnderlyingType?.GetTypeInfo().IsEnum == true
                ? nullableUnderlyingType 
                : null;
        }
    }
}