﻿namespace Byndyusoft.Dotnet.Core.Samples.Web.Application.Controllers.ValuesController
{
    using System;
    using Core.Infrastructure.CQRS.Abstractions.Commands;
    using Core.Infrastructure.CQRS.Abstractions.Queries;
    using Domain.CommandsContexts.Values;
    using Domain.Criterions.Values;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    [Route("values")]
    public class ValuesController : Controller
    {
        private readonly IQueriesDispatcher _queriesDispatcher;
        private readonly ICommandsDispatcher _commandsDispatcher;
        private readonly ILogger<ValuesController> _logger;

        private readonly ValuesControllerOptions _controllerOptions;

        public ValuesController(IQueriesDispatcher queriesDispatcher, ICommandsDispatcher commandsDispatcher, ILogger<ValuesController> logger, IOptionsSnapshot<ValuesControllerOptions> options)
        {
            if(queriesDispatcher == null)
                throw new ArgumentNullException(nameof(queriesDispatcher));
            if (commandsDispatcher == null)
                throw new ArgumentNullException(nameof(commandsDispatcher));
            if(logger == null)
                throw new ArgumentNullException(nameof(logger));
            if(options == null)
                throw new ArgumentNullException(nameof(options));

            _queriesDispatcher = queriesDispatcher;
            _commandsDispatcher = commandsDispatcher;
            _logger = logger;

            _controllerOptions = options.Value;
        }

        /// <summary>
        /// Provides values array
        /// </summary>
        /// <returns>Configuration values</returns>
        [HttpGet]
        public string[] Get()
        {
            _logger.LogInformation("Get values request");
            return _controllerOptions.Values;
        }

        /// <summary>
        /// Provide  value by id <paramref name="id"/>
        /// </summary>
        /// <param name="id">Value index</param>
        /// <returns>Value for id</returns>
        [HttpGet("{id}")]
        public string Get(int id)
        {
            _logger.LogInformation("Get value request");
            return _queriesDispatcher.Execute(new GetValueQueryCriterion(id));
        }

        /// <summary>
        /// Save value <paramref name="value"/> for id <paramref name="id"/>
        /// </summary>
        /// <param name="id">Value index</param>
        /// <param name="value">New value</param>
        [HttpPost("{id}")]
        public void Post(int id, [FromBody] string value)
        {
            _logger.LogInformation("Validation and post value request");
            _commandsDispatcher.Execute(new SetValueCommandContext(id, value));
        }
    }
}