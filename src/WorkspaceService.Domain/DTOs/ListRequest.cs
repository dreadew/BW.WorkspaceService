﻿namespace WorkspaceService.Domain.DTOs;

public class ListRequest
{
    public int Limit { get; set; } = 20;
    public int Offset { get; set; }
}