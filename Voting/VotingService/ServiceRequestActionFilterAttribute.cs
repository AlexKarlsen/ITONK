﻿using System;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace VotingService
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    internal sealed class ServiceRequestActionFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var activityId = Guid.NewGuid().ToString();
            ServiceEventSource.Current.ServiceRequestStart(actionContext.ActionDescriptor.ActionName, activityId);
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            ServiceEventSource.Current.ServiceRequestStop(actionExecutedContext.ActionContext.ActionDescriptor.ActionName,
                actionExecutedContext.Exception?.ToString() ?? string.Empty);
        }
    }
}
