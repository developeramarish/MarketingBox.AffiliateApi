﻿using System;
using MarketingBox.AffiliateApi.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace MarketingBox.AffiliateApi.Models.Reports.Requests
{
    public class LeadSearchRequest : PaginationRequest<long?>
    {
    }
}