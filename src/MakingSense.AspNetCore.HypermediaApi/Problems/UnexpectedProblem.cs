﻿using MakingSense.AspNetCore.HypermediaApi.ExceptionHandling;
using Microsoft.AspNetCore.Http;
using System;

namespace MakingSense.AspNetCore.HypermediaApi.Problems
{
	public class UnexpectedProblem : Problem
	{
		private Exception _exception;
		public override string title => "Unexpected error";
		public override int status => StatusCodes.Status500InternalServerError;
		public override int errorCode => 0;
		public override string detail => $"An unexpected error was thrown/r/n Exception Message: {_exception.Message} /r/n Exception Type: {_exception.GetType()}";

		public UnexpectedProblem(Exception exception)
		{
			_exception = exception;
		}
	}
}
