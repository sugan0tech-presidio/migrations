﻿namespace TodoApiService.Models;

public class TodoItem
{
    public int Id { get; set; }
    public string Task { get; set; }
    public bool IsCompleted { get; set; }
    public int UserId { get; set; } 
    public User User { get; set; }
}