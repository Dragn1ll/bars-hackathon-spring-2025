﻿namespace TelegramBotWorkerService.Models;

public class CommandAttribute(string name): Attribute
{
    public string Name { get; } = name;
}