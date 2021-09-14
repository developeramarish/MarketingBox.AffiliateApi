﻿using Autofac;
using MarketingBox.Affiliate.Service.Client;

namespace MarketingBox.AffiliateApi.Modules
{
    public class ServiceModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAffiliateServiceClient(Program.Settings.AffiliateServiceUrl);
        }
    }
}
