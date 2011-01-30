#region License
//
// Author: Ryan Riley <ryan.riley@panesofglass.org>
// Copyright (c) 2011, Ryan Riley.
//
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt for details.
//
#endregion

using System;
using System.Collections.Generic;
using System.ServiceModel.Description;
using Microsoft.ServiceModel.Http;
using Nina.ViewEngines;

namespace Http.Formatters
{
    /// <summary>
    /// Processes text/html and application/xhtml+xml media types.
    /// </summary>
    /// <remarks>
    ///     This single processor can be configured to use different view engines.</para>
    ///     To use the Spark View Engine, for instance, you can add this to your Global.asax:
    ///     <code>Configure.Views.WithSpark();</code>.
    ///     If you would rather use Razor, you can just use: <code>Configure.Views.WithRazor();</code>
    ///     Other supported view engines include NDango and NHaml. More are likely on the way.
    ///     <see href="https://github.com/panesofglass/nina/tree/templating">See my templating branch of the Nina project.</see>
    /// </remarks>
    public class HtmlProcessor : MediaTypeProcessor
    {
        private static readonly IDictionary<string, ITemplate> _cache = new Dictionary<string, ITemplate>();

        public HtmlProcessor(HttpOperationDescription operation, MediaTypeProcessorMode mode)
            : base(operation, mode)
        {
        }

        public override IEnumerable<string> SupportedMediaTypes
        {
            get
            {
                yield return "text/html";
                yield return "application/xhtml+xml";
            }
        }

        public override object ReadFromStream(System.IO.Stream stream, System.Net.Http.HttpRequestMessage request)
        {
            throw new NotImplementedException();
        }

        public override void WriteToStream(object instance, System.IO.Stream stream, System.Net.Http.HttpRequestMessage request)
        {
            ITemplate template;
            string templateName = instance.GetType().Name;
            if (Nina.Configuration.Configure.IsDevelopment || !_cache.TryGetValue(templateName, out template))
            {
                template = Nina.Configuration.Configure.Views.Engine.Compile<object>(templateName);
                _cache[templateName] = template;
            }

            using (var sw = new System.IO.StreamWriter(stream))
                template.Render(sw, instance);
        }
    }
}
