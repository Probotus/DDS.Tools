﻿using System.Diagnostics.CodeAnalysis;

using DDS.Tools.Enumerators;
using DDS.Tools.Interfaces.Services;
using DDS.Tools.Models;
using DDS.Tools.Settings;

using Microsoft.Extensions.Logging;

using Spectre.Console;
using Spectre.Console.Cli;

namespace DDS.Tools.Commands;

/// <summary>
/// The dds convert command class.
/// </summary>
/// <param name="loggerService">The logger service instance to use.</param>
/// <param name="todoService">The todo service instance to use.</param>
internal sealed class DdsConvertCommand(ILoggerService<DdsConvertCommand> loggerService, ITodoService todoService) : Command<DdsConvertSettings>
{
	private const ImageType Type = ImageType.DDS;
	private readonly ILoggerService<DdsConvertCommand> _loggerService = loggerService;
	private readonly ITodoService _todoService = todoService;

	private static readonly Action<ILogger, Exception?> LogException =
		LoggerMessage.Define(LogLevel.Error, 0, "Exception occured.");

	/// <inheritdoc/>
	public override int Execute([NotNull] CommandContext context, [NotNull] DdsConvertSettings settings)
	{
		return AnsiConsole.Status()
			.Spinner(Spinner.Known.Line)
			.Start("Processing..", action => Action(settings));
	}

	private int Action(DdsConvertSettings settings)
	{
		try
		{
			TodoCollection todods = _todoService.GetTodos(settings, Type);

			if (todods.Count.Equals(0))
			{
				AnsiConsole.Markup($"[yellow]There is nothing todo![/]");
				return 0;
			}

			return 0;
		}
		catch (Exception ex)
		{
			_loggerService.Log(LogException, ex);
			AnsiConsole.Markup($"[maroon]{ex.Message}[/]");
			return 1;
		}
	}
}
