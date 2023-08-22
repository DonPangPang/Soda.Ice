﻿using Soda.Ice.Abstracts;
using System.ComponentModel.DataAnnotations.Schema;

namespace Soda.Ice.Domain;

public class Blog : EntityBase, ICreator, IModifior
{
    public string Title { get; set; } = string.Empty;

    public ICollection<BlogGroup> BlogGroups { get; set; } = new HashSet<BlogGroup>();
    public ICollection<BlogTag> BlogTags { get; set; } = new HashSet<BlogTag>();

    [NotMapped]
    public string Descrption => Content[..(Content.Length > 200 ? 200 : Content.Length)];

    public string Content { get; set; } = string.Empty;

    public Guid? LocalBlogFileResourceId { get; set; }
    public FileResource? LocalBlogFileResource { get; set; }

    /// <summary>
    /// 图片资源列表
    /// </summary>
    public ICollection<FileResource> ImageFileResources { get; set; } = new List<FileResource>();

    public ICollection<BlogViewLog> BlogViewLogs { get; set; } = new List<BlogViewLog>();

    public ICollection<Comment> Comments { get; set; } = new List<Comment>();

    [NotMapped]
    public int BlogViewLogCount => BlogViewLogs.Count;

    [NotMapped]
    public int CommentsLength => Comments.Count;

    public Guid CreatorId { get; set; }
    public User? Creator { get; set; }
    public DateTime CreateTime { get; set; }
    public Guid? ModifiorId { get; set; }
    public User? Modifior { get; set; }
    public DateTime? UpdateTime { get; set; }
}