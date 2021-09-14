using MarketingBox.AffiliateApi.Pagination;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MarketingBox.AfffliateApi.Models.CampaignBoxes;
using MarketingBox.Affiliate.Service.Grpc;
using MarketingBox.Affiliate.Service.Grpc.Models.CampaignBoxes.Requests;
using MarketingBox.AffiliateApi.Extensions;
using MarketingBox.AffiliateApi.Models.CampaignBoxes;
using MarketingBox.AffiliateApi.Models.CampaignBoxes.Requests;
using MarketingBox.AffiliateApi.Models.Campaigns;
using MarketingBox.AffiliateApi.Models.Campaigns.Requests;
using Microsoft.AspNetCore.Authorization;
using CampaignBoxCreateRequest = MarketingBox.AffiliateApi.Models.CampaignBoxes.Requests.CampaignBoxCreateRequest;
using CampaignBoxUpdateRequest = MarketingBox.AffiliateApi.Models.CampaignBoxes.Requests.CampaignBoxUpdateRequest;

namespace MarketingBox.AffiliateApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("/api/campaign-boxes")]
    public class CampaignBoxController : ControllerBase
    {
        private readonly ICampaignBoxService _campaignBoxService;

        public CampaignBoxController(ICampaignBoxService campaignBoxService)
        {
            _campaignBoxService = campaignBoxService;
        }

        /// <summary>
        /// </summary>
        /// <remarks>
        /// </remarks>
        [HttpGet]
        [ProducesResponseType(typeof(Paginated<CampaignBoxModel, long>), StatusCodes.Status200OK)]

        public async Task<ActionResult<Paginated<CampaignBoxModel, long>>> SearchAsync(
            [FromQuery] CampaignBoxesSearchRequest request)
        {
            if (request.Limit < 1 || request.Limit > 1000)
            {
                ModelState.AddModelError($"{nameof(request.Limit)}", "Should not be in the range 1..1000");

                return BadRequest();
            }

            //return Ok(
            //    many.Select(MapToResponse)
            //        .ToArray()
            //        .Paginate(request, Url, x => x.Id));

            return Ok();
        }

        /// <summary>
        /// </summary>
        /// <remarks>
        /// </remarks>
        [HttpGet("{campaignBoxId}")]
        [ProducesResponseType(typeof(CampaignBoxModel), StatusCodes.Status200OK)]

        public async Task<ActionResult<CampaignBoxModel>> GetAsync(
            [FromRoute, Required] long campaignBoxId)
        {
            var tenantId = this.GetTenantId();
            var response = await _campaignBoxService.GetAsync(new CampaignBoxGetRequest() { CampaignBoxId = campaignBoxId});

            return MapToResponse(response);
        }

        /// <summary>
        /// </summary>
        /// <remarks>
        /// </remarks>
        [HttpPost]
        [ProducesResponseType(typeof(CampaignBoxModel), StatusCodes.Status200OK)]
        public async Task<ActionResult<CampaignBoxModel>> CreateAsync(
            [FromBody] CampaignBoxCreateRequest request)
        {
            var tenantId = this.GetTenantId();
            var response = await _campaignBoxService.CreateAsync(new Affiliate.Service.Grpc.Models.CampaignBoxes.Requests.CampaignBoxCreateRequest()
            {
                ActivityHours = request.ActivityHours.Select(x => new Affiliate.Service.Grpc.Models.CampaignBoxes.ActivityHours()
                {
                    Day = x.Day,
                    From = x.From,
                    IsActive = x.IsActive,
                    To = x.To
                }).ToArray(),
                BoxId = request.BoxId,
                CampaignId = request.CampaignId,
                CapType = request.CapType.MapEnum< Affiliate.Service.Grpc.Models.CampaignBoxes.CapType> (),
                CountryCode = request.CountryCode,
                DailyCapValue = request.DailyCapValue,
                EnableTraffic = request.EnableTraffic,
                Information = request.Information,
                Priority = request.Priority,
                Weight = request.Weight
            });

            return MapToResponse(response);
        }

        /// <summary>
        /// </summary>
        /// <remarks>
        /// </remarks>
        [HttpPut("{campaignBoxId}")]
        [ProducesResponseType(typeof(CampaignBoxModel), StatusCodes.Status200OK)]
        public async Task<ActionResult<CampaignBoxModel>> UpdateAsync(
            [Required, FromRoute] long campaignBoxId,
            [FromBody] CampaignBoxUpdateRequest request)
        {
            var response = await _campaignBoxService.UpdateAsync(new Affiliate.Service.Grpc.Models.CampaignBoxes.Requests.CampaignBoxUpdateRequest()
            {
                Sequence = request.Sequence,
                CampaignBoxId = campaignBoxId,
                ActivityHours = request.ActivityHours.Select(x => new Affiliate.Service.Grpc.Models.CampaignBoxes.ActivityHours()
                {
                    Day = x.Day,
                    From = x.From,
                    IsActive = x.IsActive,
                    To = x.To
                }).ToArray(),
                BoxId = request.BoxId,
                CampaignId = request.CampaignId,
                CapType = request.CapType.MapEnum<Affiliate.Service.Grpc.Models.CampaignBoxes.CapType>(),
                CountryCode = request.CountryCode,
                DailyCapValue = request.DailyCapValue,
                EnableTraffic = request.EnableTraffic,
                Information = request.Information,
                Priority = request.Priority,
                Weight = request.Weight
            });

            return MapToResponse(response);
        }

        /// <summary>
        /// </summary>
        /// <remarks>
        /// </remarks>
        [HttpDelete("{campaignBoxId}")]
        [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
        public async Task<ActionResult> DeleteAsync(
            [Required, FromRoute] long campaignBoxId)
        {
            var response = await _campaignBoxService.DeleteAsync(
                new Affiliate.Service.Grpc.Models.CampaignBoxes.Requests.CampaignBoxDeleteRequest()
            {
                CampaignBoxId = campaignBoxId,
            });

            return MapToResponseEmpty(response);
        }

        private ActionResult MapToResponse(Affiliate.Service.Grpc.Models.CampaignBoxes.CampaignBoxResponse response)
        {
            if (response.Error != null)
            {
                ModelState.AddModelError("", response.Error.Message);

                return BadRequest(ModelState);
            }

            if (response.CampaignBox == null)
                return NotFound();

            return Ok(new CampaignBoxModel()
            {
                BoxId = response.CampaignBox.BoxId,
                CampaignId = response.CampaignBox.CampaignId,
                ActivityHours = response.CampaignBox.ActivityHours.Select(x => new ActivityHours()
                {
                    Day = x.Day,
                    From = x.From,
                    IsActive = x.IsActive,
                    To = x.To
                }).ToArray(),
                CampaignBoxId = response.CampaignBox.CampaignBoxId,
                CapType = response.CampaignBox.CapType.MapEnum<CapType>(),
                CountryCode = response.CampaignBox.CountryCode,
                DailyCapValue = response.CampaignBox.DailyCapValue,
                EnableTraffic = response.CampaignBox.EnableTraffic,
                Information = response.CampaignBox.Information,
                Priority = response.CampaignBox.Priority,
                Weight = response.CampaignBox.Weight
            });
        }

        private ActionResult MapToResponseEmpty(Affiliate.Service.Grpc.Models.CampaignBoxes.CampaignBoxResponse response)
        {
            if (response.Error != null)
            {
                ModelState.AddModelError("", response.Error.Message);

                return BadRequest(ModelState);
            }

            return Ok();
        }
    }
}