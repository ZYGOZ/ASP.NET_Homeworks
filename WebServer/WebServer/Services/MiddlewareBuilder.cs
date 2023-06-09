﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServer.Middlewares;

namespace WebServer.Services
{
    public class MiddlewareBuilder
    {
        private Stack<Type>? middlewares = new(); // LoggerMiddleware StaticFilesMiddleware


        public void Use<T>() where T : IMiddleware
        {
            middlewares?.Push(typeof(T));
        }

        public HttpHandler Build()
        {
            HttpHandler handler = context => context.Response.Close();

            while (middlewares?.Count != 0)
            {
                var type = middlewares?.Pop();
                var middleware = Activator.CreateInstance(type) as IMiddleware;

                middleware.Next = handler;
                handler = middleware.Handle;

            }
            return handler;

        }
    }
}
