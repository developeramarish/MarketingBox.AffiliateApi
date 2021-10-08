﻿using System.Runtime.Serialization;

namespace MarketingBox.Reporting.Service.Grpc.Models.Leads
{
    public class LeadRouteInfo
    {
        public long AffiliateId { get; set; }

        public long BoxId { get; set; }

        public long CampaignId { get; set; }

        public long BrandId { get; set; }
    }
}


