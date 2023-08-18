using Soda.Ice.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Soda.Ice.Shared.ViewModels;

public record VBlogGroup : ViewModel
{
    public string Name { get; set; } = string.Empty;

    public ICollection<VBlog> Blogs { get; set; } = new List<VBlog>();
}