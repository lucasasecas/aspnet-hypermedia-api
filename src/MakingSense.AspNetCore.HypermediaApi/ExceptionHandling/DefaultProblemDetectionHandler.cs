﻿using System;
using MakingSense.AspNetCore.Abstractions;
using MakingSense.AspNetCore.Authentication.Abstractions;
using MakingSense.AspNetCore.HypermediaApi.Problems;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace MakingSense.AspNetCore.HypermediaApi.ExceptionHandling
{
	public class DefaultProblemDetectionHandler : IContextProblemDetectionHandler, IExceptionProblemDetectionHandler
	{
		private readonly ILogger _logger;

		public DefaultProblemDetectionHandler(ILogger logger)
		{
			_logger = logger;
		}

		public Problem ExtractProblem(Exception exception)
		{
			var apiException = exception as ApiException;
			if (apiException != null)
			{
				return apiException.Problem;
			}

			var authException = exception as AuthenticationException;
			if (authException != null)
			{
				return new AuthenticationErrorProblem(exception.Message);
			}

			_logger.LogError(new EventId(), exception, "Unexpected error");
			return new UnexpectedProblem(exception);
		}

		public Maybe<Problem> CheckForProblem(HttpContext context)
		{
			switch (context.Response.StatusCode)
			{
				case StatusCodes.Status401Unauthorized:
					return new UnauthorizedProblem();
				case StatusCodes.Status403Forbidden:
					return new ForbiddenProblem();
				case StatusCodes.Status404NotFound:
					return new RouteNotFoundProblem();
			}
			return Maybe.None<Problem>();
		}
	}
}
