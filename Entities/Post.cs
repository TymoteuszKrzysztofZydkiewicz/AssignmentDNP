﻿namespace Entities;

public class Post
{
    public int Id { get; set; }
    public string Body { get; set; }
    public string Title { get; set; }
    public int UserId { get; set; }

    public Post(string body, string title, int userId)
    {
        Body = body;
        Title = title;
        UserId = userId;
    }
}