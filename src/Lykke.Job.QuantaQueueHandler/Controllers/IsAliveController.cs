﻿using System;
using System.Net;
using Lykke.Job.QuantaQueueHandler.Core.Services;
using Lykke.Job.QuantaQueueHandler.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.SwaggerGen.Annotations;

namespace Lykke.Job.QuantaQueueHandler.Controllers
{
    [Route("api/[controller]")]
    public class IsAliveController : Controller
    {
        private readonly IHealthService _healthService;

        public IsAliveController(IHealthService healthService)
        {
            _healthService = healthService;
        }

        /// <summary>
        /// Checks service is alive
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerOperation("IsAlive")]
        [ProducesResponseType(typeof(IsAliveResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.InternalServerError)]
        public IActionResult Get()
        {
            var healthViloationMessage = _healthService.GetHealthViolationMessage();
            if (healthViloationMessage != null)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ErrorResponse
                {
                    ErrorMessage = $"Job is unhealthy: {healthViloationMessage}"
                });
            }

            // NOTE: Feel free to extend IsAliveResponse, to display job-specific health status
            return Ok(new IsAliveResponse
            {
                Version = Microsoft.Extensions.PlatformAbstractions.PlatformServices.Default.Application.ApplicationVersion,
                Env = Environment.GetEnvironmentVariable("ENV_INFO"),
                LastMessageProcessingStartedMoment = _healthService.LastMessageProcessingStartedMoment,
                LastMessageProcessingDuration = _healthService.LastMessageProcessingDuration,
                MessageProcessingIdleDuration = _healthService.MessageProcessingIdleDuration,
                MessageProcessingFailedInARow = _healthService.MessageProcessingFailedInARow,
                MaxHealthyMessageProcessingDuration = _healthService.MaxHealthyMessageProcessingDuration,
                MaxHealthyMessageProcessingIsIdleDuration = _healthService.MaxHealthyMessageProcessingIdleDuration,
                MaxHealthyMessageProcessingFailedInARow = _healthService.MaxHealthyMessageProcessingFailedInARow
            });
        }
    }
}