﻿using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WebApiClientCore.Attributes;
using Xunit;

namespace WebApiClientCore.Test.Attributes.ParameterAttributes
{
    public class JsonContentAttributeTest
    {
        [Fact]
        public async Task BeforeRequestAsyncTest()
        {
            var apiAction = new ApiActionDescriptor(typeof(IMyApi).GetMethod("PostAsync"));
            var context = new TestRequestContext(apiAction, new
            {
                name = "laojiu",
                birthDay = DateTime.Parse("2010-10-10")
            });

            context.HttpContext.RequestMessage.RequestUri = new Uri("http://www.webapi.com/");
            context.HttpContext.RequestMessage.Method = HttpMethod.Post;

            var attr = new JsonContentAttribute();
            await attr.OnRequestAsync(new ApiParameterContext(context, 0));

            var body = await context.HttpContext.RequestMessage.Content.ReadAsByteArrayAsync();

            var options = context.HttpContext.Options.JsonSerializeOptions;
            var target = context.HttpContext.Services.GetService<IJsonFormatter>().Serialize(context.Arguments[0], options);
            Assert.True(body.SequenceEqual(target));
        }
    }
}
