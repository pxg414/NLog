﻿using NLog;
using System;
using Unity.Builder;
using Unity.Extension;
using Unity.Policy;

namespace Unity.NLog
{
    public class NLogExtension : UnityContainerExtension, IBuildPlanPolicy
    {
        private static readonly Func<Type, string, string> _defaultGetName = (t, n) => t.FullName;

        public Func<Type, string, string> GetName { get; set; }

        protected override void Initialize()
        {
            Context.Policies.Set(typeof(ILogger), null, typeof(IBuildPlanPolicy), this);
        }

        public void BuildUp(ref BuilderContext context)
        {
            Func<Type, string, string> method = GetName ?? _defaultGetName;
            context.Existing = LogManager.GetLogger(method(context.Parent?.Type,
                                                           context.Parent?.Name));
            context.BuildComplete = true;
        }
    }
}
