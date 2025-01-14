using System;
using System.Collections.Generic;
using GitUIPluginInterfaces;

namespace GitCommands
{
    public sealed class CommitData
    {
        public CommitData(
            ObjectId objectId,
            ObjectId? treeGuid,
            IReadOnlyList<ObjectId>? parentIds,
            string author,
            DateTime authorDate,
            string committer,
            DateTime commitDate,
            string body)
        {
            ObjectId = objectId;
            TreeGuid = treeGuid;
            ParentIds = parentIds;
            Author = author;
            AuthorDate = authorDate.ToDateTimeOffset();
            Committer = committer;
            CommitDate = commitDate.ToDateTimeOffset();
            Body = body;
        }

        public ObjectId ObjectId { get; }
        public ObjectId? TreeGuid { get; } // TODO nothing seems to be using this value
        public IReadOnlyList<ObjectId>? ParentIds { get; }
        public string Author { get; }
        public DateTimeOffset AuthorDate { get; }
        public string Committer { get; }
        public DateTimeOffset CommitDate { get; }

        // TODO mutable properties need review

        public IReadOnlyList<ObjectId>? ChildIds { get; set; }

        /// <summary>
        /// Gets and sets the commit message.
        /// </summary>
        public string Body { get; set; }
    }
}
